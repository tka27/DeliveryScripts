using UnityEngine;

public class ExitBtn : MonoBehaviour
{
    public void Exit()
    {
        SaveSystem.Save();
        Application.Quit();
    }
}
