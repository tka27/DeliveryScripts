using Leopotam.Ecs;
using System.Collections.Generic;


sealed class SellSystem : IEcsInitSystem, IEcsDestroySystem
{

    EcsFilter<ProductBuyer, Inventory> buyerFilter;
    EcsFilter<Inventory, Player> playerFilter;
    StaticData staticData;
    SceneData sceneData;
    SoundData soundData;
    FlowingText flowingText;



    public void Init()
    {
        SellBtn.clickEvent += SellAction;
    }


    public void Destroy()
    {
        SellBtn.clickEvent -= SellAction;
    }
    void SellAction()
    {
        foreach (var fBuyer in buyerFilter)
        {
            ref var buyer = ref buyerFilter.Get1(fBuyer);
            if (buyer.tradePointData.ableToTrade)
            {
                ref var buyerInventory = ref buyerFilter.Get2(fBuyer);


                float buyerFreeSpace = buyerInventory.maxMass - buyerInventory.GetCurrentMass();
                if (buyerFreeSpace == 0)
                {
                    sceneData.Notification("Buyer storage is full");
                    return;
                }


                foreach (var fPlayer in playerFilter)
                {
                    ref var playerInventory = ref playerFilter.Get1(fPlayer);
                    ref var player = ref playerFilter.Get2(fPlayer);

                    List<int> playerIndexes = new List<int>();
                    List<int> buyerIndexes = new List<int>();
                    float minProductMass = 0;
                    foreach (var buyingProductType in buyer.buyingProductTypes)
                    {
                        for (int i = 0; i < playerInventory.inventory.Count; i++) // check all player products & buyer required products
                        {
                            for (int j = 0; j < buyerInventory.inventory.Count; j++)
                            {
                                if (playerInventory.inventory[i].type == buyerInventory.inventory[j].type && buyerInventory.inventory[j].type == buyingProductType)
                                {
                                    playerIndexes.Add(i);
                                    buyerIndexes.Add(j);// add indexes of same products
                                    if (minProductMass == 0 || minProductMass > playerInventory.inventory[i].mass)
                                    {
                                        minProductMass = playerInventory.inventory[i].mass;
                                    }
                                }
                            }
                        }
                    }

                    float playerActualProductsMass = playerIndexes.Count * minProductMass;
                    /*for (int i = 0; i < playerIndexes.Count; i++)
                    {
                        playerActualProductsMass += playerInventory.inventory[playerIndexes[i]].mass;
                    }*/
                    if (playerActualProductsMass == 0)
                    {
                        sceneData.Notification("Nothing to sell");
                        return;
                    }




                    float totalCost = 0;
                    if (playerActualProductsMass <= buyerFreeSpace) // sell all
                    {
                        for (int i = 0; i < playerIndexes.Count; i++)
                        {

                            //float productMass = playerInventory.inventory[playerIndexes[i]].mass / buyer.buyingProductTypes.Count;//fixed

                            buyerInventory.inventory[buyerIndexes[i]].mass += minProductMass;
                            playerInventory.inventory[playerIndexes[i]].mass -= minProductMass;
                            totalCost += minProductMass * buyerInventory.inventory[buyerIndexes[i]].currentPrice;
                        }
                    }
                    else if (playerActualProductsMass > buyerFreeSpace)
                    {
                        float eachProductMass = buyerFreeSpace / playerIndexes.Count;
                        for (int i = 0; i < playerIndexes.Count; i++)
                        {
                            buyerInventory.inventory[buyerIndexes[i]].mass += eachProductMass;
                            playerInventory.inventory[playerIndexes[i]].mass -= eachProductMass;
                            totalCost += eachProductMass * buyerInventory.inventory[buyerIndexes[i]].currentPrice;
                        }
                    }
                    staticData.currentMoney += totalCost;

                    flowingText.DisplayText("+" + totalCost.ToString("0.0"));

                    buyerFilter.GetEntity(fBuyer).Get<BuyDataUpdateRequest>();
                    buyerFilter.GetEntity(fBuyer).Get<OverflowCheckRequest>();
                    playerFilter.GetEntity(0).Get<UpdateCargoRequest>();

                    playerInventory.RemoveEmptySlots();
                    SoundData.PlayCoin();
                    foreach (var cargo in player.carData.playerCargo)
                    {
                        cargo.SetActive(false);
                    }
                }
            }
        }
    }
}

