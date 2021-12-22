using Leopotam.Ecs;
using UnityEngine;


sealed class BuySystem : IEcsInitSystem,IEcsPostDestroySystem
{

    EcsFilter<ProductSeller, Inventory>.Exclude<AutoService> sellerFilter;
    EcsFilter<Inventory, Player> playerFilter;
    StaticData staticData;
    SceneData sceneData;
    SoundData soundData;
    FlowingText flowingText;

    public void Init()
    {
        BuyBtn.clickEvent += BuyAction;
    }

    public void PostDestroy()
    {
    }

    void BuyAction()
    {
        foreach (var fSeller in sellerFilter)
        {
            ref var seller = ref sellerFilter.Get1(fSeller);
            if (seller.tradePointData.ableToTrade)
            {
                ref var sellerInventory = ref sellerFilter.Get2(fSeller);

                bool isProductAvailable = false;
                foreach (var playerProdType in staticData.availableProductTypes)
                {
                    if (playerProdType == seller.product.type)
                    {
                        isProductAvailable = true;
                    }
                }
                if (!isProductAvailable)
                {
                    sceneData.Notification("You can't transport this product");
                    return;
                }


                foreach (var fPlayer in playerFilter)
                {
                    ref var playerInventory = ref playerFilter.Get1(fPlayer);
                    ref var player = ref playerFilter.Get2(fPlayer);

                    float playerAvailableMass = (playerInventory.maxMass - playerInventory.GetCurrentMass());
                    float dealMass = 0;
                    if (playerAvailableMass < seller.product.mass)
                    {
                        dealMass = playerAvailableMass;
                        if (dealMass == 0)
                        {
                            sceneData.Notification("Inventory is full");
                            return;
                        }
                    }
                    else
                    {
                        dealMass = seller.product.mass;
                        if (dealMass == 0)
                        {
                            sceneData.Notification("No products for sale");
                            return;
                        }
                    }


                    if (dealMass * seller.product.currentPrice > staticData.currentMoney)
                    {
                        dealMass = staticData.currentMoney / seller.product.currentPrice;
                    }


                    bool haveProduct = false;
                    foreach (var product in playerInventory.inventory)
                    {
                        if (product.type == seller.product.type)
                        {
                            product.mass += dealMass;
                            haveProduct = true;
                        }
                    }


                    if (!haveProduct)
                    {
                        playerInventory.inventory.Add(new Product(seller.product.type, dealMass, seller.product.icon, seller.product.defaultPrice));
                    }
                    sellerFilter.GetEntity(fSeller).Get<SellDataUpdateRequest>();
                    playerFilter.GetEntity(0).Get<UpdateCargoRequest>();

                    seller.product.mass -= dealMass;
                    float dealCost = dealMass * seller.product.currentPrice;
                    staticData.currentMoney -= dealCost;
                    flowingText.DisplayText((-dealCost).ToString("0.0"));
                    SoundData.PlayCoin();


                    foreach (var go in player.carData.playerCargo)
                    {
                        go.SetActive(false);
                    }
                }
            }
        }
    }
}
