using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class Garage : MonoBehaviour, IUnityAdsListener
{
    const string PLAYER_TAG = "Player";
    bool isLeaveFromGarage;
    const string REWARDED_AD = "Rewarded_Android";
    [SerializeField] StaticData staticData;
    [SerializeField] SceneData sceneData;
    [SerializeField] GameObject garageCanvas;
    [SerializeField] GameObject adCanvas;
    void Start()
    {
        Advertisement.AddListener(this);
    }
    void OnDestroy()
    {
        Advertisement.RemoveListener(this);
    }
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == PLAYER_TAG && isLeaveFromGarage)
        {
            staticData.totalMoney += staticData.currentMoney;
            isLeaveFromGarage = false;
            ShowAdCanvas();
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == PLAYER_TAG)
        {
            isLeaveFromGarage = true;
        }
    }



    public void ToGarageConfirm()
    {
        staticData.currentMoney /= 2;
        staticData.totalMoney += staticData.currentMoney;
        UIData.UpdateUI();
        ShowAdCanvas();
    }


    public void ShowAd()
    {
        Advertisement.Show(REWARDED_AD);
    }


    void ShowAdCanvas()
    {
        if (Advertisement.IsReady())
        {
            if (garageCanvas != null)
            {
                garageCanvas.SetActive(false);
            }
            adCanvas.SetActive(true);
        }
        else
        {
            GarageEnterProcess();
        }
    }


    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
        //sceneData.Notification("Ad error");
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId != REWARDED_AD) return;
        switch (showResult)
        {
            case ShowResult.Failed:
                sceneData.Notification("Ad error");
                break;

            case ShowResult.Skipped:
                GarageEnterProcess();
                break;

            case ShowResult.Finished:
                staticData.totalMoney += staticData.currentMoney / 2;
                GarageEnterProcess();
                break;

            default: return;
        }
    }


    public void GarageEnterProcess()
    {
        Time.timeScale = 1;
        staticData.currentMoney = 0;
        SaveSystem.Save();
        SceneManager.LoadScene(0);
    }
}
