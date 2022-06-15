using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] GameObject InputManagerObj;
    [SerializeField] GameObject CanvasControllerObj;
    private ManageInputs inputManager;
    public CanvasController cv;
    [SerializeField] float speed, jump, maxSpeedX, maxFalling;
    [SerializeField] float coyoteTime; // how late can I press jump after falling and still jump
    [SerializeField] private float jumpInputStorage; // how early can the jump button be pressed before hitting ground
    [SerializeField] private float jumpCancelChangeCoeff; // positive vertical speed loss coefficient (0 to 1) that applies when jump is released prematurely 
    [SerializeField] private float minTimeBetweenJumps = 0.25f;
    [SerializeField] private AudioSource[] sfx;
    private GameObject checkpoint; // the transform of this object is where the player respawns.
    private Animator anim;

    private bool playing = true; // Whether or not the player is in a cutscene state. When true player will not be controllable and will not interact with objects, but still have physics.

    private int score = 0;
    private Rigidbody2D rb;

    [SerializeField] private float timeSinceLand, timeSinceJumpPress, timeSinceJump; 
    [SerializeField] private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManagerObj.GetComponent<ManageInputs>();
        cv = CanvasControllerObj.GetComponent<CanvasController>();
        cv.SetScore(score);

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, 0);
        
         
    }

    // Update is called once per frame
    void Update() 
    {
        // Handle Animations
        anim.SetFloat("X Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Y Speed", rb.velocity.y);
        if(Mathf.Abs(rb.velocity.x) > 0.05f) transform.localScale = rb.velocity.x > 0 ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    private void FixedUpdate()
    {
        if (playing)
        {
            rb.velocity += new Vector2(speed * inputManager.GetJoystick().x, 0) * Time.deltaTime;
            rb.velocity = new Vector2(.75f * Mathf.Sign(rb.velocity.x) * Mathf.Min(maxSpeedX, Mathf.Abs(rb.velocity.x)), Mathf.Max(maxFalling, rb.velocity.y));

            JumpHandler();
        }
    }

    public void collectCoin()
    {
        GetComponentsInChildren<AudioSource>()[6].Play(); // Collect SFX
        score++;
        cv.SetScore(score);
    }

    void Jump()
    {
        GetComponentsInChildren<AudioSource>()[1].Play(); // Jump SFX
        rb.velocity = new Vector2(rb.velocity.x, jump);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Killing") && playing)
        {
            playing = false;
            anim.Play("Hurt");
            float launchAngle = Random.Range(Mathf.PI/4, 3*Mathf.PI/4);
            float launchMagnitude = Random.Range(20, 25);
            rb.velocity = new Vector3(Mathf.Cos(launchAngle), Mathf.Sin(launchAngle), 0) * launchMagnitude;

            GetComponentsInChildren<AudioSource>()[3].Play(); // Hurt SFX
            Handheld.Vibrate();
            StartCoroutine(Respawn());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && playing) 
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                grounded = true;
                GetComponentsInChildren<AudioSource>()[2].Play(); // Land SFX
                //Debug.Log(collision.gameObject.tag);
            }
            else if (collision.gameObject.CompareTag("BounceObject"))
            {
                umbrellaScript values = collision.GetComponent<umbrellaScript>();
                GetComponentsInChildren<AudioSource>()[5].Play(); // Bounce SFX
                if (values) Bounce(values.launchSpeed, values.maintainVelocityPercent);
            }
            else if (collision.gameObject.CompareTag("Checkpoint"))
            {
                if(checkpoint != collision.gameObject) GetComponentsInChildren<AudioSource>()[4].Play(); // Checkpoint SFX
                if (checkpoint != null) checkpoint.GetComponent<Animator>().SetBool("Lit", false);
                checkpoint = collision.gameObject;
                checkpoint.GetComponent<Animator>().SetBool("Lit", true);
            }
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSecondsRealtime(.5f);
        if (checkpoint != null) transform.position = checkpoint.transform.position;
        else transform.position = new Vector3(0, 35, 0);
        playing = true;
        rb.velocity = Vector3.zero;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !collision.gameObject.CompareTag("Player") && playing)
        {
            grounded = true;
            //Debug.Log(collision.gameObject.tag);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) grounded = false;
    }

    void JumpHandler()
    {
        if (inputManager.pressedDown) timeSinceJumpPress = 0;

        if (!grounded) timeSinceLand += Time.deltaTime;
        else timeSinceLand = 0;

        if (timeSinceJumpPress < jumpInputStorage && timeSinceLand < coyoteTime && timeSinceJump > minTimeBetweenJumps)
        {
            timeSinceJump = 0;
            Jump();
        }

        timeSinceJumpPress += Time.deltaTime;
        timeSinceJump += Time.deltaTime;

        if (inputManager.justReleased)
        {
            Debug.Log("TEST");
            if (rb.velocity.y > 0 && timeSinceJumpPress < coyoteTime) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCancelChangeCoeff);
            
            timeSinceJumpPress = coyoteTime;
        }
    }

    

    public void EndJump()
    {
        //Debug.Log("TEST");
        if (rb.velocity.y > 0 && timeSinceJumpPress < coyoteTime) rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpCancelChangeCoeff);

        timeSinceJumpPress = coyoteTime;
    }

    void Bounce(float launchHeight, float maintainVelocityPercent) 
    {
        rb.velocity = new Vector2(rb.velocity.x, launchHeight + (-rb.velocity.y * maintainVelocityPercent));
    }
}
