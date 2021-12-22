using System.Collections.Generic;
using UnityEngine;

public class CarData : MonoBehaviour
{
    public float price;
    public GameObject trailer;
    public Rigidbody trailerRB;
    public List<ProductType> carProductTypes;
    public List<ProductType> trailerProductTypes;
    public List<Transform> allWheelMeshes;
    public List<WheelCollider> allWheelColliders;
    public List<WheelCollider> steeringWheelColliders;
    public List<WheelCollider> drivingWheelColliders;
    public List<WheelCollider> brakingWheelColliders;
    public Transform wheelPos;
    public Transform centerOfMass;
    public Transform cameraLookPoint;
    [HideInInspector] public List<WheelData> wheelDatas;
    public List<GameObject> playerCargo;
    public List<Rigidbody> playerCargoRB;
    [HideInInspector] public List<Vector3> playerCargoDefaultPos;
    [HideInInspector] public List<Quaternion> playerCargoDefaultRot;
    public AudioSource engineSound;
    public float enginePitchDefault;

    public float maxSteerAngle;
    public float maxTorque;
    public float maxDurability;
    public float acceleration;
    public float maxFuel;
    public float defaultMass;
    public float carStorage;
    public float trailerStorage;

}
