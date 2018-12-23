using UnityEngine;


public class Player3DExample : MonoBehaviour {

    public float moveSpeed = 8f;
    public Joystick joystick;
    private Animator m_Animator;
    public Transform slideTransform;
    private ParticleSystem slideParticle;


    public Vector3 moveDirection;
    public const float maxDashTime = 1.0f;
    public float dashDistance = 10f;
    public float dashStoppingSpeed = 0.1f;
    float currentDashTime = maxDashTime;
    float dashSpeed = 6f;



    private void Awake()
    {
        joystick = FindObjectOfType<Joystick>();
        m_Animator = gameObject.GetComponent<Animator>();
        slideParticle = slideTransform.GetComponent<ParticleSystem>();
        
    }
    void Update ()
    {
        JoystickKeyboardMoving();

        Dash();
    }

    public void Dash()
    {

        if (Input.GetButtonDown("Fire2"))
        {
            slideParticle.Play();
            m_Animator.Play("PlayerSlide");
            currentDashTime = 0f;
        }
        if (currentDashTime < maxDashTime)
        {

            moveDirection = transform.forward * dashDistance; 
            currentDashTime += dashStoppingSpeed;
            Debug.Log("dashing");
        }
        else
        {
            slideParticle.Stop();
            moveDirection = Vector3.zero;
        }
        transform.Translate(moveDirection * Time.deltaTime * dashSpeed, Space.World);
    }

    private void JoystickKeyboardMoving()
    {
        Vector3 moveVector = (Vector3.right * joystick.Horizontal + Vector3.forward * joystick.Vertical);
        Vector3 keyboardMoveVector = (Vector3.right * Input.GetAxis("Horizontal") + Vector3.forward * Input.GetAxis("Vertical"));
        if (moveVector != Vector3.zero)
        {
            if (!m_Animator.GetBool("isMouseDown"))
            {
                transform.rotation = Quaternion.LookRotation(moveVector);
            }

            transform.Translate(moveVector * moveSpeed * Time.deltaTime, Space.World);
        }
        if (keyboardMoveVector != Vector3.zero)
        {
            if (!m_Animator.GetBool("isMouseDown"))
            {
                transform.rotation = Quaternion.LookRotation(keyboardMoveVector);
            }

            transform.Translate(keyboardMoveVector * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}