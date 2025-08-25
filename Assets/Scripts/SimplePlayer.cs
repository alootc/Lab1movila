using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class SimplePlayer : NetworkBehaviour
{
    public float speed = 5f;
    public float jump = 5f;
    private Rigidbody rb;
    private bool isGrounded = false;
    private LayerMask groundLayer;


    void Start()
    {
        // GetComponent<NetworkTransform>().AuthorityMode = NetworkTransform.AuthorityModes.Owner;

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        groundLayer = LayerMask.GetMask("Ground");
       
    }

    private void Update()
    {
        if (!IsOwner) return;

        float x = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        float y = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

        transform.position += new Vector3(x, 0f, y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
        isGrounded = false;
    }
}

