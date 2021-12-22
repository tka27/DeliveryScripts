using System.Collections;
using UnityEngine;

public class WheelData : MonoBehaviour
{
    [SerializeField] WheelCollider attachedWheelColider;
    WheelFrictionCurve defaultForvardFriction;
    WheelFrictionCurve defaultSidewaysFriction;


    public bool onRoad;
    public bool inWater;
    public ParticleSystem particles;
    public Transform trailTF;

    float debuffTime = 2;
    bool firstCheck;
    string buildingTag = "Building";
    string roadTag = "Road";
    string waterTag = "Water";
    string obstacle = "Obstacle";
    string timerCor = "DebuffTimer";


    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        defaultForvardFriction = attachedWheelColider.forwardFriction;
        defaultSidewaysFriction = attachedWheelColider.sidewaysFriction;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == obstacle)
        {
            Debuff();
        }
        else if (collider.tag == waterTag)
        {
            inWater = true;
        }
    }
    void OnTriggerStay(Collider collider)
    {
        if (collider.tag == roadTag || collider.tag == buildingTag)
        {
            onRoad = true;
            firstCheck = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == roadTag)
        {
            firstCheck = false;
            StartCoroutine(OnRoadCheck());
        }
        if (collider.tag == waterTag)
        {
            inWater = false;
        }
    }

    IEnumerator OnRoadCheck()
    {

        yield return new WaitForSeconds(0.02f);
        if (!firstCheck)
        {
            onRoad = false;
        }
    }

    IEnumerator DebuffTimer()
    {
        yield return new WaitForSeconds(debuffTime);

        attachedWheelColider.forwardFriction = defaultForvardFriction;
        attachedWheelColider.sidewaysFriction = defaultSidewaysFriction;

    }

    void Debuff()
    {
        StopCoroutine(timerCor);
        WheelFrictionCurve forvardFriction = defaultForvardFriction;
        forvardFriction.stiffness = defaultForvardFriction.stiffness / 5;
        attachedWheelColider.forwardFriction = forvardFriction;

        WheelFrictionCurve sidewaysFriction = defaultSidewaysFriction;
        sidewaysFriction.stiffness = defaultSidewaysFriction.stiffness / 5;
        attachedWheelColider.sidewaysFriction = sidewaysFriction;
        StartCoroutine(timerCor);
    }
}
