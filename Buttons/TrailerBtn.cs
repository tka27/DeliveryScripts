using UnityEngine;

public class TrailerBtn : MonoBehaviour
{
    [SerializeField] StaticData staticData;
    [SerializeField] CarInfo carInfo;

    public void TrailerSwitch()
    {
        SoundData.PlayBtn();
        staticData.trailerIsSelected = !staticData.trailerIsSelected;
        carInfo.InfoUpdate();
    }
}
