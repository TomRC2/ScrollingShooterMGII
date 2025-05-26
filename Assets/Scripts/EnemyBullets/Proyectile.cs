using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10f;
    public float damage = 1f;
    public Vector3 direction;
    public string targetTag = "Player";
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction.normalized * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(targetTag)) return;

        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(damage);
            Debug.Log("Projectile hit and did " + damage + " damage");
        }

        Destroy(gameObject);
    }

}