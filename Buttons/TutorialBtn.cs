using UnityEngine;

public class TutorialBtn : MonoBehaviour
{
    [SerializeField] GameSettings settings;
    [SerializeField] TutorialData tutorialData;
    public void Next()
    {
        settings.tutorialLvl++;
        gameObject.SetActive(false);
    }
}
