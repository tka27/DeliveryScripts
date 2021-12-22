using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetBtn : MonoBehaviour
{
    public void ResetData()
    {
        SaveSystem.ClearSaveData();
        SceneManager.LoadScene(0);
        PlayerPrefs.DeleteAll();
    }
}
