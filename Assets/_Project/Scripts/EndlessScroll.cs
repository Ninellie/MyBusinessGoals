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
    [SerializeField] public UnityEvent<GameObject> onShift;

    private RectTransform _content;
    private float _spacing;
    private bool _dragging;
    private float _contentPosX;
    private RectTransform _firstChild;
    private RectTransform _lastChild;
    private float _rightBorder;
    private float _leftBorder;
    
    private void Awake()
    {
        _content = scrollRect.content;
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
        UpdateData();
        
        if (_leftBorder - _rightBorder == 0)
        {
            return;
        }
        
        while (_contentPosX < _leftBorder)
        {
            ShiftFrom(Side.Left);
            UpdateData();
        }

        while (_contentPosX > _rightBorder)
        {
            ShiftFrom(Side.Right);
            UpdateData();
        }
    }

    private void UpdateData()
    {
        _contentPosX = _content.anchoredPosition.x; 
        _firstChild = _content.GetChild(0) as RectTransform;
        var elementsCount = _content.childCount;
        _lastChild = _content.GetChild(elementsCount - 1) as RectTransform;
        if (_firstChild == null || _lastChild == null)
        {
            Debug.LogWarning("Content has no child with RectTransform component");
            return;
        }
        _rightBorder = _firstChild.rect.width;
        _leftBorder = -_lastChild.rect.width;
    }
    
    private void ShiftFrom(Side side)
    {
        var shiftedElement = ShiftContentElementFrom(side);
        var elementWidth = shiftedElement!.rect.width;
        var translation = elementWidth + _spacing;
        
        if (side == Side.Left)
        {
            _content.anchoredPosition += new Vector2(translation, 0);
        }
        else
        {
            _content.anchoredPosition -= new Vector2(translation, 0);
        }
        
        onShift.Invoke(shiftedElement.gameObject);
    }

    private RectTransform ShiftContentElementFrom(Side side)
    {
        if (side == Side.Left)
        {
            _firstChild!.SetAsLastSibling();
            return _firstChild;
        }
        _lastChild!.SetAsFirstSibling();
        return _lastChild;
    }
}