using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [HideInInspector] public bool vibration;
    [HideInInspector] public bool sound;
    [HideInInspector] public bool music;
    public int tutorialLvl;

    public void LoadPrefs()
    {
        if (!PlayerPrefs.HasKey("vibration"))
        {
            vibration = true;
        }
        else
        {
            vibration = PlayerPrefs.GetInt("vibration") == 1;
        }

        if (!PlayerPrefs.HasKey("sound"))
        {
            sound = true;
        }
        else
        {
            sound = PlayerPrefs.GetInt("sound") == 1;
        }

        if (!PlayerPrefs.HasKey("music"))
        {
            music = true;
        }
        else
        {
            music = PlayerPrefs.GetInt("music") == 1;
        }

        tutorialLvl = PlayerPrefs.GetInt("tutorialLvl");
    }

    public void SavePrefs()
    {
        if (vibration)
        {
            PlayerPrefs.SetInt("vibration", 1);
        }
        else
        {
            PlayerPrefs.SetInt("vibration", 0);
        }

        if (sound)
        {
            PlayerPrefs.SetInt("sound", 1);
        }
        else
        {
            PlayerPrefs.SetInt("sound", 0);
        }

        if (music)
        {
            PlayerPrefs.SetInt("music", 1);
        }
        else
        {
            PlayerPrefs.SetInt("music", 0);
        }

        PlayerPrefs.SetInt("tutorialLvl", tutorialLvl);

        PlayerPrefs.Save();
    }
}
