using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.AI;
public class SceneData : MonoBehaviour
{
    [SerializeField] GameObject notificationPanel;
    [SerializeField] Text notificationText;
    public List<GameObject> cars;
    [HideInInspector] public GameMode gameMode;
    public Transform buildCam;
    public GameObject driveCam;
    
    public List<Transform> animalPoints;
    public NavMeshSurface navMeshSurface;
    public List<AnimalData> animalsPool;
    public List<GameObject> roadObstaclesPool;
    [HideInInspector] public int roadObstaclesCurrentIndex;

    public List<Transform> allCoinsPositions;
    [HideInInspector] public List<Transform> emptyCoinsPositions;
    public List<GameObject> coinsPool;




    [HideInInspector] public Product[] researchList;
    public AnimationCurve researchCurve;
    [HideInInspector] public float researchSpeed = 1;
    public const float ROAD_Y_OFFSET = 0.08f;
    public const float BUILDCAM_Y_THRESHOLD = 13;


    public void Notification(string notification)
    {
        notificationPanel.SetActive(true);
        notificationText.text = notification;
    }
}


