using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoPlayer : MonoBehaviour
{
    public float velocidad = 5f;      // velocidad en X (lados) y Z (adelante/atrás)
    public float fuerzaSalto = 5f;    // fuerza del salto en Y

    private Rigidbody rb;
    private CapsuleCollider col;
    private float horizontal;   // A/D  -> eje X
    private float vertical;     // W/S  -> eje Z
    private bool quiereSaltar;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        // Bloquea toda rotación: la cápsula siempre se mantiene vertical
        rb.freezeRotation = true;
    }

    void Update()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        horizontal = 0f;
        vertical = 0f;

        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)  horizontal -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) horizontal += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed)  vertical   -= 1f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed)    vertical   += 1f;

        if (kb.spaceKey.wasPressedThisFrame) quiereSaltar = true;
    }

    void FixedUpdate()
    {
        // Movimiento en X y Z, conservando la velocidad en Y (gravedad/salto)
        Vector3 direccion = new Vector3(horizontal, 0f, vertical).normalized;

        Vector3 v = rb.linearVelocity;
        v.x = direccion.x * velocidad;
        v.z = direccion.z * velocidad;
        rb.linearVelocity = v;

        // Salto solo si está tocando el suelo
        if (quiereSaltar && EstaEnSuelo())
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
        quiereSaltar = false;
    }

    bool EstaEnSuelo()
    {
        return Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f);
    }
}