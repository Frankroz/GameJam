using UnityEngine;

public class WardenAudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("---------------- Audio Source ----------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------------- Audio clip ----------------")]
    public AudioClip damage;
    public AudioClip start;
    public AudioClip walking;
    public AudioClip roar;

    private void Start()
    {
        SFXSource.PlayOneShot(start);
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayWalking()
    {
        musicSource.PlayOneShot(walking);
    }

    public void PlayRoar()
    {
        musicSource.PlayOneShot(roar);
    }

    public void PlayDamage()
    {
        musicSource.PlayOneShot(damage);
    }
}
