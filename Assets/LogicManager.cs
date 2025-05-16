using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicManager : MonoBehaviour
{
    public Animator transitionAnim;
    private GameObject audioManager;
    private MenuAudio menuAudio;
    public float fadeOutDuration = 5f; // Duration of the volume fade-out

    void Awake()
    {
        // Find the AudioManager object that might have been marked DontDestroyOnLoad
        audioManager = GameObject.FindGameObjectWithTag("AudioManager");
        menuAudio = audioManager.GetComponent<MenuAudio>();
    }

    public void playGame()
    {
        StartCoroutine(FadeOutAndLoadGame());
    }

    public void seeControls()
    {
        StartCoroutine(loadControls());
    }

    public void returnToMenu()
    {
        StartCoroutine(loadMenu());
    }

    IEnumerator FadeOutAndLoadGame()
    {
        if (audioManager != null && menuAudio != null)
        {
            float startVolume = menuAudio.getVol();
            float timer = 0f;

            while (timer < fadeOutDuration)
            {
                timer += Time.deltaTime;
                menuAudio.changeVol(Mathf.Lerp(startVolume, 0f, timer / fadeOutDuration));
                yield return null;
            }

            menuAudio.changeVol(0); // Ensure volume is fully muted
            Destroy(audioManager);
        }
        else
        {
            Debug.LogWarning("AudioManager or MenuAudio not found, loading game without fade out.");
        }

        loadGame();
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

    void loadGame()
    {
        if (transitionAnim != null)
        {
            transitionAnim.SetTrigger("End");
            SceneManager.LoadSceneAsync("Game");
            transitionAnim.SetTrigger("Start");
        }
        else
        {
            SceneManager.LoadSceneAsync("Menu");
        }
    }

    IEnumerator loadControls()
    {
        if (transitionAnim != null)
        {
            transitionAnim.SetTrigger("End");
            yield return new WaitForSeconds(1);
            SceneManager.LoadSceneAsync("Controls");
            transitionAnim.SetTrigger("Start");
        }
        else
        {
            SceneManager.LoadSceneAsync("Menu");
        }
    }
}
