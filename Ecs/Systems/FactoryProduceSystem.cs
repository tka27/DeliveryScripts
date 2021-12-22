using Leopotam.Ecs;


sealed class FactoryProduceSystem : IEcsRunSystem
{

    EcsFilter<ProductSeller, Inventory, ProductBuyer> producerFilter;

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
            if (producerInventory.GetCurrentMass() < 1) continue;


            ref var consumer = ref producerFilter.Get3(fProd);
            bool anyBuyingProductMassIsZero = false;


            foreach (var inventoryItem in producerInventory.inventory)
            {
                foreach (var productType in consumer.buyingProductTypes)
                {
                    if (productType == inventoryItem.type)
                    {
                        if (inventoryItem.mass < 1)
                        {
                            anyBuyingProductMassIsZero = true;
                        }
                    }
                }
            }
            if (anyBuyingProductMassIsZero) continue;

            foreach (var product in consumer.buyingProductTypes)
            {
                foreach (var inventoryItem in producerInventory.inventory)
                {
                    if (product == inventoryItem.type)
                    {
                        inventoryItem.mass--;
                    }
                }
                foreach (var inventoryItem in producerInventory.inventory)
                {
                    if (producer.product.type == inventoryItem.type)
                    {
                        inventoryItem.mass++;
                    }
                }
            }

            producerFilter.GetEntity(fProd).Get<BuyDataUpdateRequest>();
            producerFilter.GetEntity(fProd).Get<SellDataUpdateRequest>();

        }
    }
}
