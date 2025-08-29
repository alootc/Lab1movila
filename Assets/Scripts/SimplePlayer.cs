using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class SimplePlayer : NetworkBehaviour
{
    public float speed = 5f;
    public float jump = 5f;
    private Rigidbody rb;
    private bool isGrounded = false;
    private LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;

    public static event Action<Transform> OnLocalPlayerSpawned;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        groundLayer = LayerMask.GetMask("Ground");

        if (IsOwner)
        {
            OnLocalPlayerSpawned?.Invoke(transform);
        }
    }

    private void Update()
    {
        if (!IsOwner) return;

        float x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        float y = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        transform.position += new Vector3(x, 0f, y);

        CheckGrounded();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void CheckGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundLayer);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckDistance);
    }
}

