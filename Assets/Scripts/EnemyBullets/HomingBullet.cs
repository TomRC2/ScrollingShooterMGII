using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    public float speed = 10f;
    public float rotateSpeed = 5f;
    private Transform target;
    public float lifetime = 5f;
    private float lifeTimer;
    public float damage = 15f;
    void Start()
    {
        target = GameObject.FindWithTag("Player")?.transform;
        lifeTimer = lifetime;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position);
        direction.y = 0f;
        direction.Normalize();

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        transform.position += direction * speed * Time.deltaTime;

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null && other.CompareTag("Player"))
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
