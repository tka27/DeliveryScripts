using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;


public class BuildingsInitSystem : IEcsInitSystem
{
    EcsWorld _world;
    BuildingsData buildingsData;
    ProductData productData;
    StaticData staticData;
    SceneData sceneData;
    PathData pathData;

    EcsFilter<ProductSeller, Inventory>.Exclude<ProductBuyer> farmFilter;
    EcsFilter<ProductSeller, Inventory, ProductBuyer> factoriesFilter;
    EcsFilter<ProductSeller> allProductionsFilter;
    EcsFilter<ProductBuyer, Shop, Inventory> shopFilter;


    public void Init()
    {
        buildingsData.buildingsInitSystem = this;
        float productPrice;

        if (staticData.researchLvl >= 1)
        {
            BakeryInit();
            WaterInit();
        }

        if (staticData.researchLvl >= 2)
        {
            MilkInit();
            MeatInit();
        }

        if (staticData.researchLvl >= 4)
        {
            FishInit();
        }

        if (staticData.researchLvl >= 6)
        {
            CheeseInit();
        }

        if (staticData.researchLvl >= 8)
        {
            CanFishInit();
        }

        if (staticData.researchLvl >= 10)
        {
            ChickenInit();
        }

        if (staticData.researchLvl >= 15)
        {
            FruitInit();
        }

        if (staticData.researchLvl >= 18)
        {
            VegetablesInit();
        }

        if (staticData.researchLvl >= 19)
        {
            JuiceInit();
        }

        if (staticData.researchLvl >= 24)
        {
            IceInit();
        }

        if (staticData.researchLvl >= 30)
        {
            PizzaInit();
        }





        var wheatFarmEntity = _world.NewEntity();
        ref var wheatFarmSeller = ref wheatFarmEntity.Get<ProductSeller>();
        wheatFarmSeller.sellerGO = buildingsData.wheatTradePoint;
        wheatFarmSeller.produceSpeed = 5;
        wheatFarmSeller.product = new Product(ProductType.Wheat, productData.wheat, 0.12f);
        wheatFarmSeller.repriceMultiplier = 1.2f;
        ref var wheatFarmInventory = ref wheatFarmEntity.Get<Inventory>();
        wheatFarmInventory.maxMass = 200;

        #region Gas

        var gasStationEntity = _world.NewEntity();
        gasStationEntity.Get<AutoService>();
        ref var gasStation = ref gasStationEntity.Get<ProductSeller>();
        gasStation.sellerGO = buildingsData.gasStationTradePoint;
        gasStation.produceSpeed = 0.3f;
        gasStation.repriceMultiplier = 1.1f;
        productPrice = 0.7f + 0.7f * sceneData.researchCurve.Evaluate(staticData.researchLvl);
        gasStation.product = new Product(ProductType.Fuel, productData.fuel, productPrice);
        ref var gasStationInventory = ref gasStationEntity.Get<Inventory>();
        gasStationInventory.maxMass = 200;

        #endregion

        #region Service

        var autoServiceEntity = _world.NewEntity();
        autoServiceEntity.Get<AutoService>();
        ref var autoService = ref autoServiceEntity.Get<ProductSeller>();
        autoService.sellerGO = buildingsData.autoServiceTradePoint;
        autoService.produceSpeed = 0.2f;
        autoService.repriceMultiplier = 1.1f;
        productPrice = 1.2f + 1.2f * sceneData.researchCurve.Evaluate(staticData.researchLvl);
        autoService.product = new Product(ProductType.AutoParts, productData.autoParts, productPrice);
        ref var autoServiceInventory = ref autoServiceEntity.Get<Inventory>();
        autoServiceInventory.maxMass = 200;

        #endregion


        var shopEntity = _world.NewEntity();
        shopEntity.Get<Shop>().maxQuestTime = 180;
        ref var shopBuyer = ref shopEntity.Get<ProductBuyer>();
        shopBuyer.buyingProductTypes = new List<ProductType>();
        shopBuyer.buyerGO = buildingsData.shopTradePoint;
        shopBuyer.repriceMultiplier = 1;
        ref var shopInventory = ref shopEntity.Get<Inventory>();
        shopInventory.maxMass = 200;

        BuildingsUpdate();
    }



    public void WaterInit()
    {
        var waterStationEntity = _world.NewEntity();
        ref var waterStationSeller = ref waterStationEntity.Get<ProductSeller>();
        waterStationSeller.sellerGO = buildingsData.waterTradePoint;
        waterStationSeller.produceSpeed = 5;
        waterStationSeller.product = new Product(ProductType.Water, productData.water, 0.1f);
        waterStationSeller.repriceMultiplier = 1.2f;
        ref var waterStationInventory = ref waterStationEntity.Get<Inventory>();
        waterStationInventory.maxMass = 200;
    }

    public void BakeryInit()
    {
        var bakeryEntity = _world.NewEntity();
        ref var bakeryBuyer = ref bakeryEntity.Get<ProductBuyer>();
        bakeryBuyer.buyerGO = buildingsData.bakeryTradePoint;
        bakeryBuyer.repriceMultiplier = 1.2f;
        bakeryBuyer.buyingProductTypes = new List<ProductType>();
        bakeryBuyer.buyingProductTypes.Add(ProductType.Wheat);
        bakeryBuyer.buyingProductTypes.Add(ProductType.Water);

        ref var bakeryInventory = ref bakeryEntity.Get<Inventory>();
        bakeryInventory.inventory = new List<Product>();
        bakeryInventory.inventory.Add(new Product(ProductType.Wheat, productData.wheat, 0.16f));
        bakeryInventory.inventory.Add(new Product(ProductType.Water, productData.water, 0.13f));
        bakeryInventory.maxMass = 400;

        ref var bakerySeller = ref bakeryEntity.Get<ProductSeller>();
        bakerySeller.produceSpeed = 3;
        bakerySeller.sellerGO = bakeryBuyer.buyerGO;
        bakerySeller.product = new Product(ProductType.Bread, productData.bread, 0.22f);
        bakeryInventory.inventory.Add(bakerySeller.product);
        bakerySeller.repriceMultiplier = 1.2f;
    }

    public void ChickenInit()
    {
        var chickenEntity = _world.NewEntity();
        ref var chickenBuyer = ref chickenEntity.Get<ProductBuyer>();
        chickenBuyer.buyerGO = buildingsData.chickenTradePoint;
        chickenBuyer.repriceMultiplier = 1.2f;
        chickenBuyer.buyingProductTypes = new List<ProductType>();
        chickenBuyer.buyingProductTypes.Add(ProductType.Wheat);

        ref var chickenInventory = ref chickenEntity.Get<Inventory>();
        chickenInventory.inventory = new List<Product>();
        chickenInventory.inventory.Add(new Product(ProductType.Wheat, productData.wheat, 0.16f));
        chickenInventory.maxMass = 200;

        ref var chickenSeller = ref chickenEntity.Get<ProductSeller>();
        chickenSeller.produceSpeed = 5;
        chickenSeller.sellerGO = chickenBuyer.buyerGO;
        chickenSeller.product = new Product(ProductType.Eggs, productData.eggs, 0.26f);
        chickenInventory.inventory.Add(chickenSeller.product);
        chickenSeller.repriceMultiplier = 1.2f;
    }

    public void MilkInit()
    {
        var milkEntity = _world.NewEntity();
        ref var milkBuyer = ref milkEntity.Get<ProductBuyer>();
        milkBuyer.buyerGO = buildingsData.milkFactoryTradePoint;
        milkBuyer.repriceMultiplier = 1.2f;
        milkBuyer.buyingProductTypes = new List<ProductType>();
        milkBuyer.buyingProductTypes.Add(ProductType.Wheat);

        ref var milkInventory = ref milkEntity.Get<Inventory>();
        milkInventory.inventory = new List<Product>();
        milkInventory.inventory.Add(new Product(ProductType.Wheat, productData.milk, 0.16f));
        milkInventory.maxMass = 200;

        ref var milkSeller = ref milkEntity.Get<ProductSeller>();
        milkSeller.product = new Product(ProductType.Milk, productData.milk, 0.24f);
        milkInventory.inventory.Add(milkSeller.product);
        milkSeller.produceSpeed = 4;
        milkSeller.sellerGO = milkBuyer.buyerGO;
        milkSeller.repriceMultiplier = 1.2f;
    }
    public void CheeseInit()
    {
        var cheeseEntity = _world.NewEntity();
        ref var cheeseBuyer = ref cheeseEntity.Get<ProductBuyer>();
        cheeseBuyer.buyerGO = buildingsData.cheeseTradePoint;
        cheeseBuyer.repriceMultiplier = 1.2f;
        cheeseBuyer.buyingProductTypes = new List<ProductType>();
        cheeseBuyer.buyingProductTypes.Add(ProductType.Milk);

        ref var cheeseInventory = ref cheeseEntity.Get<Inventory>();
        cheeseInventory.inventory = new List<Product>();
        cheeseInventory.inventory.Add(new Product(ProductType.Milk, productData.milk, 0.31f));
        cheeseInventory.maxMass = 200;

        ref var cheeseSeller = ref cheeseEntity.Get<ProductSeller>();
        cheeseSeller.product = new Product(ProductType.Cheese, productData.cheese, 0.47f);
        cheeseInventory.inventory.Add(cheeseSeller.product);
        cheeseSeller.produceSpeed = 3;
        cheeseSeller.sellerGO = cheeseBuyer.buyerGO;
        cheeseSeller.repriceMultiplier = 1.2f;
    }

    public void MeatInit()
    {
        var meatEntity = _world.NewEntity();
        ref var meatBuyer = ref meatEntity.Get<ProductBuyer>();
        meatBuyer.buyerGO = buildingsData.meatFactoryTradePoint;
        meatBuyer.repriceMultiplier = 1.2f;
        meatBuyer.buyingProductTypes = new List<ProductType>();
        meatBuyer.buyingProductTypes.Add(ProductType.Wheat);

        ref var meatInventory = ref meatEntity.Get<Inventory>();
        meatInventory.inventory = new List<Product>();
        meatInventory.inventory.Add(new Product(ProductType.Wheat, productData.wheat, 0.16f));
        meatInventory.maxMass = 200;

        ref var meatSeller = ref meatEntity.Get<ProductSeller>();
        meatSeller.product = new Product(ProductType.Meat, productData.meat, 0.28f);
        meatInventory.inventory.Add(meatSeller.product);
        meatSeller.produceSpeed = 3;
        meatSeller.sellerGO = meatBuyer.buyerGO;
        meatSeller.repriceMultiplier = 1.2f;
    }

    public void PizzaInit()
    {
        var pizzaEntity = _world.NewEntity();
        ref var pizzaBuyer = ref pizzaEntity.Get<ProductBuyer>();
        pizzaBuyer.buyerGO = buildingsData.pizzaTradePoint;
        pizzaBuyer.repriceMultiplier = 1.2f;
        pizzaBuyer.buyingProductTypes = new List<ProductType>();
        pizzaBuyer.buyingProductTypes.Add(ProductType.Bread);
        pizzaBuyer.buyingProductTypes.Add(ProductType.Meat);
        pizzaBuyer.buyingProductTypes.Add(ProductType.Cheese);

        ref var pizzaInventory = ref pizzaEntity.Get<Inventory>();
        pizzaInventory.inventory = new List<Product>();
        pizzaInventory.inventory.Add(new Product(ProductType.Bread, productData.bread, 0.29f));
        pizzaInventory.inventory.Add(new Product(ProductType.Meat, productData.meat, 0.36f));
        pizzaInventory.inventory.Add(new Product(ProductType.Cheese, productData.cheese, 0.61f));
        pizzaInventory.maxMass = 600;

        ref var pizzaSeller = ref pizzaEntity.Get<ProductSeller>();
        pizzaSeller.product = new Product(ProductType.Pizza, productData.pizza, 0.63f);
        pizzaInventory.inventory.Add(pizzaSeller.product);
        pizzaSeller.produceSpeed = 2;
        pizzaSeller.sellerGO = pizzaBuyer.buyerGO;
        pizzaSeller.repriceMultiplier = 1.2f;
    }

    public void FishInit()
    {
        var fishFarmEntity = _world.NewEntity();
        ref var fishFarmSeller = ref fishFarmEntity.Get<ProductSeller>();
        fishFarmSeller.sellerGO = buildingsData.fishTradePoint;
        fishFarmSeller.produceSpeed = 5;
        fishFarmSeller.product = new Product(ProductType.Fish, productData.fish, 0.2f);
        fishFarmSeller.repriceMultiplier = 1.2f;
        ref var fishFarmInventory = ref fishFarmEntity.Get<Inventory>();
        fishFarmInventory.maxMass = 200;
    }

    public void CanFishInit()
    {
        var canFishEntity = _world.NewEntity();
        ref var canFishBuyer = ref canFishEntity.Get<ProductBuyer>();
        canFishBuyer.buyerGO = buildingsData.canFishTradePoint;
        canFishBuyer.repriceMultiplier = 1.2f;
        canFishBuyer.buyingProductTypes = new List<ProductType>();
        canFishBuyer.buyingProductTypes.Add(ProductType.Fish);

        ref var canFishInventory = ref canFishEntity.Get<Inventory>();
        canFishInventory.inventory = new List<Product>();
        canFishInventory.inventory.Add(new Product(ProductType.Fish, productData.fish, 0.26f));
        canFishInventory.maxMass = 200;

        ref var canFishSeller = ref canFishEntity.Get<ProductSeller>();
        canFishSeller.produceSpeed = 4;
        canFishSeller.sellerGO = canFishBuyer.buyerGO;
        canFishSeller.product = new Product(ProductType.CannedFish, productData.cannedFish, 0.39f);
        canFishInventory.inventory.Add(canFishSeller.product);
        canFishSeller.repriceMultiplier = 1.2f;
    }

    public void FruitInit()
    {
        var fruitFarmEntity = _world.NewEntity();
        ref var fruitFarmSeller = ref fruitFarmEntity.Get<ProductSeller>();
        fruitFarmSeller.sellerGO = buildingsData.fruitTradePoint;
        fruitFarmSeller.produceSpeed = 4;
        fruitFarmSeller.product = new Product(ProductType.Fruits, productData.fruits, 0.2f);
        fruitFarmSeller.repriceMultiplier = 1.2f;
        ref var fruitFarmInventory = ref fruitFarmEntity.Get<Inventory>();
        fruitFarmInventory.maxMass = 300;
    }

    public void VegetablesInit()
    {
        var vegetableFarmEntity = _world.NewEntity();
        ref var vegetableFarmSeller = ref vegetableFarmEntity.Get<ProductSeller>();
        vegetableFarmSeller.sellerGO = buildingsData.vegetableTradePoint;
        vegetableFarmSeller.produceSpeed = 3;
        vegetableFarmSeller.product = new Product(ProductType.Vegetables, productData.vegetables, 0.3f);
        vegetableFarmSeller.repriceMultiplier = 1.2f;
        ref var vegetableFarmInventory = ref vegetableFarmEntity.Get<Inventory>();
        vegetableFarmInventory.maxMass = 300;
    }

    public void JuiceInit()
    {
        var juiceEntity = _world.NewEntity();
        ref var juiceBuyer = ref juiceEntity.Get<ProductBuyer>();
        juiceBuyer.buyerGO = buildingsData.juiceTradePoint;
        juiceBuyer.repriceMultiplier = 1.2f;
        juiceBuyer.buyingProductTypes = new List<ProductType>();
        juiceBuyer.buyingProductTypes.Add(ProductType.Water);
        juiceBuyer.buyingProductTypes.Add(ProductType.Fruits);

        ref var juiceInventory = ref juiceEntity.Get<Inventory>();
        juiceInventory.inventory = new List<Product>();
        juiceInventory.inventory.Add(new Product(ProductType.Water, productData.water, 0.15f));
        juiceInventory.inventory.Add(new Product(ProductType.Fruits, productData.fruits, 0.33f));
        juiceInventory.maxMass = 200;

        ref var juiceSeller = ref juiceEntity.Get<ProductSeller>();
        juiceSeller.product = new Product(ProductType.Juice, productData.juice, 0.36f);
        juiceInventory.inventory.Add(juiceSeller.product);
        juiceSeller.produceSpeed = 3;
        juiceSeller.sellerGO = juiceBuyer.buyerGO;
        juiceSeller.repriceMultiplier = 1.2f;
    }

    public void IceInit()
    {
        var iceEntity = _world.NewEntity();
        ref var iceBuyer = ref iceEntity.Get<ProductBuyer>();
        iceBuyer.buyerGO = buildingsData.iceTradePoint;
        iceBuyer.repriceMultiplier = 1.2f;
        iceBuyer.buyingProductTypes = new List<ProductType>();
        iceBuyer.buyingProductTypes.Add(ProductType.Water);
        iceBuyer.buyingProductTypes.Add(ProductType.Fruits);

        ref var iceInventory = ref iceEntity.Get<Inventory>();
        iceInventory.inventory = new List<Product>();
        iceInventory.inventory.Add(new Product(ProductType.Milk, productData.milk, 0.36f));
        iceInventory.inventory.Add(new Product(ProductType.Fruits, productData.fruits, 0.42f));
        iceInventory.maxMass = 400;

        ref var iceSeller = ref iceEntity.Get<ProductSeller>();
        iceSeller.product = new Product(ProductType.Juice, productData.iceCream, 0.57f);
        iceInventory.inventory.Add(iceSeller.product);
        iceSeller.produceSpeed = 2;
        iceSeller.sellerGO = iceBuyer.buyerGO;
        iceSeller.repriceMultiplier = 1.2f;
    }
















    void BuildingsUpdate()
    {
        foreach (var farmInd in farmFilter)
        {
            ref var farm = ref farmFilter.Get1(farmInd);
            farm.produceSpeed += farm.produceSpeed / 100 * 5 * staticData.mapPerks[staticData.selectedMapID][0];

            ref var farmInventory = ref farmFilter.Get2(farmInd);
            farmInventory.inventory = new List<Product>();
            farmInventory.maxMass += farmInventory.maxMass / 100 * 5 * staticData.mapPerks[staticData.selectedMapID][1];


            farm.tradePointData = farm.sellerGO.GetComponent<TradePointData>();
            pathData.finalPoints.Add(farm.tradePointData.finalPoint);
            buildingsData.tradePointCanvases.Add(farm.tradePointData.canvas);
            farmFilter.GetEntity(farmInd).Get<SellDataUpdateRequest>();
        }

        foreach (var factoryInd in factoriesFilter)
        {
            ref var factory = ref factoriesFilter.Get1(factoryInd);
            factory.produceSpeed += factory.produceSpeed / 100 * 5 * staticData.mapPerks[staticData.selectedMapID][2];

            ref var factoryInventory = ref factoriesFilter.Get2(factoryInd);
            factoryInventory.maxMass += factoryInventory.maxMass / 100 * 5 * staticData.mapPerks[staticData.selectedMapID][3];


            factory.tradePointData = factory.sellerGO.GetComponent<TradePointData>();
            factoriesFilter.Get3(factoryInd).tradePointData = factory.tradePointData;
            pathData.finalPoints.Add(factory.tradePointData.finalPoint);

            buildingsData.tradePointCanvases.Add(factory.tradePointData.canvas);
            factoriesFilter.GetEntity(factoryInd).Get<BuyDataUpdateRequest>();
            factoriesFilter.GetEntity(factoryInd).Get<SellDataUpdateRequest>();
        }

        foreach (var shopInd in shopFilter)
        {
            ref var shopInventory = ref shopFilter.Get3(shopInd);
            shopInventory.maxMass += shopInventory.maxMass / 100 * 5 * staticData.mapPerks[staticData.selectedMapID][4];
            shopInventory.inventory = new List<Product>();


            ref var shop = ref shopFilter.Get1(shopInd);
            shop.tradePointData = shop.buyerGO.GetComponent<TradePointData>();
            pathData.finalPoints.Add(shop.tradePointData.finalPoint);
            buildingsData.tradePointCanvases.Add(shop.tradePointData.canvas);
            shopFilter.GetEntity(shopInd).Get<BuyDataUpdateRequest>();
        }
        UnlockBuildings();
    }

    void NewFarm()
    {
        var lastEntity = farmFilter.GetEntity(farmFilter.GetEntitiesCount() - 1);
        ref var farm = ref lastEntity.Get<ProductSeller>();
        farm.produceSpeed += farm.produceSpeed / 100 * 5 * staticData.mapPerks[staticData.selectedMapID][0];

        ref var farmInventory = ref lastEntity.Get<Inventory>();
        farmInventory.inventory = new List<Product>();
        farmInventory.maxMass += farmInventory.maxMass / 100 * 5 * staticData.mapPerks[staticData.selectedMapID][1];


        farm.tradePointData = farm.sellerGO.GetComponent<TradePointData>();
        pathData.finalPoints.Add(farm.tradePointData.finalPoint);
        buildingsData.tradePointCanvases.Add(farm.tradePointData.canvas);
        lastEntity.Get<SellDataUpdateRequest>();
    }
    void NewFactory()
    {
        var lastEntity = factoriesFilter.GetEntity(factoriesFilter.GetEntitiesCount() - 1);

        ref var factory = ref lastEntity.Get<ProductSeller>();
        factory.produceSpeed += factory.produceSpeed / 100 * 5 * staticData.mapPerks[staticData.selectedMapID][2];

        ref var factoryInventory = ref lastEntity.Get<Inventory>();
        factoryInventory.maxMass += factoryInventory.maxMass / 100 * 5 * staticData.mapPerks[staticData.selectedMapID][3];


        factory.tradePointData = factory.sellerGO.GetComponent<TradePointData>();
        lastEntity.Get<ProductBuyer>().tradePointData = factory.tradePointData;
        pathData.finalPoints.Add(factory.tradePointData.finalPoint);

        buildingsData.tradePointCanvases.Add(factory.tradePointData.canvas);
        lastEntity.Get<BuyDataUpdateRequest>();
        lastEntity.Get<SellDataUpdateRequest>();
    }

    public void NewProductionBuilding()
    {
        var lastEntity = allProductionsFilter.GetEntity(allProductionsFilter.GetEntitiesCount() - 1);
        if (lastEntity.Has<ProductBuyer>())
        {
            NewFactory();
        }
        else
        {
            NewFarm();
        }
        UnlockBuildings();
    }

    void UnlockBuildings()
    {
        SwitchBuildings(1, buildingsData.bakeryTradePoint, buildingsData.waterTradePoint, buildingsData.cowFarmGO);

        SwitchBuildings(2, buildingsData.milkFactoryTradePoint, buildingsData.meatFactoryTradePoint, buildingsData.fishGO);

        SwitchBuildings(4, buildingsData.fishTradePoint, buildingsData.cheeseGO);

        SwitchBuildings(6, buildingsData.cheeseTradePoint, buildingsData.canFishGO);

        SwitchBuildings(8, buildingsData.canFishTradePoint, buildingsData.chickenGO);
    
        SwitchBuildings(10, buildingsData.chickenTradePoint, buildingsData.fruitGO);
        
        SwitchBuildings(13, buildingsData.fruitTradePoint, buildingsData.vegetableGO);
        
        SwitchBuildings(16, buildingsData.vegetableTradePoint, buildingsData.juiceGO);

        SwitchBuildings(19, buildingsData.juiceTradePoint, buildingsData.iceGO);

        SwitchBuildings(24, buildingsData.iceTradePoint);

        SwitchBuildings(30, buildingsData.pizzaTradePoint);

        sceneData.navMeshSurface.BuildNavMesh();
    }

    void SwitchBuildings(int requiredLvl, params GameObject[] gameObjects)
    {
        if (staticData.researchLvl >= requiredLvl)
        {
            foreach (var go in gameObjects)
            {
                go.SetActive(true);
            }
        }
        else
        {
            foreach (var go in gameObjects)
            {
                go.SetActive(false);
            }
        }
    }
}



