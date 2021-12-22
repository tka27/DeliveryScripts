using UnityEngine;

public class Bridge : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    [SerializeField] PathData pathData;

    void Start()
    {
        pathData.allBridges.Add(this);
        pathData.freeBridges.Add(this);
    }
}
