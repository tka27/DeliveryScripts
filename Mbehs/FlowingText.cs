using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlowingText : MonoBehaviour
{
    [SerializeField] List<Text> texts;
    [SerializeField] List<Animation> animations;
    int currentIndex;


    public void DisplayText(string flowingText)
    {
        if (currentIndex >= texts.Count)
        {
            currentIndex = 0;
        }
        texts[currentIndex].text = flowingText;
        animations[currentIndex].Play();
        currentIndex++;
    }
}
