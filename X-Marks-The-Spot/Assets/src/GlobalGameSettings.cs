using UnityEngine;
using System.Collections;

public static class GlobalGameSettings
{

    /**---------------------------------------------------------------------------------
     * Sound settings.
     */
    //Can be a value between 0 and 100
    private static int Sound_Effects_Volume;

    //Can be a value between 0 and 100
    private static int Sound_Music_Volume;

    //Can be a value between 0 and 100
    private static int Sound_Master_Volume;

    /**---------------------------------------------------------------------------------
     * Executed when settings needs to be loaded from PlayerPrefs.
     * Loads stored values in PlayerPrefs.
     */
    public static void 
    LoadSettings()
    {
        Sound_Master_Volume = PlayerPrefs.GetInt("MasterVol");
        Sound_Music_Volume = PlayerPrefs.GetInt("MusicVol");
        Sound_Effects_Volume = PlayerPrefs.GetInt("EffectsVol");
    }

    /**---------------------------------------------------------------------------------
     * Executed when settings need to be saved to PlayerPrefs.
     * Saves stored values in PlayerPrefs.
     */
    public static void 
    SaveSettings()
    {
        PlayerPrefs.SetInt("MasterVol", Sound_Master_Volume);
        PlayerPrefs.SetInt("EffectsVol", Sound_Effects_Volume);
        PlayerPrefs.SetInt("MusicVol", Sound_Music_Volume);
    }

    /**---------------------------------------------------------------------------------
     * Set master volume.
     */
    public static void 
    SetMasterVolume(int val)        { Sound_Master_Volume = val; }
    
    /**---------------------------------------------------------------------------------
     * Get master volume.
     */
    public static int 
    GetMasterVolume()               { return Sound_Master_Volume; }

    /**---------------------------------------------------------------------------------
     * Set music volume.
     */
    public static void 
    SetMusicVolume(int val)         { Sound_Music_Volume = val; }

    /**---------------------------------------------------------------------------------
     * Get music volume.
     */
    public static int
    GetMusicVolume()                { return Sound_Music_Volume; }

    /**---------------------------------------------------------------------------------
     * Set effects volume.
     */
    public static void 
    SetEffectsVolume(int val)       { Sound_Effects_Volume = val; }

    /**---------------------------------------------------------------------------------
     * Get effects volume.
     */
    public static int 
    GetEffectsVolume()              { return Sound_Effects_Volume; }
}
