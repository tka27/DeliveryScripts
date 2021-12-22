using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu]
public class StaticData : ScriptableObject
{
    public List<ProductType> availableProductTypes;
    public GameObject trailPrefab;
    public GameObject tutorialPrefab;
    public GameObject deerMaleRD;
    public GameObject deerFemRD;
    public float currentMoney;
    public float moneyForGame = 100;
    public int selectedCarID;
    public int selectedMapID;
    public bool trailerIsSelected;
    public List<CarData> allCars;
    [HideInInspector] public int adProgress;








    //for save data
    public float totalMoney;
    public int researchLvl;
    public bool[] carsUnlockStatus;
    public bool[] carsBuyStatus;
    public bool[] trailersBuyStatus;
    public int[][] carPerks;    //[carID[perkID]] = lvl     | 0 - fuel | 1 - speed | 2 - acceleration | 3 - suspension | 4 - storage |
    public int[][] mapPerks;    //[mapID[perkID]] = lvl   


    public void UpdateStaticData(SaveData data)
    {
        this.totalMoney = data.totalMoney;
        this.researchLvl = data.researchLvl;

        for (int i = 0; i < this.carsBuyStatus.Length; i++)
        {
            this.carsBuyStatus[i] = data.carsBuyStatus[i];
        }

        for (int i = 0; i < this.trailersBuyStatus.Length; i++)
        {
            this.trailersBuyStatus[i] = data.trailersBuyStatus[i];
        }

        for (int i = 0; i < this.carsUnlockStatus.Length; i++)
        {
            this.carsUnlockStatus[i] = data.carsUnlockStatus[i];
        }

        for (int i = 0; i < this.carPerks.Length; i++)
        {
            this.carPerks[i] = data.carPerks[i];
        }

        for (int i = 0; i < this.mapPerks.Length; i++)
        {
            this.mapPerks[i] = data.mapPerks[i];
        }
    }

    public void SetDefaultData()
    {
        this.totalMoney = 1000;
        this.researchLvl = 0;

        int carsCount = this.allCars.Count;
        int mapsCount = 1;

        this.carsUnlockStatus = new bool[carsCount];
        this.carsUnlockStatus[0] = true;
        this.carsBuyStatus = new bool[carsCount];
        this.carsBuyStatus[0] = true;
        this.trailersBuyStatus = new bool[carsCount];

        this.carPerks = new int[carsCount][];
        for (int i = 0; i < carsCount; i++)
        {
            this.carPerks[i] = new int[5];
        }

        this.mapPerks = new int[mapsCount][];
        for (int i = 0; i < mapsCount; i++)
        {
            this.mapPerks[i] = new int[5];
        }
    }
    public void UpdateAvailableProducts()
    {
        availableProductTypes.Clear();
        availableProductTypes.AddRange(allCars[selectedCarID].carProductTypes);
        if (trailerIsSelected)
        {
            availableProductTypes.AddRange(allCars[selectedCarID].trailerProductTypes);
        }
    }
}

