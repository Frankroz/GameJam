using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicScript : MonoBehaviour
{
    public GameObject gameOverScreen;
    public Animator transitionAnim;
    public GameObject pauseScreen;

    public bool isGamePaused;
    
    public void restartGame()
    {
        gameOverScreen.SetActive(false);
        StartCoroutine(loadGame());
    }

    public void returnToMenu()
    {
        StartCoroutine(loadMenu());
    }

    public void pauseGame()
    {
        isGamePaused = true;
        pauseScreen.SetActive(true);
    }

    public void resumeGame()
    {
        isGamePaused = false;
        pauseScreen.SetActive(false);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    IEnumerator loadMenu()
    {
        if (transitionAnim != null)
        {
            transitionAnim.SetTrigger("End");
            yield return new WaitForSeconds(1);
            SceneManager.LoadSceneAsync("Menu");
            transitionAnim.SetTrigger("Start");
        }
        else
        {
            SceneManager.LoadSceneAsync("Menu");
        }
    }

    public IEnumerator loadGame()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync("Game");
        transitionAnim.SetTrigger("Start");
    }

    public void gameOver()
    {
        gameOverScreen.SetActive(true);
    }
}
