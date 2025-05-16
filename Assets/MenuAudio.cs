using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAudio : MonoBehaviour
{
    [Header("---------------- Audio Source ----------------")]
    [SerializeField] AudioSource SFXSource;

    [Header("---------------- Audio clip ----------------")]
    public AudioClip backgroud;

    public static MenuAudio instance;
    private float fadeDuration = 2f; // Time it takes to fade
    private float targetVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
        
        if (SceneManager.GetActiveScene().name == "Game")
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SFXSource.clip = backgroud;
        SFXSource.Play();
    }

    public void changeVol(float vol)
    {
        targetVolume = vol;
        StartCoroutine(FadeVolume());
    }

    public float getVol() {
        return SFXSource.volume;
    }

    IEnumerator FadeVolume()
    {
        float startVolume = SFXSource.volume;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            SFXSource.volume = Mathf.Lerp(startVolume, targetVolume, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        SFXSource.volume = targetVolume; // Ensure the target volume is set at the end
    }
}
