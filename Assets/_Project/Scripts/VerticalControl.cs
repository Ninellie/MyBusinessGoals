using UnityEngine;

public class VerticalControl : MonoBehaviour
{
    public RectTransform rectTransform;

    public void SetHeight(float height)
    {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
        rectTransform.ForceUpdateRectTransforms();
    }
}