using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEngine;


sealed class TrailFollowSystem : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
{

    EcsFilter<Player> playerFilter;
    StaticData staticData;
    //List<Transform> activeWeelsTFs = new List<Transform>();
    float trailYOffset;
    public void Init()
    {
        /*foreach (var wc in playerFilter.Get1(0).activeWheelColliders)
        {
            activeWeelsTFs.Add(wc.transform);
        }*/
        CarReturnBtns.returnEvent += ClearTrails;


        ref var player = ref playerFilter.Get1(0);

        float smallestWheelRadius = player.activeWheelColliders[0].radius * player.activeWheelColliders[0].transform.localScale.y;

        foreach (var wc in player.activeWheelColliders)
        {
            if (wc.radius * wc.transform.lossyScale.y < smallestWheelRadius)
            {
                smallestWheelRadius = wc.radius * wc.transform.lossyScale.y;
            }
        }
        trailYOffset = smallestWheelRadius - SceneData.ROAD_Y_OFFSET * 1.1f;

    }


    public void Destroy()
    {
        CarReturnBtns.returnEvent -= ClearTrails;
    }

    void IEcsRunSystem.Run()
    {
        ref var player = ref playerFilter.Get1(0);

        for (int i = 0; i < player.carData.wheelDatas.Count; i++)
        {
            Vector3 wheelPos;
            Quaternion quaternion;
            player.activeWheelColliders[i].GetWorldPose(out wheelPos, out quaternion);
            wheelPos.y -= trailYOffset;

            if (!player.activeWheelColliders[i].isGrounded)
            {
                player.carData.wheelDatas[i].trailTF = null;
                continue;
            }
            else if (!player.carData.wheelDatas[i].trailTF && player.playerRB.velocity.magnitude > .5f)
            {
                player.carData.wheelDatas[i].trailTF = GameObject.Instantiate(staticData.trailPrefab, wheelPos, staticData.trailPrefab.transform.rotation).transform;
            }

            if (player.carData.wheelDatas[i].trailTF)
            {
                player.carData.wheelDatas[i].trailTF.position = wheelPos;
            }
        }
    }

    void ClearTrails()
    {
        ref var player = ref playerFilter.Get1(0);
        for (int i = 0; i < player.carData.wheelDatas.Count; i++)
        {
            player.carData.wheelDatas[i].trailTF = null;
        }
    }
}



