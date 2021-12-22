using Leopotam.Ecs;
using UnityEngine;


sealed class UpdateUISystem : IEcsInitSystem, IEcsDestroySystem
{
    UIData uiData;
    SceneData sceneData;
    PathData pathData;
    StaticData staticData;
    BuildingsData buildingsData;

    EcsFilter<ProductSeller> sellerFilter;
    EcsFilter<ProductBuyer> buyerFilter;
    EcsFilter<Inventory, Player> playerFilter;

    public void Destroy()
    {
        TradePointData.tradeEvent -= SwitchTradeBtns;
        UIData.updateUIEvent -= UpdateUI;
    }

    public void Init()
    {
        TradePointData.tradeEvent += SwitchTradeBtns;
        UIData.updateUIEvent += UpdateUI;
    }


    void SwitchTradeBtns()
    {
        //buyButton
        bool buyCheck = false;
        foreach (var entitySeller in sellerFilter)
        {
            ref var seller = ref sellerFilter.Get1(entitySeller);

            if (seller.tradePointData.ableToTrade)
            {
                if (!uiData.buyButton.activeSelf)
                {
                    uiData.buyButton.SetActive(true);
                }
                buyCheck = true;
                break;
            }
        }
        if (!buyCheck && uiData.buyButton.activeSelf)
        {
            uiData.buyButton.SetActive(false);
        }

        //sellButton
        bool sellCheck = false;
        foreach (var entityBuyer in buyerFilter)
        {
            ref var seller = ref buyerFilter.Get1(entityBuyer);

            if (seller.tradePointData.ableToTrade)
            {
                if (!uiData.sellButton.activeSelf)
                {
                    uiData.sellButton.SetActive(true);
                }
                sellCheck = true;
                break;
            }
        }
        if (!sellCheck && uiData.sellButton.activeSelf)
        {
            uiData.sellButton.SetActive(false);
        }
    }

    void UpdateUI()
    {
        if (sceneData.gameMode == GameMode.Build)
        {
            uiData.buildBtns.SetActive(true);
            pathData.buildSphere.gameObject.SetActive(true);
        }
        else
        {
            uiData.buildBtns.SetActive(false);
            pathData.buildSphere.gameObject.SetActive(false);
        }

        if (sceneData.gameMode == GameMode.View)
        {
            uiData.tradeBtns.SetActive(true);
        }
        else
        {
            uiData.tradeBtns.SetActive(false);
        }

        ref var player = ref playerFilter.Get2(0);
        ref var playerInventory = ref playerFilter.Get1(0);

        uiData.gameModeText.text = sceneData.gameMode.ToString();
        uiData.fuelText.text = player.currentFuel.ToString("0") + "/" + player.maxFuel.ToString("0");
        uiData.durabilityText.text = player.currentDurability.ToString("0") + "/" + player.maxDurability.ToString("0");
        uiData.cargoText.text = playerInventory.GetCurrentMass().ToString("0") + "/" + playerInventory.maxMass.ToString("0");
        uiData.moneyText.text = staticData.currentMoney.ToString("0.0");


        SwitchTPCanvases();
    }

    void SwitchTPCanvases()
    {
        if (sceneData.gameMode != GameMode.Drive && sceneData.buildCam.position.y > SceneData.BUILDCAM_Y_THRESHOLD)
        {
            foreach (var canvas in buildingsData.tradePointCanvases)
            {
                canvas.SetActive(true);
            }
        }
        else
        {
            foreach (var canvas in buildingsData.tradePointCanvases)
            {
                canvas.SetActive(false);
            }
        }

    }
}
