using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : Object{

    private static AudioMixer MasterMixer;

    private static AudioMixerSnapshot paused;
    private static AudioMixerSnapshot unPaused;

    private static AudioSource loopSound;
    private static AudioSource jumpSound;
    private static AudioSource slideSound;
    private static AudioSource collisionSound;
    private static AudioSource fallingSound;
    private static AudioSource winSound;

    private static AudioManager audioManager;
    /**---------------------------------------------------------------------------------
    * 
     */
    private static int MasterVolume;
    private static int MusicVolume;
    private static int FXVolume;

    /**---------------------------------------------------------------------------------
    * 
    */
    public static AudioManager Instance 
    {
        get 
        {
            if (audioManager == null)
                audioManager = new AudioManager();

            return audioManager;
        }
    }
    /**---------------------------------------------------------------------------------
     * 
    */
    private AudioManager()
    { 
        
    }
    /**---------------------------------------------------------------------------------
     * 
    */
    public void InitAudio()
    {
        GameObject audioHolder = Instantiate(Resources.Load("AudioHolder", typeof(GameObject))) as GameObject;
        AudioSource[] audioSources = audioHolder.GetComponents<AudioSource>();
        loopSound = audioSources[0];
        jumpSound = audioSources[1];
        slideSound = audioSources[2];
        collisionSound = audioSources[3];
        fallingSound = audioSources[4];
        winSound = audioSources[5];
        MasterMixer = Resources.Load<AudioMixer>("Audio/MasterMixer");
        paused = MasterMixer.FindSnapshot("Paused");
        unPaused = MasterMixer.FindSnapshot("Unpaused");
    }
     /**---------------------------------------------------------------------------------
     * 
     */
    public void startLoop()
    {
        loopSound.Play();
    }
    /**---------------------------------------------------------------------------------
    * 
     */
    public void stopLoop()
    {
        loopSound.Stop();
    }
    /**---------------------------------------------------------------------------------
    * 
    */
    public void JumpSound()
    {
        if (!jumpSound.isPlaying)
        {
            jumpSound.Play();
        }
    }
    /**---------------------------------------------------------------------------------
    * 
    */
    public void SlideSound()
    {
        if (!slideSound.isPlaying)
        {
            slideSound.Play();
        }
    }
    /**---------------------------------------------------------------------------------
    * 
    */
    public void CollisionSound()
    {
        if (!collisionSound.isPlaying)
        {
            collisionSound.Play();
        }
    }
    /**---------------------------------------------------------------------------------
    * 
    */
    public void FallingSound()
    {
        if (!fallingSound.isPlaying)
        {
            fallingSound.Play();
        }
    }
    /**---------------------------------------------------------------------------------
    * 
    */
    public void WinSound()
    {
        if (!winSound.isPlaying)
        {
            winSound.Play();
        }
    }
    /**---------------------------------------------------------------------------------
     * 
    */
    public void pauseVolume()
    {
        paused.TransitionTo(0.01f);
    }
    /**---------------------------------------------------------------------------------
    * 
    */
    public void UnPauseVolume()
    {
        unPaused.TransitionTo(0.01f);
    }
    /**---------------------------------------------------------------------------------
    * 
    */
    public void SetMasterVolume()
    {
        MasterVolume = GlobalGameSettings.GetEffectsVolume();
        MasterMixer.SetFloat("masterVolume", MasterVolume);
    }
    /**---------------------------------------------------------------------------------
    * 
    */
    public void SetMusicVolume()
    {
        MusicVolume = GlobalGameSettings.GetEffectsVolume();
        MasterMixer.SetFloat("musicVolume", MusicVolume);
    }
    /**---------------------------------------------------------------------------------
    * 
    */
    public void SetSoundFXVolume()
    {
        FXVolume = GlobalGameSettings.GetEffectsVolume();
        MasterMixer.SetFloat("soundFXVolume", FXVolume);
    }
}
