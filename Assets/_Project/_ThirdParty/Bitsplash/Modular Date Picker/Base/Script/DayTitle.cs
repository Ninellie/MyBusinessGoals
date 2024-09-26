using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Bitsplash.DatePicker
{
    /// <summary>
    /// shows the week day names on top the date picker
    /// </summary>
    [ExecuteInEditMode]
    public class DayTitle : DatePickerElement , IDatePickerSettingsItem
    {
        public string EditorTitle { get { return "Day Title"; } }

        public DatePickerCell CellPrefab;
        public string Format = "ddd";
        DatePickerContent mContent;
        bool mInvalid = true;

        public int Order { get { return 8; } }

        void GenerateCells()
        {
            Clear();
            if (CellPrefab == null || mContent == null)
                return;

            float ColumnSize = 1f / 7f;
            DateTime baseDate = DateTime.Today.Date;
            int monthDayOfWeek = (int)baseDate.DayOfWeek;
            int span = monthDayOfWeek - (int)mContent.FirstDayOfWeek;
            if (span < 0)
                span += 7;
            DateTime startFrom = (baseDate - TimeSpan.FromDays(span)).Date;
            for (int i = 0; i < 7; i++)
            {
                DateTime current = startFrom.Add(TimeSpan.FromDays(i)).Date;
                float startX = ((float)i) / 7f;
                if (mContent.RightToLeft)
                    startX = 1f - startX - ColumnSize;
                float endX = startX + ColumnSize;

                GameObject newObj = GameObject.Instantiate(CellPrefab.gameObject, transform);
                CommonMethods.SafeDestroy(newObj.GetComponent<DatePickerCellTemplate>());
                CommonMethods.HideObject(newObj);
                newObj.name = String.Format("day_{0}", i);
                newObj.SetActive(true);
                CommonMethods.EnsureComponent<IDateTimeItem>(newObj);
                var rect = newObj.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(startX, 0f);
                rect.anchorMax = new Vector2(endX, 1f);
                rect.anchoredPosition = new Vector2(0f, 0f);
                rect.sizeDelta = new Vector2(0f, 0f);
                var cell = newObj.GetComponent<DatePickerCell>();
                cell.SetInitialSettings(true, false);
                cell.DayValue = current;
                try
                {
                    cell.SetText(current.ToString(Format));
                }
                catch(Exception)
                {
                    Debug.LogWarning("invalid format in day title");
                }                 
            }
        }
        public void Clear()
        {
            IDateTimeItem[] children = GetComponentsInChildren<IDateTimeItem>();
            for (int i = 0; i < children.Length; ++i)
            {
                if (children[i] != null)
                {
                    if (children[i].gameObject.GetComponentInParent<DayTitle>() != this)
                        continue;
                    if (children[i].gameObject != gameObject)
                        CommonMethods.SafeDestroy(children[i].gameObject);
                }
            }
        }
        public void Invalidate()
        {
            mInvalid = true;
        }
        public override void OnValidate()
        {
            base.OnValidate();
            Invalidate();
        }
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            Invalidate();
        }
        protected override void OnSettingsChanged()
        {
            base.OnSettingsChanged();
            Invalidate();
        }
        // Update is called once per frame
        void Update()
        {
            if(mInvalid == true)
            {
                mInvalid = false;
                GenerateCells();
            }
        }

        protected override void SetContent(DatePickerContent content)
        {
            mContent = content;
            Invalidate();
        }

        protected override void SetMain(DatePickerSettings main)
        {

        }
    }
}
