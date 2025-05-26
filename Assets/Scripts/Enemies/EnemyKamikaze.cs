using UnityEngine;

public class EnemyKamikaze : Enemy
{
    public float speed = 15f;
    public float damageToPlayer = 25f;
    void Update()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;
        transform.up = dir;
        transform.position += dir * speed * Time.deltaTime;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damageToPlayer);
            }

            Die();
        }
    }
}
