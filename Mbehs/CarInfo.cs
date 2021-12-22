using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarInfo : MonoBehaviour
{
    [SerializeField] List<Image> productIcons;
    [SerializeField] StaticData staticData;
    [SerializeField] ProductData productData;
    [SerializeField] MainMenuSceneData mainMenuSceneData;
    [SerializeField] GameObject mapBtn;
    [SerializeField] GameObject buyCarBtn;
    [SerializeField] GameObject upgradeBtn;
    [SerializeField] GameObject price;
    [SerializeField] Text priceText;
    [SerializeField] GameObject padlock;
    [SerializeField] Image trailerBackground;
    [SerializeField] Text mass;
    [SerializeField] Text storage;


    public void InfoUpdate()
    {
        int carID = staticData.selectedCarID;
        mainMenuSceneData.cars[carID].gameObject.SetActive(true);

        staticData.UpdateAvailableProducts();
        UpdateIcons(carID);

        if (!staticData.trailerIsSelected)
        {
            mass.text = "Mass: " + staticData.allCars[carID].defaultMass;
            storage.text = "Storage: " + staticData.allCars[carID].carStorage;
        }
        else
        {
            mass.text = "Mass: " + (staticData.allCars[carID].defaultMass + staticData.allCars[carID].trailer.GetComponent<Rigidbody>().mass);
            storage.text = "Storage: " + (staticData.allCars[carID].carStorage + staticData.allCars[carID].trailerStorage);
        }

        //car status check
        if (staticData.carsUnlockStatus[carID] && staticData.carsBuyStatus[carID])
        {
            upgradeBtn.SetActive(true);
            buyCarBtn.SetActive(false);
            mapBtn.SetActive(true);
            padlock.SetActive(false);
            price.SetActive(false);
        }
        else if (staticData.carsUnlockStatus[carID] && !staticData.carsBuyStatus[carID])
        {
            upgradeBtn.SetActive(false);
            mapBtn.SetActive(false);
            buyCarBtn.SetActive(true);
            padlock.SetActive(true);
            price.SetActive(true);
            priceText.text = staticData.allCars[carID].price.ToString();
        }
        else if (!staticData.carsUnlockStatus[carID])
        {
            upgradeBtn.SetActive(false);
            mapBtn.SetActive(false);
            buyCarBtn.SetActive(false);
            padlock.SetActive(true);
            price.SetActive(false);
        }

        if (staticData.trailerIsSelected)
        {
            trailerBackground.color = new Color(.7f, 1, .45f, 1);
            mainMenuSceneData.trailers[carID].SetActive(true);
            if (!staticData.trailersBuyStatus[carID])
            {
                mapBtn.SetActive(false);
                if (staticData.carsBuyStatus[carID])
                {
                    buyCarBtn.SetActive(true);
                    price.SetActive(true);
                    priceText.text = (staticData.allCars[carID].price / 2).ToString();
                }
            }
        }
        else
        {
            trailerBackground.color = new Color(1, .55f, .35f, 1);
            mainMenuSceneData.trailers[carID].SetActive(false);
        }
    }


    void UpdateIcons(int carID)
    {
        foreach (var icon in productIcons)
        {
            icon.gameObject.SetActive(false);
        }
        if (staticData.carsUnlockStatus[carID])
        {
            for (int i = 0; i < staticData.availableProductTypes.Count; i++)
            {
                productIcons[i].gameObject.SetActive(true);
                productIcons[i].sprite = productData.FindProductIcon(staticData.availableProductTypes[i]);
            }
        }
        else
        {
            for (int i = 0; i < staticData.availableProductTypes.Count; i++)
            {
                productIcons[i].gameObject.SetActive(true);
                productIcons[i].sprite = productData.question;
            }
        }
    }
}