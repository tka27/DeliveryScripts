using UnityEngine;
using UnityEngine.UI;

public class BuyUpgradeBtn : MonoBehaviour
{
    [SerializeField] MainMenuSceneData mainMenuSceneData;
    [SerializeField] StaticData staticData;
    [SerializeField] UpgradeInfo buyUpgradeInfo;
    [SerializeField] Text totalMoney;
    public void BuyCarUpgrade()
    {
        SoundData.PlayBtn();
        float perkCost = buyUpgradeInfo.perksPrices[mainMenuSceneData.selectedPerkID];
        if (staticData.totalMoney > perkCost)
        {
            staticData.totalMoney -= perkCost;
            staticData.carPerks[staticData.selectedCarID][mainMenuSceneData.selectedPerkID]++; //selected perk for current car

            totalMoney.text = staticData.totalMoney.ToString("0");
            buyUpgradeInfo.CarUpgradeUpdate();
            SoundData.PlayCoin();
        }
        else
        {
            MainMenu.Notification("Not enough money");
        }
    }

    public void BuyMapUpgrade()
    {
        SoundData.PlayBtn();
        float perkCost = buyUpgradeInfo.perksPrices[mainMenuSceneData.selectedPerkID];
        if (staticData.totalMoney > perkCost)
        {
            staticData.totalMoney -= perkCost;
            staticData.mapPerks[staticData.selectedMapID][mainMenuSceneData.selectedPerkID]++; //selected perk for current car

            totalMoney.text = staticData.totalMoney.ToString("0");
            buyUpgradeInfo.MapUpgradeUpdate();
            SoundData.PlayCoin();
        }
        else
        {
            MainMenu.Notification("Not enough money");
        }
    }

}
