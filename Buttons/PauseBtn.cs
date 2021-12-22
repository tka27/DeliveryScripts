using UnityEngine;

public class PauseBtn : MonoBehaviour
{
    [SerializeField] SoundData soundData;
    [SerializeField] GameSettings settings;

    public void PauseClick()
    {
        soundData.SwitchLoopSounds(false);
        Time.timeScale = 0;
    }
    public void ResumeClick()
    {
        soundData.SwitchLoopSounds(settings.sound);
        Time.timeScale = 1;
    }
}
