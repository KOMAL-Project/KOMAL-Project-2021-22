using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private Button restartBtn, jumpBtn;
    [SerializeField] Image joystick;
    [SerializeField] private GameObject endUI, score, demoUI;

    public void LevelEnd()
    {
        joystick.gameObject.SetActive(false);
        jumpBtn.gameObject.SetActive(false);

        endUI.GetComponent<Animator>().SetTrigger("Go");

    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartLevel()
    {
        Destroy(demoUI);
    }

    public void SetScore(int num)
    {
        score.GetComponentInChildren<Animator>().Play("Jump");
        Debug.Log(score.GetComponentInChildren<Animator>());
        foreach(Text t in score.GetComponentsInChildren<Text>())
        {
            //Debug.Log(t.text);
            t.text = num.ToString();
        }
    }
}
