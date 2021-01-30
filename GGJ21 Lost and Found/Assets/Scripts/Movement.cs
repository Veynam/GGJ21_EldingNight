using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Variables
    public Rigidbody target;
    private float horizontal;
    private float vertical;

    public float speed = 5f;
    public float jumpForce = 5f;

    // Ground check
    public LayerMask groundMask;
    public bool isGrounded;

    // Ceiling check
    public LayerMask ceilingMask;
    public bool touchingCeiling;

    //CapsuleCollider playerCol;
    //float originalHeight;
    //public float reducedHeight;

    // Sprint
    public bool isSprinting;
    #endregion

    #region Methods
    void Awake()
    {
        target = GetComponent<Rigidbody>();

        //playerCol = GetComponent<CapsuleCollider>();
        //originalHeight = playerCol.height;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal") * speed;
        vertical = Input.GetAxisRaw("Vertical") * speed;

        // Check if the player is touching the ceiling
        touchingCeiling = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), 0.4f, ceilingMask); // +1 checks above the player

        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.4f, groundMask); // -1 makes th sphere check below the player feet.

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !touchingCeiling)
            target.velocity = new Vector3(target.velocity.x, jumpForce, target.velocity.y);

        // Moving
        Vector3 move = (transform.right * horizontal + transform.forward * vertical).normalized * speed;
        Vector3 newMove = new Vector3(move.x, target.velocity.y, move.z);

        target.velocity = newMove;

        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            Sprint();
            isSprinting = true;
        }
        else if (isGrounded)
        {
            StopSprint();
            isSprinting = false;
        }
    }

    // Increase speed
    void Sprint()
    {
        if (speed <= 8.0f)
        {
            speed += 2f * Time.deltaTime;
        }   
    }
    void StopSprint()
    {
        if (speed >= 5.0f)
        {
            speed -= 4f * Time.deltaTime;
        }
    }

    // Reduce height
    //void Crouch()
    //{
    //    playerCol.height = reducedHeight;
    //}

    //// Reset height
    //void GetUp()
    //{
    //    playerCol.height = originalHeight;
    //}
    #endregion
}
