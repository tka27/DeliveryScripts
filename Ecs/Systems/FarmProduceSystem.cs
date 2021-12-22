using Leopotam.Ecs;


sealed class FarmProduceSystem : IEcsRunSystem
{

    EcsFilter<ProductSeller, Inventory>.Exclude<ProductBuyer> producerFilter;

    void IEcsRunSystem.Run()
    {
        foreach (var fProd in producerFilter)
        {
            ref var producer = ref producerFilter.Get1(fProd);

            producer.productionTimer--;
            if (producer.productionTimer > 0)
            {
                continue;
            }
            producer.productionTimer = 50 / producer.produceSpeed;

            ref var producerInventory = ref producerFilter.Get2(fProd);
            if (producerInventory.inventory.Count == 0)
            {
                producerInventory.inventory.Add(producer.product);
            }
            if (producerInventory.GetCurrentMass() < producerInventory.maxMass)
            { 
                producer.product.mass++;
                producerFilter.GetEntity(fProd).Get<SellDataUpdateRequest>();
            }
        }
    }
}
