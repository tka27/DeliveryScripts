using UnityEngine;

public class SellBtn : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction clickEvent;
    public void EventInvoke()
    {
        SoundData.PlayBtn();
        clickEvent.Invoke();
    }
}
