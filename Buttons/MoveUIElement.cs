using UnityEngine;

public class MoveUIElement : MonoBehaviour
{
    [SerializeField] Transform tgtPos;
    [SerializeField] Transform elementTF;
    public void MoveElement()
    {
        elementTF.position = tgtPos.position;
    }
}
