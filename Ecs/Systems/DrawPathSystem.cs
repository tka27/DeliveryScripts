using Leopotam.Ecs;
using UnityEngine;

sealed class DrawPathSystem : IEcsRunSystem, IEcsInitSystem
{
    EcsFilter<Player> playerFilter;
    SceneData sceneData;
    UIData uiData;
    LayerMask layer;
    Camera camera;
    PathData pathData;
    const int PATH_STEP = 1;
    const int BRIDGE_POINT_RADIUS = 15;

    public void Init()
    {
        layer = LayerMask.GetMask("Ground");
        camera = Camera.main;
    }

    void IEcsRunSystem.Run()
    {
        if (uiData.isPathComplete) return;

        RaycastHit hit;
        Ray mouseRay = camera.ScreenPointToRay(Input.mousePosition);
        Vector3 waypointPos;

        if (sceneData.gameMode == GameMode.Build &&
        Input.GetMouseButton(0) &&
        Physics.Raycast(mouseRay, out hit, 1000, layer) &&
        !UIData.IsMouseOverUI())//IsMouseOverButton(uiData.buttons))
        {
            waypointPos = new Vector3(hit.point.x, hit.point.y + SceneData.ROAD_Y_OFFSET, hit.point.z);
            float distanceToNextPoint = 0;

            if (pathData.wayPoints.Count != 0)
            {
                Vector3 waypointPos0 = new Vector3(waypointPos.x, 0, waypointPos.z);
                Vector3 nextPoint0 = new Vector3(pathData.wayPoints[pathData.wayPoints.Count - 1].position.x, 0, pathData.wayPoints[pathData.wayPoints.Count - 1].position.z);

                distanceToNextPoint = (waypointPos0 - nextPoint0).magnitude;
            }
            else
            {

                Transform playerTR = playerFilter.Get1(0).playerGO.transform;
                Physics.Raycast(playerTR.position, -playerTR.up, out hit, 10, layer);

                distanceToNextPoint = (waypointPos - hit.point).magnitude;
            }

            if (distanceToNextPoint >= PATH_STEP && distanceToNextPoint <= PathData.BUILD_SPHERE_RADIUS) //distance btw points
            {
                if (pathData.wayPoints.Count != 0)
                {
                    SetWaypoints(pathData.wayPoints[pathData.wayPoints.Count - 1].position, waypointPos);
                    CheckBridges();
                    if (!uiData.isPathComplete)
                    {
                        CheckPathComplete();
                    }
                }
                else
                {
                    SetWaypoints(hit.point, waypointPos);
                    pathData.lineRenderer.SetPosition(0, pathData.wayPoints[0].position);
                }
            }
        }
    }

    void SetWaypoints(Vector3 first, in Vector3 last)
    {
        int iterations = (int)(last - first).magnitude / PATH_STEP;
        for (int i = 0; i < iterations; i++)
        {
            Vector3 waypointPos = (last - first).normalized * PATH_STEP + first;
            first = waypointPos;
            pathData.wayPoints.Add(WPFromPool(waypointPos));

            pathData.lineRenderer.positionCount++;
            pathData.lineRenderer.SetPosition(pathData.wayPoints.Count, pathData.wayPoints[pathData.wayPoints.Count - 1].position);

            pathData.buildSphere.position = pathData.wayPoints[pathData.wayPoints.Count - 1].position;
        }
    }
    Transform WPFromPool(in Vector3 pos)
    {
        if (pathData.currentPoolIndex >= pathData.waypointsPool.Count)
        {
            pathData.waypointsPool.Add(GameObject.Instantiate(pathData.waypointsPool[0]));
        }
        Transform waypoint = pathData.waypointsPool[pathData.currentPoolIndex];
        waypoint.position = pos;
        waypoint.gameObject.SetActive(true);
        pathData.currentPoolIndex++;
        return waypoint;
    }

    void CheckPathComplete()
    {
        foreach (var finalPoint in pathData.finalPoints)
        {
            float distanceToNextPoint = (finalPoint.position - pathData.wayPoints[pathData.wayPoints.Count - 1].position).magnitude;
            float distanceToCurrentPoint = (finalPoint.position - pathData.wayPoints[0].position).magnitude;
            if (distanceToNextPoint < 5 && distanceToCurrentPoint > 5)
            {
                SetWaypoints(pathData.wayPoints[pathData.wayPoints.Count - 1].position, finalPoint.position);
                uiData.isPathComplete = true;
            }
        }
    }

    void CheckBridges()
    {
        for (int i = 0; i < pathData.freeBridges.Count; i++)
        {
            Bridge bridge = pathData.freeBridges[i];
            float distanceToBridge = (bridge.point1.position - pathData.wayPoints[pathData.wayPoints.Count - 1].position).magnitude;
            if (distanceToBridge < BRIDGE_POINT_RADIUS)
            {
                SetWaypoints(pathData.wayPoints[pathData.wayPoints.Count - 1].position, bridge.point1.position);
                SetWaypoints(pathData.wayPoints[pathData.wayPoints.Count - 1].position, bridge.point2.position);
                pathData.freeBridges.Remove(bridge);
                return;
            }
            distanceToBridge = (bridge.point2.position - pathData.wayPoints[pathData.wayPoints.Count - 1].position).magnitude;
            if (distanceToBridge < BRIDGE_POINT_RADIUS)
            {
                SetWaypoints(pathData.wayPoints[pathData.wayPoints.Count - 1].position, bridge.point2.position);
                SetWaypoints(pathData.wayPoints[pathData.wayPoints.Count - 1].position, bridge.point1.position);
                pathData.freeBridges.Remove(bridge);
                return;
            }
        }
    }
}
