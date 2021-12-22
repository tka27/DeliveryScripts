using Leopotam.Ecs;


sealed class QuestUpdateSystem : IEcsRunSystem
{

    EcsFilter<Inventory, Shop> shopFilter;

    void IEcsRunSystem.Run()
    {
        foreach (var fShop in shopFilter)
        {
            ref var shopInventory = ref shopFilter.Get1(fShop);
            if (shopInventory.GetCurrentMass() >= shopInventory.maxMass)
            {
                shopFilter.Get2(fShop).timer = 0;
            }
        }
    }
}
