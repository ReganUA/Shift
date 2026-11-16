using UnityEngine;
using UnityEngine.Audio;

public class musicManager : MonoBehaviour
{
    public AudioSource music1;
    public AudioSource music2;
    private AudioLowPassFilter AudioLowPassFilter1;
    private AudioLowPassFilter AudioLowPassFilter2;
    public AudioMixer musicMixer;
    private float audioMixerLowpass = 22000f;
    public static musicManager instance;
    [Header("Sounds")]
    public AudioSource levelCompleted;
    public AudioSource playerDeath;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Закріплюємо ПЕРШИЙ створений об'єкт
        }
        else
        {
            Destroy(gameObject); // Знищуємо ДУБЛІКАТ, який намагається з'явитися на новій сцені
            return; // Виходимо з методу, щоб не виконувати код далі
        }
        AudioLowPassFilter1 = music1.GetComponent<AudioLowPassFilter>();
        AudioLowPassFilter2 = music2.GetComponent<AudioLowPassFilter>();
    }
    private void Update()
    {
        if(AudioLowPassFilter1.cutoffFrequency < 22000)
        {
            AudioLowPassFilter1.cutoffFrequency += 15000f * Time.unscaledDeltaTime;
            AudioLowPassFilter2.cutoffFrequency += 15000f * Time.unscaledDeltaTime;
        }
    }
    public void StartPlaying()
    {
        if(music1.isPlaying || music2.isPlaying)
        {
            return;
        }
        music1.Play();
        music2.Play();
    }
    public void ChangeDimenshionMusic()
    {
        if (gameManager.instance.dimension1.activeSelf)
        {
            music1.volume = 1;
            music2.volume = 0;
        }
        else
        {
            music1.volume = 0;
            music2.volume = 0.82f;
        }
        AudioLowPassFilter1.cutoffFrequency = 4000f;
        AudioLowPassFilter2.cutoffFrequency = 4000f;
    }
    //public void PlayMusic(AudioClip clip)
    //{
    //    currentAudioSource.clip = clip;
    //    currentAudioSource.Play();
    //}
}
