using Leopotam.Ecs;


sealed class AutoServiceSystem : IEcsInitSystem
{
    EcsFilter<ProductSeller, Inventory, AutoService> sellerFilter;
    EcsFilter<Player> playerFilter;
    UIData uiData;
    SceneData sceneData;
    StaticData staticData;
    public void Init()
    {
        BuyBtn.clickEvent += BuyAction;
    }
    void BuyAction()
    {
        foreach (var fSeller in sellerFilter)
        {
            ref var seller = ref sellerFilter.Get1(fSeller);
            if (seller.tradePointData.ableToTrade)
            {
                ref var player = ref playerFilter.Get1(0);
                ref var sellerStorage = ref sellerFilter.Get2(fSeller);


                float playerAvailableMass = 0;
                if (seller.product.type == ProductType.Fuel)
                {
                    playerAvailableMass = (player.maxFuel - player.currentFuel);
                }
                else
                {
                    playerAvailableMass = (player.maxDurability - player.currentDurability);
                }



                float dealMass = 0;
                if (playerAvailableMass < seller.product.mass)
                {
                    dealMass = playerAvailableMass;
                }
                else
                {
                    dealMass = seller.product.mass;
                }

                if (dealMass * seller.product.currentPrice > staticData.currentMoney)
                {
                    dealMass = staticData.currentMoney / seller.product.currentPrice;
                }



                if (dealMass == 0)
                {
                    return;
                }

                sellerStorage.inventory[0].mass -= dealMass;
                staticData.currentMoney -= dealMass * seller.product.currentPrice;
                uiData.moneyText.text = staticData.currentMoney.ToString("0");

                sellerFilter.GetEntity(fSeller).Get<SellDataUpdateRequest>();



                if (seller.product.type == ProductType.Fuel)
                {
                    player.currentFuel += dealMass;
                    uiData.fuelText.text = player.currentFuel.ToString("0");
                }
                else
                {
                    player.currentDurability += dealMass;
                    uiData.durabilityText.text = player.currentDurability.ToString("0");
                }
                SoundData.PlayCoin();
            }
        }
    }
}