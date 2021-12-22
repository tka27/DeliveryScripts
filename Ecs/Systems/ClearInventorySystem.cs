using Leopotam.Ecs;
using UnityEngine;

sealed class ClearInventorySystem : IEcsInitSystem, IEcsDestroySystem
{
    EcsFilter<Player, Inventory>.Exclude<UpdateCargoRequest> playerFilter;
    UIData uiData;

    

    public void Init()
    {
        ClearInventoryBtn.clickEvent += ClearAction;
    }
    
    
    public void Destroy()
    {
        ClearInventoryBtn.clickEvent -= ClearAction;
    }


    void ClearAction()
    {
        var player = playerFilter.Get1(0);
        var cargo = playerFilter.Get2(0);
        if (cargo.GetCurrentMass() == 0) return;
        player.playerRB.AddRelativeForce(new Vector3(0, -100000, 100000));
        for (int i = 0; i < player.carData.playerCargo.Count; i++)
        {
            player.carData.playerCargoRB[i].isKinematic = false;
            player.carData.playerCargoRB[i].AddExplosionForce(Random.Range(2000, 3000), player.carData.wheelPos.transform.position, 0);
        }
        cargo.inventory.Clear();
        playerFilter.GetEntity(0).Get<UpdateCargoRequest>();
    }

}