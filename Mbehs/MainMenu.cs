using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] StaticData staticData;
    [SerializeField] GameSettings settings;
    [SerializeField] Text totalMoney;
    [SerializeField] GameObject demoCam;
    static GameObject staticNotificationPanel;
    static Text staticNotificationText;
    [SerializeField] GameObject notificationPanel;
    [SerializeField] Text notificationText;
    [SerializeField] UnityEvent carInfoUpdateEvent;

    void Start()
    {
        staticNotificationPanel = notificationPanel;
        staticNotificationText = notificationText;
        demoCam.SetActive(false);

        staticData.SetDefaultData();
        settings.LoadPrefs();
        LoadGameProgress();
        carInfoUpdateEvent.Invoke();
        totalMoney.text = staticData.totalMoney.ToString("0.0");
    }



    void LoadGameProgress() //copy SaveData to staticData
    {
        SaveData data = SaveSystem.Load();
        if (data != null)
        {
            staticData.UpdateStaticData(data);
        }
    }



    public static void Notification(string notification)
    {
        staticNotificationPanel.SetActive(true);
        staticNotificationText.text = notification;
    }

}
