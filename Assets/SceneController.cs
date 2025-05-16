using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public Animator transitionAnim;

    public IEnumerator loadMenu()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync("Menu");
        transitionAnim.SetTrigger("Start");
    }

    public IEnumerator loadGame()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync("Game");
        transitionAnim.SetTrigger("Start");
    }

    public IEnumerator loadControls()
    {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync("Controls");
        transitionAnim.SetTrigger("Start");
    }
}
