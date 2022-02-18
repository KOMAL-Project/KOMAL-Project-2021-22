using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] float speed, jump, coyoteTime, jumpInputStorage;
    float timeSinceLand, timeSinceJumpPress;
    bool grounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(speed * Input.GetAxis("Horizontal"), rb.velocity.y);
        //Debug.Log(rb.velocity);
        if (Input.GetButtonDown("Jump")) timeSinceJumpPress = 0;



        if (!grounded) timeSinceLand+=Time.deltaTime;
        else timeSinceLand = 0;

        if (timeSinceJumpPress < jumpInputStorage && timeSinceLand < coyoteTime) Jump();

        timeSinceJumpPress += Time.deltaTime;
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jump);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && transform.position.y > collision.transform.position.y + collision.transform.localScale.y*.45f)
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) grounded = false;
    }

}
