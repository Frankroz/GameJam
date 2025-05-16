using UnityEngine;

public class ZombieAudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("---------------- Audio Source ----------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------------- Audio clip ----------------")]
    public AudioClip damage;
    public AudioClip walking;

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayWalking()
    {
        musicSource.PlayOneShot(walking);
    }

    public void PlayDamage()
    {
        musicSource.PlayOneShot(damage);
    }
}
