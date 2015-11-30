using UnityEngine;
using System.Collections;

public class AudioManager : Object{
    GameObject SoundObject;
    public void JumpSound()
    {
        AudioSource JumpSound = new AudioSource();
        //JumpSound = Instantiate(Resources.Load<AudioSource>("Resources/Audio/JumpSound"));
        //SoundObject = GameObject.Find("Player(Clone)");
        JumpSound = GameObject.Find("Player(Clone)").GetComponent<AudioSource>();
        if (!JumpSound.isPlaying);
        {
            JumpSound.Play();
        }
    }
}
