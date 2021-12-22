using Leopotam.Ecs;
using System.Collections.Generic;
using System;


sealed class ShopQuestSystem : IEcsRunSystem
{

    EcsFilter<ProductBuyer, Shop, Inventory> shopFilter;
    EcsFilter<ProductSeller>.Exclude<AutoService> sellerFilter;
    StaticData staticData;

    void IEcsRunSystem.Run()
    {
        foreach (var fShop in shopFilter)
        {
            ref var quest = ref shopFilter.Get2(fShop);
            ref var shopInventory = ref shopFilter.Get3(fShop);

            quest.timer--;
            if (quest.timer > 0)
            {
                continue;
            }
            quest.timer = 50;

            ref var buyer = ref shopFilter.Get1(fShop);
            quest.currentQuestTime--;
            TimeSpan time = new TimeSpan();

            if (quest.currentQuestTime > 0)
            {
                time = TimeSpan.FromSeconds(quest.currentQuestTime);
                buyer.tradePointData.currentQuestTime.text = time.ToString("mm':'ss");
                continue;
            }
            quest.currentQuestTime = quest.maxQuestTime;
            time = TimeSpan.FromSeconds(quest.currentQuestTime);
            buyer.tradePointData.currentQuestTime.text = time.ToString("mm':'ss");

            Product product = SelectRandomSimilarProduct();
            buyer.buyingProductTypes.Clear();
            buyer.buyingProductTypes.Add(product.type);

            shopInventory.inventory.Clear();
            shopInventory.inventory.Add(new Product(product.type, product.icon, product.defaultPrice * 2f));
            buyer.tradePointData.buyProductImage.sprite = product.icon;
            shopFilter.GetEntity(fShop).Get<BuyDataUpdateRequest>();
        }
    }

    Product SelectRandomSimilarProduct()
    {
        List<Product> similarProducts = new List<Product>();
        foreach (var fSeller in sellerFilter)
        {
            ref var seller = ref sellerFilter.Get1(fSeller);
            foreach (var prodType in staticData.availableProductTypes)
            {
                if (prodType == seller.product.type)
                {
                    similarProducts.Add(seller.product);
                }
            }
        }




        int randomIndex = UnityEngine.Random.Range(0, similarProducts.Count);
        if (similarProducts.Count > 0)
        {
            return similarProducts[randomIndex];
        }
        return SelectRandomProducedProduct();
    }
    
    Product SelectRandomProducedProduct()
    {
        List<Product> products = new List<Product>();
        foreach (var fSeller in sellerFilter)
        {
            ref var seller = ref sellerFilter.Get1(fSeller);
            products.Add(seller.product);
        }
        int randomIndex = UnityEngine.Random.Range(0, products.Count);
        if (products.Count > 0)
        {
            return products[randomIndex];
        }
        return null;
    }
}
