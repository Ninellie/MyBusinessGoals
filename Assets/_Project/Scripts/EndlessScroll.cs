using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public enum Side
{
    Left,
    Right,
    //Up,
    //Down
}

public class EndlessScroll : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private HorizontalOrVerticalLayoutGroup contentLayoutGroup;
    [SerializeField] private UnityEvent<Side> onShift;

    private RectTransform _content;
    private RectTransform _viewport;
    private float _spacing;
    private bool _dragging;
    
    private void Awake()
    {
        _content = scrollRect.content;
        _viewport = scrollRect.viewport;
        _spacing = contentLayoutGroup.spacing;
    }

    private void OnEnable() => scrollRect.onValueChanged.AddListener(OnScrollMove);

    private void OnDisable() => scrollRect.onValueChanged.RemoveListener(OnScrollMove);

    public void OnBeginDrag(PointerEventData eventData) => _dragging = true;

    public void OnEndDrag(PointerEventData eventData) => _dragging = false;

    private void OnScrollMove(Vector2 delta)
    {
        if (_dragging)
        {
            return;
        }
        // TODO После драга можно вызывать функцию которая вычислит насколько сдвинулся контент от середины и заполнит.
        // Если драг не происходит, то отслеживать отклонение на левый элемент вправо и на правый влево 
        var viewportHalf = _viewport.rect.width / 2;
        var rightContentPos = _content.rect.xMax - viewportHalf; 
        var contentX = _content.anchoredPosition.x;
        
        if (contentX < -rightContentPos)
        {
            ShiftFrom(Side.Left);
            return;
        }

        if (contentX > rightContentPos)
        {
            ShiftFrom(Side.Right);
        }
    } 
    
    private void ShiftFrom(Side side)
    {
        var elementForShift = ShiftContentElementFrom(side);
        var elementWidth = elementForShift!.rect.width;
        var translation = elementWidth + _spacing;
        
        if (side == Side.Left)
        {
            _content.anchoredPosition += new Vector2(translation, 0);
        }
        else
        {
            _content.anchoredPosition -= new Vector2(translation, 0);
        }
        
        onShift.Invoke(side);
    }

    private RectTransform ShiftContentElementFrom(Side side)
    {
        if (side == Side.Left)
        {
            var firstElement = _content.GetChild(0) as RectTransform; 
            firstElement!.SetAsLastSibling();
            return firstElement;
        }
        
        var elementsCount = _content.childCount;
        var lastElement = _content.GetChild(elementsCount - 1) as RectTransform;
        lastElement!.SetAsFirstSibling();
        return lastElement;
    }
}