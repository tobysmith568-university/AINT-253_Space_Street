using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;

public static class MyPrefs
{
    public enum Prefs
    {
        XSensitivity,
        YSensitivity,
        CrosshairRed,
        KeyMappings,
        XAxisInverted,
        YAxisInverted
    }

    public static float XSensitivity
    {
        get
        {
            return PlayerPrefs.GetFloat("XSensitivity");
        }
        set
        {
            PlayerPrefs.SetFloat("XSensitivity", value);
            PlayerPrefs.Save();
        }
    }
    public static float YSensitivity
    {
        get
        {
            return PlayerPrefs.GetFloat("YSensitivity");
        }
        set
        {
            PlayerPrefs.SetFloat("YSensitivity", value);
            PlayerPrefs.Save();
        }
    }
    public static Mapping[] KeyMappings
    {
        get
        {
            return JsonConvert.DeserializeObject<Mapping[]>(PlayerPrefs.GetString("KeyMappings"));
        }
        set
        {
            PlayerPrefs.SetString("KeyMappings", JsonConvert.SerializeObject(value));
            PlayerPrefs.Save();
        }
    }
    public static bool XAxisInverted
    {
        get
        {
            return PlayerPrefs.GetInt("XAxisInverted") == 1;
        }
        set
        {
            PlayerPrefs.SetInt("XAxisInverted", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
    public static bool YAxisInverted
    {
        get
        {
            return PlayerPrefs.GetInt("YAxisInverted") == 1;
        }
        set
        {
            PlayerPrefs.SetInt("YAxisInverted", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    static MyPrefs()
    {
        //If any prefs don't exsist, give them their default values
        if (!Exists(Prefs.XSensitivity))
            XSensitivity = 2f;
        if (!Exists(Prefs.YSensitivity))
            YSensitivity = 2f;
        //Prefs.KeyMappings is set in MyInput.cs so isn't set here
        if (!Exists(Prefs.XAxisInverted))
            XAxisInverted = false;
        if (!Exists(Prefs.YAxisInverted))
            YAxisInverted = false;
    }

    /// <summary>
    /// Deletes a PlayerPref
    /// Note: It will still be in the pref Enum
    /// </summary>
    /// <param name="pref">The name of the PlayerPref</param>
    public static void Delete(Prefs pref)
    {
        PlayerPrefs.DeleteKey(pref.ToString());
    }

    /// <summary>
    /// Tests to see if a PlayerPref has a value
    /// </summary>
    /// <param name="pref">The name of the PlayerPref</param>
    /// <returns>True if it does have a value</returns>
    public static bool Exists(Prefs pref)
    {
        return PlayerPrefs.HasKey(pref.ToString());
    }
}
