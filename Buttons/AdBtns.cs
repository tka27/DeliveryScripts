using UnityEngine;

public class AdBtns : MonoBehaviour
{
    [SerializeField] GameObject carReturnCanvas;
    [SerializeField] GameObject prevCanvas;

    public delegate void ReturnHandler();
    public static event ReturnHandler returnEvent;

    public void ActivateCarReturnCanvas()
    {
        SoundData.PlayBtn();
        carReturnCanvas.SetActive(true);
        prevCanvas.SetActive(false);
    }

    public void ReturnConfirm()
    {
        SoundData.PlayBtn();
        if (BuildingsData.lastVisit) returnEvent.Invoke();
        
        carReturnCanvas.SetActive(false);
        prevCanvas.SetActive(true);
    }
}
