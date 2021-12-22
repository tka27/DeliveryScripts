using UnityEngine;

public static class Extensions
{
    public static void SetTransform(this RectTransform tf, RectTransform tgt)
    {
        tf.anchoredPosition = tgt.anchoredPosition;
        tf.anchorMin = tgt.anchorMin;
        tf.anchorMax = tgt.anchorMax;
        tf.localScale = tgt.localScale;
    }
}

