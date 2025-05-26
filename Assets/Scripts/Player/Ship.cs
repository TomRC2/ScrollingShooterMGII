using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Ship : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public Vector2 moveInput;
    [Header("UI")]
    public Slider healthSlider;
    [Header("Ammo")]
    public int maxAmmo = 150;
    public int currentAmmo;
    public UnityEngine.UI.Slider ammoSlider;
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float fireRate = 0.25f;
    private float fireCooldown;

    [Header("Limites de Movimiento")]
    public float minX = -5f, maxX = 5f;
    public float minZ = -5f, maxZ = 5f;

    private PlayerInputActions inputActions;
    public GameObject LosePanel;
    public float health = 100f;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Fire.performed += ctx => TryShoot();
    }
    void Start()
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }
        currentAmmo = maxAmmo;

        if (ammoSlider != null)
        {
            ammoSlider.maxValue = maxAmmo;
            ammoSlider.value = currentAmmo;
        }
        Time.timeScale = 1f;
    }

    void OnEnable() => inputActions.Enable();
    void OnDisable() => inputActions.Disable();

    void Update()
    {
        Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, minX, maxX);
        clampedPos.z = Mathf.Clamp(clampedPos.z, minZ, maxZ);
        transform.position = clampedPos;

        if (inputActions.Player.Fire.ReadValue<float>() > 0 && fireCooldown <= 0f)
        {
            TryShoot();
        }

        if (fireCooldown > 0f)
            fireCooldown -= Time.deltaTime;
    }

    void TryShoot()
    {
        if (fireCooldown > 0f || currentAmmo <= 0) return;

        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        currentAmmo--;
        fireCooldown = fireRate;

        if (ammoSlider != null)
            ammoSlider.value = currentAmmo;
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Jugador recibió daño: " + amount);

        if (healthSlider != null)
            healthSlider.value = health;

        if (health <= 0f)
        {
            Debug.Log("Jugador muerto!");
            Destroy(gameObject);
            Time.timeScale = 0f;

            if (LosePanel != null)
                LosePanel.SetActive(true);
        }
    }
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
        if (ammoSlider != null)
            ammoSlider.value = currentAmmo;
    }
}