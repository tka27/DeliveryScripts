using UnityEngine;

public class CarReturnBtns : MonoBehaviour
{

    public delegate void ReturnHandler();
    public static event ReturnHandler returnEvent;

    public void ReturnConfirm()
    {
        SoundData.PlayBtn();
        if (BuildingsData.lastVisit) returnEvent.Invoke();
    }
}
