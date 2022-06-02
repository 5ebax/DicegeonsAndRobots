using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/** Created by Sebatián Jiménez F.
 * Class Player Controller.
 */

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public int health;
    public int atkDmg;
    public float speed;
    public float atkRange;
    public bool shield;

    [Header("Sprites")]
    public Animator animPJ;
    public Animator animPJShadow;
    public SpriteRenderer spriteDron;
    public SpriteRenderer shadowSpritePJ;
    public SpriteRenderer shadowSpriteDron;

    private HealthControl healthControl;
    private ShieldControl shieldControl;
    private float turnSmoothTime = 0.1f;

    private float turnSmoothVelocity;
    private float inputHorizontal;
    private float inputVertical;
    private bool damaged;

    private SpriteRenderer spritePJ;
    private Transform mainCamera;
    private Vector3 direction;

    private Vector3 velocity;
    CinemachineFreeLook cam;
    private Rigidbody rb;

    public Transform dron;
    public Transform dronFirepoint;

    [Header("Ground")]
    private bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float gravity = -9.81F;
    private CharacterController characterController;

    [HideInInspector] public Vector3 hook;
     public bool hooked;
    [HideInInspector] public bool dead;
    private Vector3 lastPosition;
    private AudioManager audioM;
    private bool delay;


    private void Awake()
    {
        audioM = AudioManager.Instance;
        characterController = GetComponent<CharacterController>(); 
        Cursor.lockState = CursorLockMode.Confined;
        mainCamera = Camera.main.transform;
        healthControl = FindObjectOfType<HealthControl>();
        shieldControl = FindObjectOfType<ShieldControl>();

        spritePJ = GetComponentInChildren<SpriteRenderer>();
        cam = FindObjectOfType<CinemachineFreeLook>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        health = 6;
        atkDmg = 1;
        atkRange = 0.5F;
        shield = false;
    }

    private void Update()
    {
        if (!dead)
        {
            CameraMovement();

            inputHorizontal = Input.GetAxisRaw("Horizontal");
            inputVertical = Input.GetAxisRaw("Vertical");

            FlipSprites();

            direction = new Vector3(inputHorizontal, 0f, inputVertical).normalized;

            if (!hooked)
                Movement();
            else
            {
                characterController.Move(hook * 50 * Time.deltaTime);
            }
            lastPosition = characterController.transform.position;
        }
        else { transform.position = lastPosition; }
    }

    #region Movements
    private void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        else if (!isGrounded && velocity.y < 0.5F)
            velocity.y -= 10F * Time.deltaTime;

        if (direction.magnitude > 0.1f)
        {

            //Calculate the angles from direction and camera.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Move the player to camera direction.
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            characterController.Move(moveDir * speed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            if(!delay)
            StartCoroutine(Delay());
            animPJ.SetBool("Walk", true);
        }
        else
        {
            audioM.Stop("Walk");
            animPJ.SetBool("Walk", false);
        }
    }

    private void CameraMovement()
    {
        if(Input.GetMouseButton(1))
            cam.enabled = true;
        else if(Input.GetMouseButtonUp(1))
            cam.enabled = false;
    }

    private void FlipSprites()
    {
        if (inputHorizontal < 0)
        {
            spritePJ.flipX = true;
            spriteDron.flipX = true;
            dron.localRotation = new Quaternion(0,180, 0,0);
            dronFirepoint.localRotation = new Quaternion(0, 180, 0, 0);
            shadowSpritePJ.flipX = true;
            shadowSpriteDron.flipX = true;
        }
        else if (inputHorizontal > 0) 
        { 
            spritePJ.flipX = false;
            spriteDron.flipX = false;
            dron.localRotation = new Quaternion(0, 0, 0, 0);
            dronFirepoint.localRotation = new Quaternion(0, 0, 0, 0);
            shadowSpritePJ.flipX = false;
            shadowSpriteDron.flipX = false;
        }
    }
    #endregion

    #region Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blitz"))
            hooked = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        LoseHealth(collision.gameObject.GetComponentInParent<Enemies>().atkDmg,collision.transform.position);

        if (collision.gameObject.CompareTag("Hook"))
        {
            hooked = true;
            StartCoroutine(unKook());
        }
    }
    #endregion

    #region Others
    public void LoseHealth(int dmg, Vector3 pos)
    {
        if (!damaged && !dead)
        {
            damaged = true;
            Vector3 moveDirection = transform.position - pos;
            characterController.Move(moveDirection);

            StartCoroutine(DmgControl(dmg));
        }
    }

    private IEnumerator DmgControl(int dmg)
    {
        
        animPJ.SetTrigger("Hit");
        animPJShadow.SetTrigger("Hit");
        audioM.PlayOneShot("Hit");
        audioM.PlayOneShot("Hit2");
        if (!shield)
            {
                health -= dmg;
                healthControl.ReduceHealth(health);
            }
            else if (shield)
            {
                shield = !shield;
                shieldControl.Shield(shield);
            }
        if (health < 1)
        {
            dead = true;
            animPJ.Play("Dead");
            animPJShadow.Play("Dead");
            audioM.PlayOneAtTime("Muerte1");
            audioM.PlayOneAtTime("Muerte2");
        }
            yield return new WaitForSeconds(1.5F);
        damaged = false;
    }

    private IEnumerator unKook()
    {
        yield return new WaitForSeconds(5f);
        if (hooked)
            transform.position = Vector3.zero;
    }

    //Programacion de emergencia.
    private IEnumerator Delay()
    {
        delay = true;
        audioM.PlayOneAtTime("Walk");
        yield return new WaitForSeconds(0.4F);
        delay = false;
    }
    #endregion
}