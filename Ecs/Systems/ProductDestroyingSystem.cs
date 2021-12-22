using Leopotam.Ecs;


sealed class ProductDestroyingSystem : IEcsRunSystem
{
    EcsFilter<Inventory, ProductBuyer, OverflowCheckRequest> destroyFilter;
    const float THRESHOLD_MULTIPLIER = 1.5f;

    void IEcsRunSystem.Run()
    {
        foreach (var destroyEI in destroyFilter)
        {
            ref var request = ref destroyFilter.Get3(destroyEI);

            request.timer--;
            if (request.timer > 0)
            {
                continue;
            }
            request.timer = 50;


            ref var buyerInventory = ref destroyFilter.Get1(destroyEI);
            ref var buyer = ref destroyFilter.Get2(destroyEI);

            float spacePerProduct = buyerInventory.maxMass / buyer.buyingProductTypes.Count * THRESHOLD_MULTIPLIER;
            bool haveOverflowing = false;

            foreach (var item in buyerInventory.inventory)
            {
                if (item.mass > spacePerProduct)
                {
                    item.mass--;
                    haveOverflowing = true;
                }
            }
            if (!haveOverflowing)
            {
                destroyFilter.GetEntity(destroyEI).Del<OverflowCheckRequest>();
            }

            destroyFilter.GetEntity(destroyEI).Get<BuyDataUpdateRequest>();
        }
    }
}