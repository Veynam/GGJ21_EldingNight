using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Variables
    public Rigidbody target;
    private float horizontal;
    private float vertical;

    public float speed = 100;
    public float jumpForce = 5f;

    // Ground check
    public LayerMask groundMask;
    public bool isGrounded;
    #endregion

    #region Methods
    void Awake()
    {
        target = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal") * speed;
        vertical = Input.GetAxisRaw("Vertical") * speed;

        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.4f, groundMask); // -1 makes th sphere check below the player feet.

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            target.velocity = new Vector3(target.velocity.x, jumpForce, target.velocity.y);

        // Moving
        Vector3 move = (transform.right * horizontal + transform.forward * vertical).normalized * speed;
        Vector3 newMove = new Vector3(move.x, target.velocity.y, move.z);

        target.velocity = newMove;
    }
    #endregion
}
