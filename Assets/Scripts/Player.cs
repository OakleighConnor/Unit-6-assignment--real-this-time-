using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Enemy enemyScript;

    //Variables
    float speed = 6f;
    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float gravity = -9.81f;
    float jumpHeight = 3f;
    float groundDistance = 0.4f;

    //State Checks
    bool jumping = false;
    public bool isGrounded;
    public bool attacking = false;

    //Components
    public CharacterController controller;
    public Transform cam;
    Animator anim;
    public Transform groundCheck;
    public LayerMask groundMask;

    //Vector3
    Vector3 velocity;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        enemyScript = GameObject.Find("Enemy").GetComponent<Enemy>();
        anim = GameObject.Find("GFX").GetComponent<Animator>();
        GetComponent<BoxCollider>().enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        AnimationReset();
        if (!attacking)
        {
            Movement();
            Attack();
        }
        Gravity();
        if (!isGrounded && !jumping)
        {
            anim.SetBool("fall", true);
        }
    }
    void Movement()
    {
        //Gravity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            anim.SetBool("grounded", true);
            jumping = false;
        }
        velocity.y += gravity * Time.deltaTime;

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

    void Attack()
    {
        if (isGrounded)
        {
            if (Input.GetMouseButton(0))
            {

                GetComponent<BoxCollider>().enabled = true;
                anim.SetTrigger("light");
                attacking = true;
            }
            if (Input.GetMouseButton(1))
            {
                GetComponent<BoxCollider>().enabled = true;
                anim.SetTrigger("heavy");
                attacking = true;
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
        anim.SetBool("fall", false);
    }
   void OnTriggerStay(Collider other)
   {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (attacking)
            {
                Destroy(other.gameObject);
            }
        }
    }
}