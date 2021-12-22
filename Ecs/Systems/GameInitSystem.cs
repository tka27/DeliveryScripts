using Leopotam.Ecs;
using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine.AI;
using System.Collections;

public class GameInitSystem : IEcsInitSystem
{
    EcsWorld _world;
    SceneData sceneData;
    BuildingsData buildingsData;
    StaticData staticData;
    SoundData soundData;
    PathData pathData;
    GameSettings settings;
    UIData uiData;
    ProductData productData;

    public void Init()
    {
        if (Application.isEditor) LoadForTests();


        staticData.currentMoney = staticData.moneyForGame;
        staticData.UpdateAvailableProducts();

        PlayerInit();
        LabInit();
        AnimalsInit();

    }

    void PlayerInit()
    {
        var playerEntity = _world.NewEntity();
        ref var playerComp = ref playerEntity.Get<Player>();
        playerEntity.Get<Movable>();
        sceneData.cars[staticData.selectedCarID].SetActive(true);
        playerComp.playerGO = sceneData.cars[staticData.selectedCarID];
        playerComp.carData = playerComp.playerGO.GetComponent<CarData>();
        playerComp.playerRB = playerComp.playerGO.GetComponent<Rigidbody>();
        playerComp.playerRB.mass = playerComp.carData.defaultMass;
        playerComp.playerRB.centerOfMass = playerComp.carData.centerOfMass.transform.localPosition;
        playerComp.maxSteerAngle = playerComp.carData.maxSteerAngle;
        playerComp.maxFuel = playerComp.carData.maxFuel + playerComp.carData.maxFuel / 100 * 5 * staticData.carPerks[staticData.selectedCarID][0];
        playerComp.maxTorque = playerComp.carData.maxTorque + playerComp.carData.maxTorque / 100 * 5 * staticData.carPerks[staticData.selectedCarID][1];
        playerComp.acceleration = playerComp.carData.acceleration + playerComp.carData.acceleration / 100 * 5 * staticData.carPerks[staticData.selectedCarID][2];
        playerComp.maxDurability = playerComp.carData.maxDurability + playerComp.carData.maxDurability / 100 * 5 * staticData.carPerks[staticData.selectedCarID][3];
        playerComp.currentDurability = playerComp.maxDurability;
        uiData.durabilityText.text = playerComp.currentDurability.ToString();
        playerComp.currentFuel = playerComp.maxFuel;
        uiData.fuelText.text = playerComp.currentFuel.ToString();
        playerComp.fuelConsumption = playerComp.carData.drivingWheelColliders.Count * playerComp.acceleration / 500;
        ref var playerInventory = ref playerEntity.Get<Inventory>();
        playerInventory.inventory = new List<Product>();
        if (!staticData.trailerIsSelected)
        {
            playerInventory.maxMass = playerComp.carData.carStorage + playerComp.carData.carStorage / 100 * 5 * staticData.carPerks[staticData.selectedCarID][4];
        }
        else
        {
            playerInventory.maxMass = playerComp.carData.carStorage + playerComp.carData.trailerStorage + playerComp.carData.carStorage / 100 * 5 * staticData.carPerks[staticData.selectedCarID][4];
        }

        playerEntity.Get<UpdateCargoRequest>();



        for (int i = 0; i < playerComp.carData.playerCargo.Count; i++)
        {
            playerComp.carData.playerCargoRB.Add(playerComp.carData.playerCargo[i].gameObject.GetComponent<Rigidbody>());
            playerComp.carData.playerCargoDefaultPos.Add(playerComp.carData.playerCargo[i].transform.localPosition);
            playerComp.carData.playerCargoDefaultRot.Add(playerComp.carData.playerCargo[i].transform.localRotation);
        }
        if (staticData.trailerIsSelected)
        {
            playerComp.carData.trailer.SetActive(true);
        }
        else
        {
            playerComp.carData.trailer.SetActive(false);
        }
        playerComp.activeWheelColliders = new List<WheelCollider>();
        foreach (var wc in playerComp.carData.allWheelColliders)
        {
            if (wc.gameObject.activeInHierarchy)
            {
                playerComp.activeWheelColliders.Add(wc);
            }
        }
        foreach (var wheel in playerComp.carData.allWheelMeshes)
        {
            if (wheel.gameObject.activeInHierarchy)
            {
                playerComp.carData.wheelDatas.Add(wheel.GetComponent<WheelData>());
            }
        }

        soundData.loopSounds.Add(playerComp.carData.engineSound);
        soundData.SwitchLoopSounds(settings.sound);


        var virtualCam = sceneData.driveCam.GetComponent<CinemachineVirtualCamera>();
        virtualCam.Follow = playerComp.playerGO.transform;
        virtualCam.LookAt = playerComp.carData.cameraLookPoint;
    }

    void LabInit()
    {
        CreateResearchList();
        if (staticData.researchLvl < sceneData.researchList.Length)
        {
            buildingsData.labTradePoint.SetActive(true);
            var labEntity = _world.NewEntity();
            ref var labComp = ref labEntity.Get<ResearchLab>();
            ref var labBuyer = ref labEntity.Get<ProductBuyer>();
            labBuyer.buyingProductTypes = new List<ProductType>();
            labBuyer.buyerGO = buildingsData.labTradePoint;
            labBuyer.tradePointData = labBuyer.buyerGO.GetComponent<TradePointData>();
            labBuyer.repriceMultiplier = 1;
            ref var labInventory = ref labEntity.Get<Inventory>();
            labInventory.inventory = new List<Product>();
            labEntity.Get<LabUpdateRequest>();
            labEntity.Get<BuyDataUpdateRequest>();
            pathData.finalPoints.Add(labBuyer.tradePointData.finalPoint);
            buildingsData.tradePointCanvases.Add(labBuyer.tradePointData.canvas);
        }
        else
        {
            buildingsData.labTradePoint.SetActive(false);
        }


    }


    void CreateResearchList()
    {
        sceneData.researchList = new Product[]{
        new Product(ProductType.Wheat, productData.wheat, 0),
        new Product(ProductType.Water, productData.water, 0),
        new Product(ProductType.Wheat, productData.wheat, 0),
        new Product(ProductType.Meat, productData.meat, 0),
        new Product(ProductType.Bread, productData.bread, 0),
        new Product(ProductType.Wheat, productData.wheat, 0),
        new Product(ProductType.Milk, productData.milk, 0),
        new Product(ProductType.Meat, productData.meat, 0),
        new Product(ProductType.Fish, productData.fish, 0),
        new Product(ProductType.Water, productData.water, 0),
        new Product(ProductType.Wheat, productData.wheat, 0),
        new Product(ProductType.Bread, productData.bread, 0),
        new Product(ProductType.Water, productData.water, 0),
        new Product(ProductType.Meat, productData.meat, 0),
        new Product(ProductType.Eggs, productData.eggs, 0),
        new Product(ProductType.Water, productData.water, 0),
        new Product(ProductType.Fruits, productData.fruits, 0),
        new Product(ProductType.Vegetables, productData.vegetables, 0),
        new Product(ProductType.Fruits, productData.fruits, 0),
        new Product(ProductType.Water, productData.water, 0),
        new Product(ProductType.Wheat, productData.wheat, 0),
        new Product(ProductType.Water, productData.water, 0),
        new Product(ProductType.Fruits, productData.fruits, 0),
        new Product(ProductType.Juice, productData.juice, 0),
        new Product(ProductType.Milk, productData.milk, 0),
        new Product(ProductType.Wheat, productData.wheat, 0),
        new Product(ProductType.Water, productData.water, 0),
        new Product(ProductType.Milk, productData.milk, 0),
        new Product(ProductType.Bread, productData.bread, 0),
        new Product(ProductType.Meat, productData.meat, 0),
        new Product(ProductType.Cheese, productData.cheese, 0),
        };



    }


    void LoadForTests()
    {
        settings.LoadPrefs();

        staticData.carPerks = new int[sceneData.cars.Count][];
        for (int i = 0; i < sceneData.cars.Count; i++)
        {
            staticData.carPerks[i] = new int[5];
        }

        int mapsCount = 1;
        staticData.mapPerks = new int[mapsCount][];
        for (int i = 0; i < mapsCount; i++)
        {
            staticData.mapPerks[i] = new int[5];
        }

        LoadGameProgress();
    }


    void LoadGameProgress() //copy SaveData to staticData
    {
        SaveData data = SaveSystem.Load();
        if (data != null)
        {
            staticData.UpdateStaticData(data);
        }
    }


    void AnimalsInit()
    {
        foreach (var animal in sceneData.animalsPool)
        {
            _world.NewEntity().Get<Animal>().animalData = animal.GetComponent<AnimalData>();
        }
    }
    
}
