using Bitsplash.DatePicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bitsplash.DatePicker
{
    [ExecuteInEditMode]
    public partial class DatePickerText : DatePickerElement
    {

        //[SerializeField]
        private RectTransform contentGameObject = null;

 //       [HideInInspector]
        [SerializeField]
        private string text;

        [SerializeField]
        private Color color = Color.white;

        [SerializeField]
        private int textSize = 14;

        [SerializeField]
        private FontStyle fontStyle = FontStyle.Normal;

        [SerializeField]
        private TextAnchor alignment = TextAnchor.MiddleLeft;
 
        protected DatePickerContent Content { get; set; }
        bool mValidate = false;
        public RectTransform ContentGameObject
        {
            get { return contentGameObject; }
            set
            {
                contentGameObject = value;
                RecreateTextObject();
            }
        }
        public string Text
        {
            get { return text; }
            set { SetText(value); }
        }
        public FontStyle FontStyle
        {
            get { return fontStyle; }
            set
            {
                SetStyle(value);
            }
        }
        public Color Color
        {
            get { return color; }
            set { SetColor(value); }
        }

        public TextAnchor Alignment
        {
            get { return alignment; }
            set
            {
                SetAlignment(value);
            }
        }
        public int TextSize
        {
            get { return textSize; }
            set
            {
                SetTextSize(value);
            }
        }

        DatePickerSettings mMain;

        [HideInInspector]
        [SerializeField]
        UnityEngine.Object mTextObject;

        protected override void SetContent(DatePickerContent content)
        {
            Content = content;
        }

        protected override void SetMain(DatePickerSettings main)
        {
            mMain = main;
            InnerSetMain();
            if(mTextObject == null)
                RecreateTextObject();
        }

        void OnEnable()
        {
            if(mTextObject == null)
                RecreateTextObject(true);
        }

        void RecreateTextObject(bool overrideCheck = false)
        {
            if(overrideCheck || isActiveAndEnabled)
                StartCoroutine(RecreateTextObjectCorout());
        }
        partial void DestroyTextMesh();
        IEnumerator RecreateTextObjectCorout()
        {
            bool res = false;
            CheckTextMesh(ref res);
            if(mValidate)
                yield return 0;
            if (mTextObject != null || GetComponent<Text>() != null || res == true)
            {
                CommonMethods.SafeDestroy(mTextObject);
                CommonMethods.SafeDestroy(GetComponent<Text>());
                DestroyTextMesh();
                mTextObject = null;
                yield return 0;
            }
            VerifyTextObject();
        }
        void VerifyTextObject()
        {
            if (mMain == null)
                return;
            InnerVerifyTextObject();
            if(mTextObject == null)
            {
                var obj = gameObject;
                if (contentGameObject != null)
                    obj = contentGameObject.gameObject;
                var text = CommonMethods.EnsureComponent<Text>(obj,true);
                text.font = mMain.TextFont;
                mTextObject = text;
            }
            ApplyText();
        }
        void ApplyText()
        {
            SetText(text);
            SetTextSize(textSize);
            SetColor(color);
            SetAlignment(alignment);
            SetStyle(fontStyle);
        }

        partial void InnerSetMain();
        partial void InnerVerifyTextObject();


        partial void MediateTextMeshProText(string text);
        partial void MediateTextMeshProColor(Color color);
        partial void MediateTextMeshAlignment(TextAnchor alignment);
        partial void MediateTextMeshSize(int size);
        partial void MediateTextMeshStyle(FontStyle style);

        partial void CheckTextMesh(ref bool res);

        private void SetAlignment(TextAnchor alignment)
        {
            this.alignment = alignment;
            MediateTextMeshAlignment(alignment);
            var comp = mTextObject as Text;
            if (comp != null)
                comp.alignment = alignment;
        }

        private void SetStyle(FontStyle style)
        {
            this.fontStyle = style;
            MediateTextMeshStyle(style);
            var comp = mTextObject as Text;
            if (comp != null)
                comp.fontStyle = style;
        }

        private void SetTextSize(int size)
        {
            this.textSize = size;
            MediateTextMeshSize(size);
            var comp = mTextObject as Text;
            if (comp != null)
                comp.fontSize = size;
        }

        public override void OnValidate()
        {
            mValidate = true;
            bool create = mTextObject != null;
            base.OnValidate();
            if(create)
                RecreateTextObject(false);
            ApplyText();
            mValidate = false;
        }

        private void SetText(string text)
        {
            this.text = text;
            MediateTextMeshProText(text);
            var comp = mTextObject as Text;
            if (comp != null)
                comp.text = text;
        }

        private void SetColor(Color color)
        {
            this.color = color;
            MediateTextMeshProColor(color);
            var comp = mTextObject as Text;
            if (comp != null)
                comp.color = color;

        }
    }
}
