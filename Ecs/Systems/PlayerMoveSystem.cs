using Leopotam.Ecs;
using UnityEngine;


sealed class PlayerMoveSystem : IEcsRunSystem, IEcsInitSystem
{
    SceneData sceneData;
    PathData pathData;
    EcsFilter<Player, Movable> playerFilter;
    EcsFilter<WorldCoinsComp>.Exclude<WorldCoinsReplaceRequest> coinsFilter;
    UIData uiData;
    EcsWorld _world;
    Camera camera;

    public void Init()
    {
        camera = Camera.main;
    }

    void IEcsRunSystem.Run()
    {
        foreach (var playerF in playerFilter)
        {
            float steer;
            Vector3 tgtPos;
            ref var player = ref playerFilter.Get1(playerF);


            if (pathData.wayPoints.Count != 0 && sceneData.gameMode == GameMode.Drive)
            {
                tgtPos = pathData.wayPoints[pathData.currentWaypointIndex].transform.position;
                tgtPos.y = player.carData.wheelPos.transform.position.y;
                float distanceToCurrentPoint = (pathData.wayPoints[pathData.currentWaypointIndex].transform.position - player.carData.wheelPos.position).magnitude;


                for (int i = pathData.currentWaypointIndex; i < pathData.wayPoints.Count; i++) //calc nearest point
                {
                    float checkDist = (pathData.wayPoints[i].transform.position - player.carData.wheelPos.position).magnitude;
                    if (checkDist < distanceToCurrentPoint)
                    {
                        pathData.currentWaypointIndex = i;
                    }
                }


                if (distanceToCurrentPoint >= 3f)
                {
                    Vector3 tgtAt0 = new Vector3(tgtPos.x, 0, tgtPos.z);
                    Vector3 playerAt0 = new Vector3(player.carData.wheelPos.position.x, 0, player.carData.wheelPos.position.z);

                    Vector3 yNormal = Vector3.Cross(Vector3.forward - playerAt0, Vector3.right - playerAt0);
                    if (yNormal.y < 0)
                    {
                        yNormal *= -1;
                    }
                    Vector3 projectToXZ = Vector3.ProjectOnPlane(player.carData.wheelPos.forward, yNormal);

                    steer = Vector3.SignedAngle(tgtAt0 - playerAt0, projectToXZ, yNormal);
                    steer *= -1;


                    if (steer > player.maxSteerAngle)
                    {
                        steer = player.maxSteerAngle;
                    }
                    else if (steer < -player.maxSteerAngle)
                    {
                        steer = -player.maxSteerAngle;
                    }




                    if (Input.GetMouseButton(0))//move
                    {
                        if (player.currentTorque < player.maxTorque - player.acceleration)
                        {
                            if (player.currentTorque == 0)
                            {
                                player.currentTorque = player.activeWheelColliders[0].rpm;
                            }

                            player.currentTorque += player.acceleration;
                        }
                        foreach (var wheel in player.activeWheelColliders)
                        {
                            wheel.brakeTorque = 0;
                        }

                    }
                    else    //stop
                    {
                        player.currentTorque = 0;
                        foreach (var brakingWheel in player.carData.brakingWheelColliders)
                        {
                            brakingWheel.brakeTorque = player.playerRB.velocity.magnitude * player.maxTorque / 100;
                        }
                    }
                    foreach (var drivingWheel in player.carData.drivingWheelColliders)
                    {
                        drivingWheel.motorTorque = player.currentTorque;
                    }



                    foreach (var steeringWheel in player.carData.steeringWheelColliders)
                    {
                        steeringWheel.steerAngle = steer;
                    }
                }
                else
                {
                    if (pathData.currentWaypointIndex < pathData.wayPoints.Count - 1)
                    {
                        pathData.currentWaypointIndex++;
                    }
                    else // reach final point
                    {
                        _world.NewEntity().Get<DestroyRoadRequest>();
                        coinsFilter.GetEntity(0).Get<WorldCoinsReplaceRequest>();
                        Vector3 pos = new Vector3(player.playerGO.transform.position.x, 20, player.playerGO.transform.position.z);
                        sceneData.buildCam.position = pos;
                        sceneData.gameMode = GameMode.View;
                        UIData.UpdateUI();
                    }
                }
            }
            else
            {
                //stop method
                player.currentTorque = 0;
                for (int i = 0; i < player.carData.drivingWheelColliders.Count; i++)
                {
                    player.carData.drivingWheelColliders[i].motorTorque = 0;
                    player.carData.drivingWheelColliders[i].brakeTorque = player.playerRB.velocity.magnitude * player.maxTorque;
                }
            }
        }
    }
}
