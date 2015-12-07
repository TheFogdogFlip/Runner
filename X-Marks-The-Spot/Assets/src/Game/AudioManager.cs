using UnityEngine;
using System.Collections;

public class AudioManager : Object{


    AudioSource jumpSound;
    AudioSource slideSound;
    public void InitAudio()
    {
        GameObject audioHolder = Instantiate(Resources.Load("AudioHolder", typeof(GameObject))) as GameObject;
        AudioSource[] audioSources = audioHolder.GetComponents<AudioSource>();
        jumpSound = audioSources[1];
        slideSound = audioSources[2];
    }

    public void JumpSound()
    {
        //var aSources = GameObject.Find("Player(Clone)").GetComponents<AudioSource>();
        //jumpSound = new AudioSource();
        //jumpSound = aSources[0];
        if (!jumpSound.isPlaying);
        {
            jumpSound.Play();
        }
    }
    
    public void SlideSound()
    {
        //var aSources = GameObject.Find("Player(Clone)").GetComponents<AudioSource>();
        //slideSound = new AudioSource();
        //slideSound = aSources[1];
        if (!slideSound.isPlaying);
        {
            slideSound.Play();
        }
    }
}
