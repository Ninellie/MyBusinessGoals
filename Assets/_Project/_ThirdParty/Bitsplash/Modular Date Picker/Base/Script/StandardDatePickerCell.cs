using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Bitsplash.DatePicker
{
    [ExecuteInEditMode]
    public class StandardDatePickerCell : DatePickerCell
    {
        public DatePickerText TextItem;
        public Image Mark;
        public Image Background;

        public int TextSize = 14;
        public FontStyle FontStyle;

        public Color EnabledTextColor;
        public Color SelectedTextColor;
        public Color DisabledTextColor;

        public Sprite MarkSprite;
        [NonSerialized]
        public Color MarkSelectedColor = new Color(0f,0f,0f,0f);

        public Color SelectedBackgroundColor;
        // public Color HoverBackgroundColor;
        public Color EnabledBackgroundColor;
        public Color DisabledBackgroundColor;

        public float SlideSpeed = 0.9f;

        bool mSelected = false;
        bool mEnabled = true;
        DateTime mDayValue = new DateTime();
        private IEnumerator mCoroutine;

        Color TargetColor;

        public override Color MarkerColor 
        {
            get
            {
                return MarkSelectedColor;
            }
            set
            {
                MarkSelectedColor = value;
                AssignTextAndBackground();
            }
        }
        public override bool CellSelected
        {
            get
            {
                return mSelected;
            }
            set
            {
                mSelected = value;
                AssignTextAndBackground();
            }
        }

        public override bool CellEnabled
        {
            get
            {
                return mEnabled;
            }
            set
            {
                mEnabled = value;
                AssignTextAndBackground();
            }
        }

        public override DateTime DayValue
        {
            get
            {
                return mDayValue;
            }
            set
            {
                mDayValue = value;

            }
        }

        Vector4 ColorToVector4(Color c)
        {
            return new Vector4(c.r, c.g, c.b, c.a);
        }
        bool CompareColor(Color a, Color b, float margin)
        {
            Vector4 va = new Vector4(a.r, a.g, a.b, a.a);
            Vector4 vb = new Vector4(b.r, b.g, b.b, b.a);
            return (va - vb).sqrMagnitude < margin * margin;
        }
        void SlideColor(Color color)
        {
            if (Background == null)
                return;
            if (mCoroutine != null)
            {
                StopCoroutine(mCoroutine);
                mCoroutine = null;
            }
            if (CompareColor(Background.color, color, 0.01f))
                return;
            if (isActiveAndEnabled == false || SlideSpeed <0f)
            {

                Background.color = color;
                return;
            }
            mCoroutine = SlideToColor(color, SlideSpeed);
            StartCoroutine(mCoroutine);
        }
        private void OnDisable()
        {
            if (mCoroutine != null)
            {
                StopCoroutine(mCoroutine);
                if (Background != null)
                    Background.color = TargetColor;
                mCoroutine = null;
            }
        }
        IEnumerator SlideToColor(Color color, float factor)
        {
            TargetColor = color;
            Vector4 from = ColorToVector4(Background.color);
            Vector4 to = ColorToVector4(color);
            Vector4 move = (to - from);
            float time = 0f;
            float magnitude = move.magnitude;
            Color start = Background.color;
            while (CompareColor(Background.color, color, 0.01f) == false)
            {
                Background.color = Color.Lerp(start, color, (time * factor) / magnitude);
                time += Time.deltaTime;
                yield return 0;
            }

        }
        public override void SetInitialSettings(bool enabled, bool selected)
        {
            mEnabled = enabled;
            mSelected = selected;
            AssignTextAndBackground(true);
        }
        void AssignTextAndBackground(bool dontSlide = false)
        {
            var color = SelectedTextColor;
            var backColor = SelectedBackgroundColor;

            if(mSelected == false)
            {
                if(mEnabled)
                {
                    color = EnabledTextColor;
                    backColor = EnabledBackgroundColor;
                }
                else
                {
                    color = DisabledTextColor;
                    backColor = DisabledBackgroundColor;
                }
                

            }

            if (Mark != null)
                Mark.color = MarkSelectedColor;
            
            if (Mark != null)
                Mark.sprite = MarkSprite;
            if (TextItem != null)
            {
                TextItem.Color = color;
                TextItem.TextSize = TextSize;
                TextItem.FontStyle = FontStyle;

            }
            if(dontSlide)
            {
                if (Background != null)
                    Background.color = backColor;
            }
            else
                SlideColor(backColor);
//            if (Background != null)
//                Background.color = backColor;
        }
        void CopyFrom(StandardDatePickerCell cell)
        {
            TextSize = cell.TextSize;
            FontStyle = cell.FontStyle;

            EnabledTextColor = cell.EnabledTextColor;
            SelectedTextColor = cell.SelectedTextColor;
            DisabledTextColor = cell.DisabledTextColor;

            MarkSprite = cell.MarkSprite;
            MarkSelectedColor = cell.MarkSelectedColor;

            SelectedBackgroundColor = cell.SelectedBackgroundColor;
            // public Color HoverBackgroundColor;
            EnabledBackgroundColor = cell.EnabledBackgroundColor;
            DisabledBackgroundColor = cell.DisabledBackgroundColor;

            SlideSpeed = cell.SlideSpeed;
        }
        public void OnValidate()
        {
            if (GetComponent<DatePickerCellTemplate>() == null)
            {
                AssignTextAndBackground(true);
                return;
            }
            var settings = GetComponentInParent<DatePickerSettings>();
            if(settings != null)
            {
                foreach(var cell in settings.GetComponentsInChildren<StandardDatePickerCell>())
                {
                    if (cell != this)
                    {
                        cell.CopyFrom(this);
                        cell.OnValidate();
                    }
                }
            }
        }

        public override void SetText(string text)
        {
            if (TextItem != null)
                TextItem.Text = text;// DateToString(mDayValue));
        }
    }
}
