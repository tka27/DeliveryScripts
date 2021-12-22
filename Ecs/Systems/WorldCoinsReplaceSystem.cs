using Leopotam.Ecs;
using UnityEngine;


sealed class WorldCoinsReplaceSystem : IEcsRunSystem, IEcsInitSystem
{
    EcsWorld _world;
    SceneData sceneData;
    EcsFilter<WorldCoinsComp, WorldCoinsReplaceRequest> coinsFilter;

    public void Init()
    {
        var worldCoinsEntity = _world.NewEntity();
        worldCoinsEntity.Get<WorldCoinsReplaceRequest>();
        worldCoinsEntity.Get<WorldCoinsComp>();
    }

    void IEcsRunSystem.Run()
    {
        foreach (var entity in coinsFilter)
        {
            coinsFilter.GetEntity(entity).Del<WorldCoinsReplaceRequest>();
            int coinsCount = sceneData.coinsPool.Count;
            sceneData.emptyCoinsPositions.Clear();
            sceneData.emptyCoinsPositions.AddRange(sceneData.allCoinsPositions);
            foreach (var coin in sceneData.coinsPool)
            {
                int random = Random.Range(0, sceneData.emptyCoinsPositions.Count);
                coin.transform.position = sceneData.emptyCoinsPositions[random].position;
                sceneData.emptyCoinsPositions.RemoveAt(random);
                coin.SetActive(true);
            }
        }
    }
}



