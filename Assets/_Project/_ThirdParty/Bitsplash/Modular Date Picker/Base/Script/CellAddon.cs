using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Bitsplash.DatePicker
{
    public class CellAddon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,IBeginDragHandler,IEndDragHandler,IDragHandler,IPointerDownHandler
    {
        DatePickerContent mParent;
        int mChildIndex;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (mParent != null)
                ((IDatePickerPrivate)mParent).RaiseStartSelection(mChildIndex);
        }

        public void OnDrag(PointerEventData eventData)
        {
         
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (mParent != null)
                ((IDatePickerPrivate)mParent).EndSelection();
        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (mParent != null)
                ((IDatePickerPrivate)mParent).RaiseClick(mChildIndex);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                var cellAddon = eventData.pointerDrag.GetComponent<CellAddon>();
                if (cellAddon != null && mParent != null)
                    ((IDatePickerPrivate)mParent).RaiseSelectionEnter(mChildIndex, cellAddon.mChildIndex);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                var cellAddon = eventData.pointerDrag.GetComponent<CellAddon>();
                if (cellAddon != null && mParent != null)
                    ((IDatePickerPrivate)mParent).RaiseSelectionExit(mChildIndex, cellAddon.mChildIndex);
            }
        }

        public void SetParent(DatePickerContent parent,int childIndex)
        {
            mParent = parent;
            mChildIndex = childIndex;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}