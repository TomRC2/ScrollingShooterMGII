using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float healAmount = 20f;
    public float moveSpeed = 5f;
    private Transform player;
    public float attractRadius = 5f;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);
        if (dist < attractRadius)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageable playerDamageable = other.GetComponent<IDamageable>();
            if (playerDamageable != null)
            {
                playerDamageable.TakeDamage(-healAmount);
                Debug.Log("Curado: +" + healAmount);
            }
            Destroy(gameObject);
        }
    }
}