using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour
{

    [SerializeField] int initialDirection; // which way should the crab be going upon Awake? (1 = right, 0 = left)
    [SerializeField] float speed = 3;
    [SerializeField] int distanceBound; // How far the crab can go from the spawn point
    private Rigidbody2D rb;
    private Vector3 spawn;

    // Start is called before the first frame update
    void Start()
    {
        initialDirection = Random.Range(0, 2);
        spawn = transform.position;

        rb = GetComponent<Rigidbody2D>();

        if (initialDirection == 0) rb.velocity = new Vector2(speed * -1, 0);
        else rb.velocity = new Vector2(speed, 0);
    }

    private void Update()
    {
        if (Mathf.Abs(spawn.x - transform.position.x) > distanceBound && Random.Range(0, 50) < 1)
        {
            if (transform.position.x < spawn.x) rb.velocity = new Vector2(speed, rb.velocity.y);
            if (transform.position.x > spawn.x) rb.velocity = new Vector2(speed*-1, rb.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        if (!collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = rb.velocity.x < 0 ? new Vector2(speed, rb.velocity.y) : new Vector2(speed * -1, rb.velocity.y); // If the crab runs into a wall, change directions.
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.position.y < transform.position.y - .025f && collision.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("Exit wall");
            //rb.velocity = rb.velocity.x < 0 ? new Vector2(speed, rb.velocity.y) : new Vector2(speed * -1, rb.velocity.y); // If the crab is close to a ledge, change directions.
        }
    }

}
