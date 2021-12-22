using UnityEngine;

public class ClearInventoryBtn : MonoBehaviour
{
    public delegate void ClickAction();
    public static event ClickAction clickEvent;
    public void EventInvoke()
    {
        SoundData.PlayBtn();
        clickEvent.Invoke();
    }
}
