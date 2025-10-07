using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] public float movingSpeed = 5f; 
    public float OriginalMovingSpeed { get; private set; }

    private float minMovementSpeed = 0.1f;
    private bool isRunning = false;
    private const string IS_RUNNING = "isRunning";
    private Vector2 lastInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        OriginalMovingSpeed = movingSpeed;
    }
    private void Update()
    {
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        Movement();
        animator.SetBool(IS_RUNNING, IsRunning());
        
    }

    
    private void Movement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVector();
        rb.linearVelocity = inputVector * movingSpeed; 
        lastInput = inputVector;
    }

    private void HandleAnimation()
    {
        if (Mathf.Abs(lastInput.x) > minMovementSpeed || Mathf.Abs(lastInput.y) > minMovementSpeed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (lastInput != Vector2.zero)
        {
            animator.SetFloat("Xinput", lastInput.x);
            animator.SetFloat("Yinput", lastInput.y);
        }
    }

    public bool IsRunning()
    {
        return isRunning;
    }

 
}
