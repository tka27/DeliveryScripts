using UnityEngine;

public class Product
{
    public ProductType type { get; }
    public float mass { get; set; }
    public Sprite icon { get; }
    public float defaultPrice { get; }
    public float currentPrice { get; set; }

    public Product(ProductType type, Sprite icon, float defaultPrice)
    {
        this.type = type;
        mass = 0;
        this.icon = icon;
        this.defaultPrice = defaultPrice;
        currentPrice = this.defaultPrice;
    }
    public Product(ProductType type, float mass, Sprite icon, float defaultPrice) : this(type, icon, defaultPrice)
    {
        this.mass = mass;
    }
}
