using UnityEngine;

public class Water : MonoBehaviour
{
    string playerTag = "Player";
    string animalTag = "Animal";

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == playerTag)
        {
            Rigidbody rb = collider.attachedRigidbody;
            if (rb == null) return;

            rb.drag = 7;
            rb.angularDrag = 7;
        }
        else if (collider.tag == animalTag)
        {
            Rigidbody rb = collider.attachedRigidbody;
            if (rb == null) return;

            rb.AddForce(Vector3.up * rb.mass * 55);
            rb.AddForce(-rb.velocity * rb.mass * 40);
            rb.drag = 7;
            rb.angularDrag = 7;
        }
    }
   /* void OnTriggerStay(Collider collider)
    {
        if (collider.tag == playerTag)
        {
            Rigidbody rb = collider.attachedRigidbody;
            rb?.AddForce(Vector3.up * rb.mass * 2.5f);
        }
        else if (collider.tag == animalTag)
        {
            Rigidbody rb = collider.attachedRigidbody;
            rb?.AddForce(Vector3.up * rb.mass * 55);
        }
    }*/
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == playerTag)
        {
            Rigidbody rb = collider.attachedRigidbody;
            rb.drag = 0;
            rb.angularDrag = 0.05f;
        }
    }
}