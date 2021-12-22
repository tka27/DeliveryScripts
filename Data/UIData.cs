using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public class UIData : MonoBehaviour
{
    public bool isPathComplete;
    public bool isPathConfirmed;
    public GameObject buyButton;
    public GameObject sellButton;
    public GameObject tradeBtns;
    public GameObject buildBtns;
    public GameObject carReturnBtn;
    public Text gameModeText;
    public Text fuelText;
    public Text durabilityText;
    public Text moneyText;
    public Text cargoText;
    public GameObject inventoryCanvas;
    public List<Text> inventoryText;
    public List<Image> inventoryIcons;

    public delegate void UpdateUIHandler();
    public static event UpdateUIHandler updateUIEvent;
    public static void UpdateUI()
    {
        updateUIEvent.Invoke();
    }






    /*public static bool IsMouseOverButton(List<GameObject> buttons)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        foreach (var obj in raycastResults)
        {
            foreach (var button in buttons)
            {
                if (button == obj.gameObject)
                {
                    return true;
                }
            }
        }
        return false;
    }*/

    public static bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

}