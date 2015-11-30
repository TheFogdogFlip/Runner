using UnityEngine;
using System.Collections;

public static class GlobalGameSettings
{

    /*------------------------------------Other settings------------------------------------*/
    //Can be a value between 0 and 100
    public static int Sound_Effects_Volume;

    //Can be a value between 0 and 100
    public static int Sound_Music_Volume;
    
    //Can be a value between 80 and 110
    public static int Field_Of_View;
    
    //Bool which checks if player will be using Game pad. Not needed??
    public static bool Using_Game_Pad;


    /*------------------------------------Keyboard------------------------------------*/
    //Keyboard key bind for moving right
    public static string kb_Move_Right_Key;

    //Keyboard key bind for moving left
    public static string kb_Move_Left_Key;

    //Keyboard key bind for jumping
    public static string kb_Jump_Key;

    //Keyboard key bind for sliding
    public static string kb_Slide_Key;


    /*------------------------------------Game Pad------------------------------------*/
    //Game pad key bind for moving right
    public static string gp_Move_Right_Key;

    //Game pad key bind for moving left
    public static string gp_Move_Left_Key;

    //Game pad key bind for jumping
    public static string gp_Jump_Key;

    //Game pad key bind for sliding
    public static string gp_Slide_Key;


    public static void LoadSettings()
    {
        string filename = "config.xml";
        //load settings from file config.xml here
    }
}
