using UnityEngine;
using System.Collections;

public class AudioManager : Object{


    AudioSource jumpSound;
    AudioSource slideSound;
    AudioSource collisionSound;
    AudioSource fallingSound;
    public void InitAudio()
    {
        GameObject audioHolder = Instantiate(Resources.Load("AudioHolder", typeof(GameObject))) as GameObject;
        AudioSource[] audioSources = audioHolder.GetComponents<AudioSource>();
        jumpSound = audioSources[1];
        slideSound = audioSources[2];
        collisionSound = audioSources[3];
        fallingSound = audioSources[4];
    }

    public void JumpSound()
    {
        if (!jumpSound.isPlaying);
        {
            jumpSound.Play();
        }
    }
    
    public void SlideSound()
    {
        if (!slideSound.isPlaying);
        {
            slideSound.Play();
        }
    }
    public void CollisionSound()
    {
        if (!collisionSound.isPlaying);
        {
            collisionSound.Play();
        }
    }
    public void FallingSound()
    {
        if (!fallingSound.isPlaying);
        {
            fallingSound.Play();
        }
    }
}
