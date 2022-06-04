using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [SerializeField] GameObject levelEndController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {

            

            GetComponent<Animator>().SetTrigger("Collect");

            if (this.gameObject.CompareTag("Objective"))
            {
                levelEndController.GetComponent<CanvasController>().LevelEnd();
                Destroy(gameObject);
            }
            else collision.gameObject.GetComponent<PlayerController>().collectCoin();

            GetComponent<BoxCollider2D>().enabled = false;
            
            
        }
    }
}
