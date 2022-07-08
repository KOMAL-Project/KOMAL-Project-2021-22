using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Image transition;

    public void loadGame()
    {
        SceneManager.LoadSceneAsync(1);
        transition.GetComponent<Animator>().SetTrigger("Go");
    }
}
