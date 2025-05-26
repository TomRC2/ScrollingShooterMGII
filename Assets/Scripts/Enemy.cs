using UnityEngine;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour, IDamageable
{
    protected Transform player;
    public VisualEffect deathVFX;
    public GameObject ammoDropPrefab;
    public GameObject healthDropPrefab;
    public float dropChanceAmmo = 0.75f;
    public float dropChanceHealth = 0.25f;
    public int health = 3;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= Mathf.CeilToInt(damage);
        Debug.Log($"Enemy took damage: {damage}, health now: {health}");

        if (health <= 0)
            Die();
    }
    void TryDropItem()
    {
        float roll = Random.value;

        if (roll <= dropChanceAmmo && ammoDropPrefab != null)
        {
            Instantiate(ammoDropPrefab, transform.position, Quaternion.identity);
        }
        else if (healthDropPrefab != null)
        {
            Instantiate(healthDropPrefab, transform.position, Quaternion.identity);
        }
    }
    protected virtual void Die()
    {
        if (deathVFX != null)
        {
            VisualEffect vfx = Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(vfx.gameObject, 2f);
        }
        Debug.Log("Enemy died");
        Destroy(gameObject);
        TryDropItem();
    }
}