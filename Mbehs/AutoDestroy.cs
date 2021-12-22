using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] GameObject parent;
    Vector3 cameraPos;
    void Start()
    {
        cameraPos = Camera.main.transform.position;
        cameraPos.y = 0;
    }
    void OnBecameInvisible()
    {
        if ((cameraPos - transform.position).magnitude > 150)
        {
            GameObject.Destroy(parent);
        }
    }
}
