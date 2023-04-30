using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource sfx;

    [Header("Audio Clips")]
    public AudioClip theme;
    public AudioClip coin;
    public AudioClip victory;
    public AudioClip jump;
    public AudioClip hit;
    public AudioClip death;

    void Start()
    {
        bool isPlaying = SceneManager.GetActiveScene().name == "GameScene" && GameManager.state == GameState.InGame;
        if (isPlaying)
            PlayThemeMusic();
    }

    public void PlaySound(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }

    private void PlayThemeMusic()
    {
        music.clip = theme;
        music.Play();
    }
}
