using System.Collections.Generic;
using UnityEngine;



public struct Player
{
    public Rigidbody playerRB;
    public GameObject playerGO;
    public CarData carData;
    public float maxSteerAngle;
    public float maxTorque;
    public float currentTorque;
    public float acceleration;
    public float maxDurability;
    public float currentDurability;
    public float maxFuel;
    public float currentFuel;
    public float fuelConsumption;
    public List<WheelCollider> activeWheelColliders;
}
public struct Movable { }
public struct DestroyRoadRequest { }
public struct ImmobilizeRequest { }
public struct ProductBuyer
{
    public GameObject buyerGO;
    public TradePointData tradePointData;
    public List<ProductType> buyingProductTypes;
    public float repriceMultiplier;

}
public struct ProductSeller
{
    public GameObject sellerGO;
    public TradePointData tradePointData;
    public Product product;
    public float produceSpeed;
    public float repriceMultiplier;
    public float productionTimer;

}
public struct Inventory
{
    public List<Product> inventory;
    public float maxMass;
    public float GetCurrentMass()
    {
        float currentMass = 0;
        foreach (var product in inventory)
        {
            currentMass += product.mass;
        }
        return currentMass;
    }
    public void RemoveEmptySlots()// only for player
    {
        List<int> indexesForRemove = new List<int>();
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].mass <= 0)
            {
                indexesForRemove.Add(i);
            }
        }
        for (int i = indexesForRemove.Count - 1; i >= 0; i--)
        {
            inventory.RemoveAt(indexesForRemove[i]);
        }
    }

    public bool HasItem(ProductType type, float minCount)
    {
        foreach (var item in inventory)
        {
            if (item.type == type && item.mass >= minCount)
            {
                return true;
            }
        }
        return false;
    }
}

public struct AutoService { }
public struct SellDataUpdateRequest { }
public struct BuyDataUpdateRequest { }
public struct Shop
{
    public float currentQuestTime;
    public float maxQuestTime;
    public float timer;
}
public struct UpdateCargoRequest { }
public struct CratesDisplayRequest { }
public struct ResearchLab
{
    public float progress;
    public const float DEFAULT_REQUIREMENT = 200;
    public float requirement;
}
public struct LabUpdateRequest { }
public struct WorldCoinsReplaceRequest { }
public struct WorldCoinsComp { }
public struct Animal
{
    public AnimalData animalData;
}

public struct OverflowCheckRequest
{
    public float timer;
}



