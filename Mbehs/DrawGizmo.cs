using UnityEngine;

public class DrawGizmo : MonoBehaviour
{
    [SerializeField] int radius;
    [SerializeField] bool showGizmo;
    void OnDrawGizmos()
    {
        if (showGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
