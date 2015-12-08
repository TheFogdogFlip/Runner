using UnityEngine;
using System.Collections;

public static class GlobalGameSettings
{

    /*------------------------------------Other settings------------------------------------*/
    //Can be a value between 0 and 100
    private static int Sound_Effects_Volume;

    //Can be a value between 0 and 100
    private static int Sound_Music_Volume;

    //Can be a value between 0 and 100
    private static int Sound_Master_Volume;
    
    /*
    //Can be a value between 80 and 110
    private static int Field_Of_View;
    
    //Bool which checks if player will be using Game pad. Not needed??
    private static bool Using_Game_Pad;
    */

    /*------------------------------------Keyboard------------------------------------*/
    /*
    //Keyboard key bind for moving right
    private static string kb_Move_Right_Key;

    //Keyboard key bind for moving left
    private static string kb_Move_Left_Key;

    //Keyboard key bind for jumping
    private static string kb_Jump_Key;

    //Keyboard key bind for sliding
    private static string kb_Slide_Key;
    */

    /*------------------------------------Game Pad------------------------------------*/
    /*
    //Game pad key bind for moving right
    private static string gp_Move_Right_Key;

    //Game pad key bind for moving left
    private static string gp_Move_Left_Key;

    //Game pad key bind for jumping
    private static string gp_Jump_Key;

    //Game pad key bind for sliding
    private static string gp_Slide_Key;
    */

    public static void LoadSettings()
    {
        Sound_Master_Volume = PlayerPrefs.GetInt("MasterVol");
        Sound_Music_Volume = PlayerPrefs.GetInt("MusicVol");
        Sound_Effects_Volume = PlayerPrefs.GetInt("EffectsVol");
    }

    public static void SaveSettings()
    {
        PlayerPrefs.SetInt("MasterVol", Sound_Master_Volume);
        PlayerPrefs.SetInt("EffectsVol", Sound_Effects_Volume);
        PlayerPrefs.SetInt("MusicVol", Sound_Music_Volume);
    }

    //Set and Get Master Volume
    public static void SetMasterVolume(int val)     { Sound_Master_Volume = val; }
    public static int GetMasterVolume()             { return Sound_Master_Volume; }

    //Set and Get Music Volume
    public static void SetMusicVolume(int val)      { Sound_Music_Volume = val; }
    public static int GetMusicVolume()              { return Sound_Music_Volume; }

    //Set and Get Effects Volume
    public static void SetEffectsVolume(int val)    { Sound_Effects_Volume = val; }
    public static int GetEffectsVolume()            { return Sound_Effects_Volume; }

    /*
    //Set and Get Field of View
    public static void SetFieldOfView(int val)      { Field_Of_View = val; }
    public static int GetFieldOfView()              { return Field_Of_View; }

    //Set and Get Kb Move Right
    public static void SetKbMoveRight(string key)   { kb_Move_Right_Key = key; }
    public static string GetKbMoveRight()           { return kb_Move_Left_Key; }

    //Set and Get Kb Move Left
    public static void SetKbMoveLeft(string key)    { kb_Move_Left_Key = key; }
    public static string GetKbMoveLeft()            { return kb_Move_Left_Key; }

    //Set and Get Kb Jump
    public static void SetKbJump(string key)        { kb_Jump_Key = key; }
    public static string GetKbJump()                { return kb_Jump_Key; }

    //Set and Get Kb Slide
    public static void SetKbSlide(string key)       { kb_Slide_Key = key; }
    public static string GetKbSlide()               { return kb_Slide_Key; }

    //Set and Get Gp Move Right
    public static void SetGpMoveRight(string key)   { gp_Move_Right_Key = key; }
    public static string GetGpMoveRight()           { return gp_Move_Right_Key; }

    //Set and Get Gp Move Left
    public static void SetGpMoveLeft(string key)    { gp_Move_Left_Key = key; }
    public static string GetGpMoveLeft()            { return gp_Move_Left_Key; }

    //Set and Get Gp Jump
    public static void SetGpJump(string key)        { gp_Jump_Key = key; }
    public static string GetGpJump()                { return gp_Jump_Key; }

    //Set and Get Gp Slide
    public static void SetGpSlide(string key)       { gp_Slide_Key = key; }
    public static string GetGpSlide()               { return gp_Slide_Key; }
    */
}
