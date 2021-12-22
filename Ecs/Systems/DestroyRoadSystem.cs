using Leopotam.Ecs;
using UnityEngine;


sealed class DestroyRoadSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{
    EcsFilter<DestroyRoadRequest> pathRequestFilter;
    EcsFilter<Player> playerFilter;
    EcsWorld _world;
    UIData uiData;
    SceneData sceneData;
    PathData pathData;
    LayerMask layer;
    public void Init()
    {
        layer = LayerMask.GetMask("Ground");
        ClearPathBtn.clickEvent += AddRequest;
    }
    public void Destroy()
    {
        ClearPathBtn.clickEvent -= AddRequest;
    }
    void IEcsRunSystem.Run()
    {
        foreach (var pathEntity in pathRequestFilter)
        {
            Transform playerTR = playerFilter.Get1(0).playerGO.transform;

            RaycastHit hit;
            Physics.Raycast(playerTR.position, -playerTR.up, out hit, 10, layer);
            pathData.buildSphere.position = hit.point;

            pathData.currentWaypointIndex = 0;
            pathData.currentPoolIndex = 0;

            foreach (var wp in pathData.wayPoints)
            {
                wp.gameObject.SetActive(false);
            }
            pathData.wayPoints.Clear();

            pathData.lineRenderer.positionCount = 1;
            pathData.lineRenderer.SetPosition(0, hit.point);
            uiData.isPathComplete = false;
            uiData.isPathConfirmed = false;

            ResetObstacles();
            ResetBridges();
            pathRequestFilter.GetEntity(pathEntity).Destroy();
        }
    }

    void ResetObstacles()
    {
        sceneData.roadObstaclesCurrentIndex = 0;
        foreach (var obstacle in sceneData.roadObstaclesPool)
        {
            obstacle.SetActive(false);
        }
    }

    void ResetBridges()
    {
        pathData.freeBridges.Clear();
        foreach (var bridge in pathData.allBridges)
        {
            pathData.freeBridges.Add(bridge);
        }
    }

    void AddRequest()
    {
        _world.NewEntity().Get<DestroyRoadRequest>();
    }


}
