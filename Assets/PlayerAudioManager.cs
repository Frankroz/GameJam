using UnityEngine;

public class PlayerAudioManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("---------------- Audio Source ----------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("---------------- Audio clip ----------------")]
    public AudioClip damage;
    public AudioClip pistolGunshot;
    public AudioClip pistolReload;
    public AudioClip rifleGunshot;
    public AudioClip rifleReload;
    public AudioClip shotgunReload;
    public AudioClip shotgunGunshot;
    public AudioClip deadSound;
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
