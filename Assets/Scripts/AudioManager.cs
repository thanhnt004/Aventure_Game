using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance{get;private set;} 
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    public AudioClip background;
    public AudioClip atk;
    public AudioClip run;
    public AudioClip jump;
    public AudioClip dead;
    public AudioClip checkPoint;
    public AudioClip hurt;
    public AudioClip arrow;

private void Start()
{
    instance = this;
    musicSource.clip = background;
    musicSource.Play();
}
public void PlaySFX(AudioClip clip)
{
    SFXSource.PlayOneShot(clip);
}

}
