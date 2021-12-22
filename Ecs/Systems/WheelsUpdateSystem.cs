using Leopotam.Ecs;
using UnityEngine;

sealed class WheelsUpdateSystem : IEcsRunSystem
{
    EcsFilter<Player> playerFilter;
    void IEcsRunSystem.Run()
    {
        foreach (var fPlayer in playerFilter)
        {
            ref var player = ref playerFilter.Get1(fPlayer);
            for (int i = 0; i < player.carData.allWheelColliders.Count; i++)
            {
                Vector3 pos;
                Quaternion quaternion;
                player.carData.allWheelColliders[i].GetWorldPose(out pos, out quaternion);
                player.carData.allWheelMeshes[i].transform.position = pos;
                player.carData.allWheelMeshes[i].transform.rotation = quaternion;
            }
        }
    }
}
