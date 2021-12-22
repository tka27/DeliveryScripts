using Leopotam.Ecs;


sealed class CratesDisplaySystem : IEcsRunSystem
{

    EcsFilter<Player, Inventory, CratesDisplayRequest> playerFilter;

    void IEcsRunSystem.Run()
    {
        foreach (var fPlayer in playerFilter)
        {
            ref var player = ref playerFilter.Get1(fPlayer);
            ref var playerInventory = ref playerFilter.Get2(fPlayer);
            float filledPart = playerInventory.GetCurrentMass() / playerInventory.maxMass;

            for (int i = 0; i < player.carData.playerCargo.Count * filledPart; i++)
            {
                player.carData.playerCargo[i].SetActive(true);
                player.carData.playerCargoRB[i].isKinematic = true;
                player.carData.playerCargo[i].transform.localPosition = player.carData.playerCargoDefaultPos[i];
                player.carData.playerCargo[i].transform.localRotation = player.carData.playerCargoDefaultRot[i];
            }
            playerFilter.GetEntity(fPlayer).Del<CratesDisplayRequest>();
        }
    }
}
