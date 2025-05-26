using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.VFX;
public class BossController : MonoBehaviour, IDamageable
{
    public GameObject projectilePrefab;
    public GameObject kamikazeEnemyPrefab;
    public Transform shootPoint;
    public float maxHealth = 300f;
    public float currentHealth;
    public float attackCooldown = 5f;
    private float attackTimer = 0f;
    private int attackPhase = 0;
    [HideInInspector] public GameObject victoryPanel;
    [HideInInspector] public VisualEffect deathEffectPrefab;
    void Start()
    {
        currentHealth = maxHealth;
        Time.timeScale = 1f;

        BossHealthHUD bossHud = FindObjectOfType<BossHealthHUD>();
        if (bossHud != null)
        {
            bossHud.boss = this;
            bossHud.SetRedTheme();
        }
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            switch (attackPhase)
            {
                case 0:
                    StartCoroutine(WideFanAttack());
                    break;
                case 1:
                    StartCoroutine(KamikazeSpawnAttack());
                    break;
                case 2:
                    StartCoroutine(DescendingWallAttack());
                    break;
            }

            attackPhase = (attackPhase + 1) % 3;
            attackTimer = attackCooldown;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log("Boss recibió daño: " + amount);

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Boss derrotado!");

        if (deathEffectPrefab != null)
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        Time.timeScale = 0f;
        Destroy(gameObject);
    }

    IEnumerator WideFanAttack()
    {
        int numProjectiles = 9;
        float angleRange = 90f;

        for (int i = 0; i < numProjectiles; i++)
        {
            float angle = -angleRange / 2 + (angleRange / (numProjectiles - 1)) * i;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * Vector3.back;

            ShootProjectile(dir);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator KamikazeSpawnAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPos = shootPoint.position + new Vector3(Random.Range(-4f, 4f), 0, 0);
            Instantiate(kamikazeEnemyPrefab, spawnPos, Quaternion.identity);
        }

        yield return null;
    }

    IEnumerator DescendingWallAttack()
    {
        int rows = 6;
        int cols = 10;
        float spacing = 1f;
        float descentSpeed = 1.5f;
        float zigzagAmplitude = 0.5f;
        float zigzagSpeed = 3f;

        Vector3 wallStart = new Vector3(0, shootPoint.position.y, shootPoint.position.z);

        float timeAlive = 0f;
        float wallLifetime = 6f;
        int safeCol = Random.Range(2, cols - 2);
        GameObject[,] wallProjectiles = new GameObject[rows, cols];

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (Mathf.Abs(c - safeCol) <= 1)
                    continue;

                Vector3 localOffset = new Vector3((c - cols / 2) * spacing, 0, -r * spacing);
                Vector3 spawnPos = wallStart + localOffset;

                GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
                wallProjectiles[r, c] = proj;
            }
        }

        while (timeAlive < wallLifetime)
        {
            timeAlive += Time.deltaTime;

            float verticalOffset = -timeAlive * descentSpeed;
            float zigzagOffset = Mathf.Sin(timeAlive * zigzagSpeed) * zigzagAmplitude;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    GameObject proj = wallProjectiles[r, c];
                    if (proj == null) continue;

                    Vector3 basePos = wallStart + new Vector3((c - cols / 2) * spacing, 0, -r * spacing);
                    proj.transform.position = basePos + new Vector3(zigzagOffset, 0, verticalOffset);
                }
            }

            yield return null;
        }

        foreach (var proj in wallProjectiles)
        {
            if (proj != null)
                Destroy(proj);
        }
    }

    void ShootProjectile(Vector3 direction)
    {
        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Projectile p = proj.GetComponent<Projectile>();
        if (p != null)
        {
            p.direction = direction;
            p.targetTag = "Player";
        }

    }
}
