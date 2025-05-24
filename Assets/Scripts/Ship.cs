using UnityEngine;
using UnityEngine.InputSystem;

public class Ship : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public Vector2 moveInput;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public float fireRate = 0.25f;
    private float fireCooldown;

    [Header("Limites de Movimiento")]
    public float minX = -5f, maxX = 5f;
    public float minZ = -5f, maxZ = 5f;

    private PlayerInputActions inputActions;

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Fire.performed += ctx => TryShoot();
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
        if (fireCooldown > 0f) return;

        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        fireCooldown = fireRate;
    }
}