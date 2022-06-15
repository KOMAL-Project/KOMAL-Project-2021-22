using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    

    private bool collected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Animator>().SetTrigger("Collect");

            if (this.gameObject.CompareTag("Objective"))
            {
                collision.gameObject.GetComponent<PlayerController>().cv.LevelEnd();
                Destroy(gameObject);
            }
            else if(!collected) collision.gameObject.GetComponent<PlayerController>().collectCoin();

            GetComponent<BoxCollider2D>().enabled = false;
            
            
        }
        collected = true;
    }
}
