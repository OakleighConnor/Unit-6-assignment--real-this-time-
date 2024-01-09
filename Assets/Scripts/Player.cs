using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    Animator anim;
    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    Vector3 velocity;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 3f;

    public bool jumping = false;

    public bool isGrounded;
    void Start()
    {
        anim = GameObject.Find("GFX").GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isGrounded && !jumping)
        {
            anim.SetBool("falling", true);
        }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        AnimationReset();
        Movement();
        Gravity();
    }
    void Movement()
    {

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            anim.SetBool("grounded", true);
            jumping = false;
        }


        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumping = true;
            anim.SetBool("grounded", false);
            anim.SetBool("fall", false);
        }
        //Gravity
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //This moves the player 
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * speed * Time.deltaTime);
            if (isGrounded)
            {
                anim.SetBool("walk", true);
            }
        }
    }
    void Gravity()
    {
        //Gravity
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
    void AnimationReset()
    {
        anim.SetBool("walk", false);
        
    }
}
