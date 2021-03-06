using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Variables

    [Header("Player Sound Elements")]
    public AudioClip[] FootSteps;

    public Rigidbody target;
    private float horizontal;
    private float vertical;

    public float speed = 4f;
    public float jumpForce = 5f;

    // Ground check
    public LayerMask groundMask;
    public bool isGrounded;

    // Ceiling check
    public LayerMask ceilingMask;
    public bool touchingCeiling;

    private Vector3 newMove;

    private AudioSource MovementSound;

    private bool isMoving;

    CapsuleCollider playerCol;
    float originalHeight;
    public float reducedHeight;

    // Sprint
    public bool isSprinting;

    public bool dead;
    #endregion

    #region Methods
    void Awake()
    {
        target = GetComponent<Rigidbody>();
        MovementSound = GetComponent<AudioSource>();

        playerCol = GetComponent<CapsuleCollider>();
        originalHeight = playerCol.height;
    }

    void Update()
    {
        if(!dead)
		{
            horizontal = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
            vertical = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

            // Check if the player is touching the ceiling
            touchingCeiling = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), 0.4f, ceilingMask); // +1 checks above the player

            // Check if the player is grounded
            isGrounded = Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.4f, groundMask); // -1 makes th sphere check below the player feet.

            // Jumping
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !touchingCeiling)
                target.velocity = new Vector3(target.velocity.x, jumpForce, target.velocity.y);

            // Moving
            MovementSounds();
            Vector3 move = (transform.right * horizontal + transform.forward * vertical).normalized * speed;
            newMove = new Vector3(move.x, target.velocity.y, move.z);

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

            if (Input.GetKey(KeyCode.LeftControl))
            {
                Crouch();
            }
            else
            {
                GetUp();
            }
        }
    }

    // Increase speed
    void Sprint()
    {
        if (speed <= 7.0f)
        {
            speed += 2f * Time.deltaTime;
        }   
    }
    void StopSprint()
    {
        if (speed >= 4.0f)
        {
            speed -= 4f * Time.deltaTime;
        }
    }

    private void MovementSounds()
    {
        if (newMove.magnitude > 0.1)
        {
            isMoving = true;
        }
        else isMoving = false;

        if (isMoving && isGrounded)
        {
            if (MovementSound.isPlaying == false)
            {
                MovementSound.PlayOneShot(FootSteps[Random.Range(0, FootSteps.Length)]);
            }
        }
    }

    // Reduce height
    void Crouch()
    {
        playerCol.height = reducedHeight;
    }

    // Reset height
    void GetUp()
    {
        playerCol.height = originalHeight;
    }
    #endregion
}
