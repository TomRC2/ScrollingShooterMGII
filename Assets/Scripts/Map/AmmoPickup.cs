using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 10;
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
            Ship playerShip = other.GetComponent<Ship>();
            if (playerShip != null)
            {
                playerShip.AddAmmo(ammoAmount);
            }
            Destroy(gameObject);
        }
    }
}