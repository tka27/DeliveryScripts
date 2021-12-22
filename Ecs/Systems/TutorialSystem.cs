using Leopotam.Ecs;
using UnityEngine;


sealed class TutorialSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    StaticData staticData;
    SceneData sceneData;
    PathData pathData;
    FlowingText flowingText;
    BuildingsData buildingsData;
    UIData uiData;
    GameSettings settings;
    EcsFilter<Inventory, Player> playerFilter;
    EcsFilter<ResearchLab, Inventory, ProductBuyer> labFilter;
    EcsFilter<ProductSeller, Inventory> sellerFilter;
    TutorialData tutorialData;
    const int RESEARCH_SPEED_MULTIPLIER = 10;
    public void Init()
    {
        TradePointData.tradeEvent += OnTradeEvent;

        tutorialData = GameObject.Instantiate(staticData.tutorialPrefab).GetComponent<TutorialData>();

        settings.tutorialLvl = 0;
        sceneData.researchSpeed *= RESEARCH_SPEED_MULTIPLIER;

        tutorialData.yellowText.gameObject.SetActive(true);
        tutorialData.yellowText.text = "This is the game mode button.\nIn view mode you can move/zoom camera and see some info.\nTap on button to change mode";

        tutorialData.blackPanel.gameObject.SetActive(true);

        tutorialData.blackText.text = "In build mode you can build road.\nTap inside yellow sphere for building";
        tutorialData.blackText.gameObject.SetActive(false);
    }

    public void Destroy()
    {
        TradePointData.tradeEvent -= OnTradeEvent;
    }

    void IEcsRunSystem.Run()
    {
        if (staticData.currentMoney < 10)
        {
            staticData.currentMoney = staticData.moneyForGame;
        }

        switch (settings.tutorialLvl)
        {
            case 0:
                if (sceneData.gameMode == GameMode.Build)
                {
                    settings.tutorialLvl++;
                    tutorialData.blackText.gameObject.SetActive(true);
                    SwitchBlackPanel(false);
                }
                break;

            case 1:
                if (pathData.wayPoints.Count != 0)
                {
                    settings.tutorialLvl++;
                    SwitchBlackPanel(true);
                    tutorialData.blackPanel.SetTransform(tutorialData.clearBtnPos);
                    tutorialData.yellowText.text = "Destroy road button will remove the road.\nTap on the button to continue";
                }
                break;

            case 2:
                if (pathData.wayPoints.Count == 0)
                {
                    settings.tutorialLvl++;
                    SwitchBlackPanel(false);
                    tutorialData.blackText.text = "Build road to wheat farm";
                }
                break;

            case 3:
                MoveCamera(buildingsData.wheatTradePoint.transform);
                break;

            case 4:
                if (uiData.isPathConfirmed)
                {
                    settings.tutorialLvl++;
                    tutorialData.blackText.text = "In drive mode tap on the screen to accelerate.\nAcceleration is consuming fuel ";
                }
                break;

            case 5:
                if (playerFilter.Get2(0).playerRB.velocity.magnitude > 2)
                {
                    settings.tutorialLvl++;
                    tutorialData.blackText.text = "Drive to the farm.\nWhen your wheels are off road you take damage based on speed";
                }
                break;

            case 7:
                if (playerFilter.Get2(0).playerRB.velocity.magnitude < 1)
                {
                    settings.tutorialLvl++;
                    tutorialData.farmPanel.SetActive(true);
                    SwitchBlackPanel(true);
                    tutorialData.blackPanel.SetTransform(tutorialData.buyBtnPos);
                }
                break;

            case 8:
                if (playerFilter.Get1(0).HasItem(ProductType.Wheat, 200))
                {
                    settings.tutorialLvl++;
                    SwitchBlackPanel(false);
                    tutorialData.farmPanel.SetActive(false);
                    tutorialData.blackText.text = "Zoom in the camera to open your inventory";
                }
                break;

            case 9:
                if (uiData.inventoryCanvas.activeSelf)
                {
                    tutorialData.nextBtn.SetActive(true);
                    tutorialData.blackText.text = "Clear button will drop all items from your inventory";
                }
                break;

            case 10:

                settings.tutorialLvl++;
                tutorialData.blackText.text = "Deliver 200kg of wheat to the quality control lab";
                break;

            case 11:
                MoveCamera(buildingsData.labTradePoint.transform);
                break;

            case 12:
                if (sceneData.gameMode == GameMode.Drive)
                {
                    settings.tutorialLvl++;
                    tutorialData.blackText.text = "Deers leave droppings when they cross the road, which makes your wheels slippery";
                }
                break;

            case 13:
                if (labFilter.Get3(0).tradePointData.ableToTrade && playerFilter.Get2(0).playerRB.velocity.magnitude < 1)
                {
                    SwitchBlackPanel(true);
                    tutorialData.labPanel.SetActive(true);
                    tutorialData.blackPanel.SetTransform(tutorialData.sellBtnPos);
                    settings.tutorialLvl++;
                }
                break;

            case 14:
                if (labFilter.Get2(0).GetCurrentMass() > 0)
                {
                    settings.tutorialLvl++;
                }
                break;

            case 15:
                SwitchBlackPanel(false);
                tutorialData.labPanel.SetActive(false);
                tutorialData.blackText.text = "Research unlocks new buildings and cars";
                tutorialData.nextBtn.SetActive(true);
                break;

            case 16:
                tutorialData.blackText.text = "In fuel station you can buy fuel";
                settings.tutorialLvl++;
                break;

            case 17:
                MoveCamera(buildingsData.gasStationTradePoint.transform);
                break;

            case 18:
                tutorialData.nextBtn.SetActive(true);
                break;

            case 19:
                tutorialData.blackText.text = "In auto service you can be repaired";
                settings.tutorialLvl++;
                break;

            case 20:
                MoveCamera(buildingsData.autoServiceTradePoint.transform);
                break;

            case 21:
                tutorialData.nextBtn.SetActive(true);
                break;

            case 22:
                tutorialData.blackText.text = "In shop you can sell a products at a high price\nRequired product is updated every 2 minutes or shop storage will be full";
                settings.tutorialLvl++;
                break;

            case 23:
                MoveCamera(buildingsData.shopTradePoint.transform);
                break;
            case 24:
                tutorialData.nextBtn.SetActive(true);
                break;

            case 25:
                tutorialData.blackText.text = "Buy wheat and water";
                settings.tutorialLvl++;
                break;

            case 26:
                MoveCamera(buildingsData.waterTradePoint.transform);
                break;

            case 27:
                ref var inventory = ref playerFilter.Get1(0);
                if (inventory.HasItem(ProductType.Wheat, 1) && inventory.HasItem(ProductType.Water, 1))
                {
                    settings.tutorialLvl++;
                    tutorialData.blackText.text = "Deliver products to bakery";
                }
                break;

            case 28:
                MoveCamera(buildingsData.bakeryTradePoint.transform);
                break;

            case 30:
                tutorialData.bakeryPanel.SetActive(true);
                SwitchBlackPanel(true);
                foreach (var sellerID in sellerFilter)
                {
                    if (sellerFilter.Get2(sellerID).HasItem(ProductType.Bread, 1)) settings.tutorialLvl++;
                }
                break;

            case 31:
                tutorialData.bakeryPanel.SetActive(false);
                SwitchBlackPanel(false);
                tutorialData.blackText.text = "Tutorial is complete\nYou can return to garage for upgrade car and map";
                settings.tutorialLvl++;
                break;

            case 32:
                MoveCamera(tutorialData.transform);
                break;

            case 33:
                GameObject.Destroy(tutorialData.gameObject, 5);
                settings.tutorialLvl = -1;
                sceneData.researchSpeed /= RESEARCH_SPEED_MULTIPLIER;
                settings.SavePrefs();
                
                staticData.currentMoney += 500;
                flowingText.DisplayText("+500");
                SoundData.PlayCoin();
                break;

            default: return;
        }
    }
    void SwitchBlackPanel(bool value)
    {
        tutorialData.blackPanel.gameObject.SetActive(value);
        if (value)
        {
            tutorialData.blackText.text = "";
        }
        else
        {
            tutorialData.yellowText.text = "";
        }
    }
    void MoveCamera(in Transform tf)
    {
        Vector3 tgt = new Vector3(tf.position.x, 30, tf.position.z);
        sceneData.buildCam.position = Vector3.Lerp(sceneData.buildCam.position, tgt, .03f);

        if ((sceneData.buildCam.position - tgt).magnitude < .5f)
        {
            settings.tutorialLvl++;
        }
    }

    void OnTradeEvent()
    {
        switch (settings.tutorialLvl)
        {
            case 6:
                if (IsBuildingReadyToTrade(ProductType.Wheat)) settings.tutorialLvl++;
                break;

            case 29:
                if (IsBuildingReadyToTrade(ProductType.Bread)) settings.tutorialLvl++;
                break;



            default: return;
        }
    }
    bool IsBuildingReadyToTrade(in ProductType productType)
    {
        foreach (var sellerID in sellerFilter)
        {
            ref var seller = ref sellerFilter.Get1(sellerID);
            if (!seller.tradePointData.ableToTrade) continue;

            if (seller.product.type == productType) return true;
        }
        return false;
    }


}



