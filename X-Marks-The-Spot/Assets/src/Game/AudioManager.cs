using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour{

    public AudioMixer MasterMixer;

    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unPaused;

    private AudioSource jumpSound;
    private AudioSource slideSound;
    private AudioSource collisionSound;
    private AudioSource fallingSound;
    private AudioSource winSound;
    public void InitAudio()
    {
        GameObject audioHolder = Instantiate(Resources.Load("AudioHolder", typeof(GameObject))) as GameObject;
        AudioSource[] audioSources = audioHolder.GetComponents<AudioSource>();
        jumpSound = audioSources[1];
        slideSound = audioSources[2];
        collisionSound = audioSources[3];
        fallingSound = audioSources[4];
        winSound = audioSources[5];
    }

    public void JumpSound()
    {
        if (!jumpSound.isPlaying)
        {
            jumpSound.Play();
        }
    }
    
    public void SlideSound()
    {
        if (!slideSound.isPlaying)
        {
            slideSound.Play();
        }
    }

    public void CollisionSound()
    {
        if (!collisionSound.isPlaying)
        {
            collisionSound.Play();
        }
    }

    public void FallingSound()
    {
        if (!fallingSound.isPlaying)
        {
            fallingSound.Play();
        }
    }

    public void WinSound()
    {
        if (!winSound.isPlaying)
        {
            winSound.Play();
        }
    }

    public void pauseVolume()
    {
        paused.TransitionTo(0.01f);
    }

    public void UnPauseVolume()
    {
        unPaused.TransitionTo(0.01f);
    }

    public void SetMasterVolume(float masterVolume)
    {
        MasterMixer.SetFloat("masterVolume", masterVolume);
    }

    public void SetMusicVolume(float musicVolume)
    {
        MasterMixer.SetFloat("musicVolume", musicVolume);
    }

    public void SetSoundFXVolume(float soundFXVolume)
    {
        MasterMixer.SetFloat("soundFXVolume", soundFXVolume);
    }
}
