using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;
    public bool isGameFinished = false;

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!musicAudioSource.isPlaying &&  musicAudioSource.clip.name == "powerup")
            PlayMusic("game");
    }

    public void PlaySFX(string name)
    {
        AudioClip clip = Resources.Load<AudioClip>("SFX/" + name);

        if (clip == null || isGameFinished)
            return;

        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    public void PlayMusic(string name, bool loop = true)
    {
        AudioClip clip = Resources.Load<AudioClip>("Music/" + name);

        if(name == "powerup")
            loop = false;

        if (musicAudioSource.clip == clip || clip == null || isGameFinished) 
            return;

        musicAudioSource.loop = loop;
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
}
