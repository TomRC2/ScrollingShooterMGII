using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
public class EnemyKillTracker : MonoBehaviour
{
    public int killsToSpawnBoss = 20;
    private int killCount = 0;

    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public GameObject enemySpawner;
    public GameObject bossHealthBarCanvas;

    public GameObject victoryPanel;
    public VisualEffect deathEffect;
    public Text killsRemainingText;
    public GameObject[] redHudBlocks;
    public Material redMaterial;

    private bool bossSpawned = false;

    void Start()
    {
        UpdateKillUI();
    }
    public void RegisterKill()
    {
        if (bossSpawned) return;

        killCount++;
        UpdateKillUI();

        if (killCount >= killsToSpawnBoss)
        {
            SpawnBoss();
        }
    }
    void UpdateKillUI()
    {
        if (killsRemainingText != null)
        {
            int remaining = Mathf.Max(0, killsToSpawnBoss - killCount);
            killsRemainingText.text = "Enemigos restantes: " + remaining;
        }
    }
    void SpawnBoss()
    {
        bossSpawned = true;

        GameObject boss = Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);

        BossController bossController = boss.GetComponent<BossController>();
        if (bossController != null)
        {
            bossController.victoryPanel = victoryPanel;
            bossController.deathEffectPrefab = deathEffect;
        }

        if (enemySpawner != null)
            enemySpawner.SetActive(false);

        if (bossHealthBarCanvas != null)
            bossHealthBarCanvas.SetActive(true);

        SetRedTheme();
    }

    void SetRedTheme()
    {
        foreach (GameObject block in redHudBlocks)
        {
            Renderer renderer = block.GetComponent<Renderer>();
            if (renderer != null && redMaterial != null)
                renderer.material = redMaterial;
        }
    }
}
