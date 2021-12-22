using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    [SerializeField] UIData uiData;
    [SerializeField] SceneData sceneData;

    public void ConfirmClick()
    {
        SoundData.PlayBtn();
        if (uiData.isPathComplete)
        {
            uiData.isPathConfirmed = true;
            sceneData.gameMode = GameMode.Drive;
            uiData.gameModeText.text = sceneData.gameMode.ToString();
            UIData.UpdateUI();
        }
        else
        {
            sceneData.Notification("Build a road to the building");
        }
    }
}
