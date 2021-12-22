using UnityEngine;

public class PanelSwitchBtn : MonoBehaviour
{
    [SerializeField] GameObject closePanel;
    [SerializeField] GameObject openPanel;


    public void SwitchPanels()
    {
        SoundData.PlayBtn();
        if (openPanel != null)
        {
            openPanel.SetActive(true);
        }
        if (closePanel != null)
        {
            closePanel.SetActive(false);
        }
    }
}
