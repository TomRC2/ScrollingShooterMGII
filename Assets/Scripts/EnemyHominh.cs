using UnityEngine;

public class EnemyHominh : Enemy
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float fireTimer;
    public float speed = 5f;

    protected override void Start()
    {
        base.Start();
        fireTimer = fireRate;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 dir = (player.position - transform.position).normalized;
        transform.up = dir;

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.FromToRotation(Vector3.up, transform.up));
            fireTimer = fireRate;
        }
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

}
