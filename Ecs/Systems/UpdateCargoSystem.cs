using Leopotam.Ecs;
using UnityEngine;


sealed class UpdateCargoSystem : IEcsRunSystem
{

    EcsFilter<Inventory, Player, UpdateCargoRequest> playerFilter;
    StaticData staticData;
    UIData uiData;
    void IEcsRunSystem.Run()
    {
        foreach (var fPlayer in playerFilter)
        {
            ref var playerInventory = ref playerFilter.Get1(fPlayer);
            ref var player = ref playerFilter.Get2(fPlayer);
            player.playerRB.mass = player.carData.defaultMass + playerInventory.GetCurrentMass();
            uiData.cargoText.text = playerInventory.GetCurrentMass().ToString("0") + "/" + playerInventory.maxMass.ToString("0");
            uiData.moneyText.text = staticData.currentMoney.ToString("0.0");

            for (int i = 0; i < uiData.inventoryIcons.Count; i++)
            {
                uiData.inventoryIcons[i].color = Color.clear;
                uiData.inventoryText[i].text = "";
            }
            for (int i = 0; i < playerInventory.inventory.Count; i++)
            {
                uiData.inventoryIcons[i].color = Color.white;
                uiData.inventoryIcons[i].sprite = playerInventory.inventory[i].icon;
                uiData.inventoryText[i].text = playerInventory.inventory[i].mass.ToString("0.0");
            }

            playerFilter.GetEntity(fPlayer).Del<UpdateCargoRequest>();
            playerFilter.GetEntity(fPlayer).Get<CratesDisplayRequest>();
        }
    }
}
