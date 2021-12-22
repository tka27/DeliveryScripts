using UnityEngine;

[CreateAssetMenu]
public class ProductData : ScriptableObject
{
    public Sprite question;
    public Sprite wheat;
    public Sprite water;
    public Sprite bread;
    public Sprite fuel;
    public Sprite autoParts;
    public Sprite meat;
    public Sprite milk;
    public Sprite pizza;
    public Sprite cheese;
    public Sprite eggs;
    public Sprite fish;
    public Sprite fruits;
    public Sprite cannedFish;
    public Sprite juice;
    public Sprite vegetables;
    public Sprite iceCream;

    public Sprite FindProductIcon(ProductType type)
    {
        switch (type)
        {
            case ProductType.Wheat:
                return wheat;

            case ProductType.Bread:
                return bread;

            case ProductType.Cheese:
                return cheese;

            case ProductType.Eggs:
                return eggs;

            case ProductType.Meat:
                return meat;

            case ProductType.Milk:
                return milk;

            case ProductType.Pizza:
                return pizza;

            case ProductType.Water:
                return water;

            case ProductType.Fish:
                return fish;

            case ProductType.Fruits:
                return fruits;

            case ProductType.CannedFish:
                return cannedFish;

            case ProductType.Juice:
                return juice;

            case ProductType.Vegetables:
                return vegetables;

            case ProductType.IceCream:
                return iceCream;

            default:
                Debug.LogError("Sprite not found");
                return null;
        }
    }
}