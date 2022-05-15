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
            if (this.gameObject.CompareTag("Objective"))
            {
                levelEndController.GetComponent<CanvasController>().LevelEnd();
            }
            else collision.gameObject.GetComponent<PlayerController>().collectCoin();
            
            Destroy(gameObject);
        }
    }
}
