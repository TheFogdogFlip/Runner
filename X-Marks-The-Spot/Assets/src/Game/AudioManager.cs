using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class AudioManager : Object{
    /**---------------------------------------------------------------------------------
     * Audiomixer, audio manager and snapshots that are used in the script.
     */
    private static AudioMixer MasterMixer;
    private static AudioManager audioManager;
    private static AudioMixerSnapshot paused;
    private static AudioMixerSnapshot unPaused;
    /**---------------------------------------------------------------------------------
     * Audio Sources used in the script.
     */
    private static AudioSource loopSound;
    private static AudioSource jumpSound;
    private static AudioSource slideSound;
    private static AudioSource collisionSound;
    private static AudioSource fallingSound;
    private static AudioSource winSound;
    /**---------------------------------------------------------------------------------
     * Variables used in the script.
     */
    private static bool audioInitiated = false;
    private static float volumeMultiplyer;

    /**---------------------------------------------------------------------------------
     * 
     */
    public static 
    AudioManager Instance 
    {
        get 
        {
            if (!audioInitiated)
            {
                audioManager = new AudioManager();
                audioManager.initAudio();
                audioInitiated = true;
            }

            return audioManager;
        }
    }
    /**---------------------------------------------------------------------------------
     * Constructor
     */
    private 
    AudioManager()
    {
    }
    /**---------------------------------------------------------------------------------
     * Initiates audioholder, snapshots, sounds and mixers.
     * Is used when first audio is called.
     */
    private void 
    initAudio()
    {
        GameObject audioHolder = Instantiate(Resources.Load("AudioHolder", typeof(GameObject))) as GameObject;
        DontDestroyOnLoad(audioHolder);
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
{

    /**---------------------------------------------------------------------------------
     * Playes the background music.
     */
    public void startLoop()
    {
        loopSound.Play();
    }
    /**---------------------------------------------------------------------------------
     * Stops the background music.
     */
    public void stopLoop()
    {
        loopSound.Stop();
    }
    /**---------------------------------------------------------------------------------
     * Is called by Player "Jump" function.
     * Playes the slide sound.
     */
    public void 
    JumpSound()
    {
        if (!jumpSound.isPlaying)
        {
            jumpSound.Play();
        }
    }
    /**---------------------------------------------------------------------------------
     * Is called by Player "Slide" function.
     * Playes the slide sound.
     */
    public void 
    SlideSound()
    {
        if (!slideSound.isPlaying)
        {
            slideSound.Play();
        }
    }
    /**---------------------------------------------------------------------------------
     * Is called by Player "Death" function.
     * Playes the collision/death sound.
     */
    public void 
    CollisionSound()
    {
        if (!collisionSound.isPlaying)
        {
            collisionSound.Play();
        }
    }
    /**---------------------------------------------------------------------------------
     * Is called by Player "Falling" function.
     * Playes the fall sound.
     */
    public void 
    FallingSound()
    {
        if (!fallingSound.isPlaying)
        {
            fallingSound.Play();
        }
    }
    /**---------------------------------------------------------------------------------
     * Is called by Player "GoalFunc" function.
     * Playes the win sound.
     */
    public void 
    WinSound()
    {
        if (!winSound.isPlaying)
        {
            winSound.Play();
        }
    }
    /**---------------------------------------------------------------------------------
     * Is called by PauseMenu "keyPress (Esc)" function.
     * sets audiomixersnapshot to "Paused".
     */
    public void 
    pauseVolume()
    {
        paused.TransitionTo(0.01f);
    }
    /**---------------------------------------------------------------------------------
     * Is called by PauseMenu "ResumeGamePress" function.
     * sets audiomixersnapshot to "Unpaused".
     */
    public void 
    UnPauseVolume()
    {
        unPaused.TransitionTo(0.01f);
    }
    /**---------------------------------------------------------------------------------
     * Is called by menu "SetMasterVolume" function.
     * sets master channel volume.
     */
    public void 
    SetMasterVolume(int MasterVolume)
    {
        MasterMixer.SetFloat("Master", MasterVolume * 0.8f - 80);
        

    }
    /**---------------------------------------------------------------------------------
     * Is called by menu "SetMusicVolume" function.
     * sets music channel volume.
     */
    public void 
    SetMusicVolume(int MusicVolume)
    {
        MasterMixer.SetFloat("Music", MusicVolume * 0.8f - 80);
    }
    /**---------------------------------------------------------------------------------
     * Is called by menu "SetSoundEffectsVolume" function.
     * sets soundeffects channel volume.
     */
    public void 
    SetSoundFXVolume(int FXVolume)
    {
        MasterMixer.SetFloat("SoundFx", FXVolume * 0.8f - 80);
    }
}
