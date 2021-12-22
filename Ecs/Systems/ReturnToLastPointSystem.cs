using Leopotam.Ecs;
using UnityEngine;


sealed class ReturnToLastPointSystem : IEcsInitSystem, IEcsDestroySystem
{
    EcsWorld _world;
    SceneData sceneData;
    StaticData staticData;
    EcsFilter<Player> playerFilter;
    EcsFilter<WorldCoinsComp> coinsFilter;

    

    public void Init()
    {
        CarReturnBtns.returnEvent += ReturnToTP;
    }
    
    
    public void Destroy()
    {
        CarReturnBtns.returnEvent -= ReturnToTP;
    }

    void ReturnToTP()
    {
        staticData.adProgress += 50;

        ref var player = ref playerFilter.Get1(0);
        player.currentDurability -= player.maxDurability / 10;
        sceneData.cars[staticData.selectedCarID].transform.position = BuildingsData.lastVisit.position;

        _world.NewEntity().Get<DestroyRoadRequest>();
        coinsFilter.GetEntity(0).Get<WorldCoinsReplaceRequest>();
        Vector3 pos = new Vector3(player.playerGO.transform.position.x, 20, player.playerGO.transform.position.z);
        sceneData.buildCam.position = pos;
        sceneData.gameMode = GameMode.View;
        UIData.UpdateUI();
    }
}



