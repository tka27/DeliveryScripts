using System.Collections.Generic;
using UnityEngine;

public class PathData : MonoBehaviour
{

    [HideInInspector] public List<Transform> wayPoints;
    [HideInInspector] public int currentWaypointIndex;
    [SerializeField] SceneData sceneData;
    public LineRenderer lineRenderer;
    public List<Transform> waypointsPool;
    [HideInInspector] public int currentPoolIndex;
    public List<Transform> finalPoints;
    [HideInInspector] public List<Bridge> allBridges;
    [HideInInspector] public List<Bridge> freeBridges;
    public Transform buildSphere;
    public const int BUILD_SPHERE_RADIUS = 15;

    void Start()
    {
        buildSphere.localScale = new Vector3(BUILD_SPHERE_RADIUS * 2, BUILD_SPHERE_RADIUS * 2, BUILD_SPHERE_RADIUS * 2);
    }
    public void SwitchSphere()
    {
        if (sceneData.gameMode == GameMode.Build)
        {
            buildSphere.gameObject.SetActive(true);
        }
        else
        {
            buildSphere.gameObject.SetActive(false);
        }
    }
}
