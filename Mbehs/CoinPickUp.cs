using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    [SerializeField] FlowingText flowingText;
    [SerializeField] StaticData staticData;
    [SerializeField] SceneData sceneData;
    [SerializeField] UIData uiData;
    [SerializeField] ParticleSystem particles;
    string playerTag = "Player";
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == playerTag)
        {
            SoundData.PlayCoin();
            particles.transform.position = transform.position;
            particles.Play();
            gameObject.SetActive(false);
            float revardValue = 1 + 1 * sceneData.researchCurve.Evaluate(staticData.researchLvl);
            staticData.currentMoney += revardValue;
            flowingText.DisplayText("+" + (revardValue).ToString("0.0"));
            uiData.moneyText.text = staticData.currentMoney.ToString("0.0");
        }
    }
}
