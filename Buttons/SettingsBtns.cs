using UnityEngine;
using UnityEngine.UI;

public class SettingsBtns : MonoBehaviour
{
    [SerializeField] GameSettings settings;
    [SerializeField] Sprite cross;
    [SerializeField] Sprite check;
    [SerializeField] Image vibrationImage;
    [SerializeField] Image soundImage;
    [SerializeField] Image musicImage;
    [SerializeField] SoundData soundData;




    public void VibrationSwitch()
    {
        SoundData.PlayBtn();
        settings.vibration = !settings.vibration;
        DisplayUpdate();
    }
    public void SoundSwitch()
    {
        settings.sound = !settings.sound;
        SoundData.PlayBtn();
        DisplayUpdate();
    }
    public void MusicSwitch()
    {
        SoundData.PlayBtn();
        settings.music = !settings.music;
        soundData.SwitchMusic(settings.music);
        DisplayUpdate();
    }



    public void DisplayUpdate()
    {
        if (settings.vibration)
        {
            vibrationImage.sprite = check;
        }
        else
        {
            vibrationImage.sprite = cross;
        }

        if (settings.sound)
        {
            soundImage.sprite = check;
        }
        else
        {
            soundImage.sprite = cross;
        }

        if (settings.music)
        {
            musicImage.sprite = check;
        }
        else
        {
            musicImage.sprite = cross;
        }
    }
}
