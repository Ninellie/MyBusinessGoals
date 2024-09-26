using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bitsplash.DatePicker
{
    partial class DatePickerText
    {

        partial void InnerSetMain()
        {
            mMain.TextTypeChanged -= MMain_TextTypeChanged;
            mMain.TextTypeChanged += MMain_TextTypeChanged;
        }
        partial void CheckTextMesh(ref bool res)
        {
            res = GetComponent<TMPro.TextMeshProUGUI>() != null;
        }
        partial void DestroyTextMesh()
        {
            CommonMethods.SafeDestroy(GetComponent<TMPro.TextMeshProUGUI>());
        }

        partial void InnerVerifyTextObject()
        {
            if (mMain.TextType == TextTypeEnum.StandardText)
                return;
            var obj = gameObject;
            if (contentGameObject != null)
                obj = contentGameObject.gameObject;
            var rect = obj.GetComponent<RectTransform>();
            if (rect != null)
            {
                Vector2 size = rect.sizeDelta;
                Vector2 anchor = rect.anchoredPosition;
                var tmpObj = CommonMethods.EnsureComponent<TMPro.TextMeshProUGUI>(obj, true);
                mTextObject = tmpObj;
                tmpObj.font = mMain.FontAsset;
                rect.anchoredPosition = anchor;
                rect.sizeDelta = size;
            }
            else
                Debug.LogWarning("object must have a rect transform attached to it in order to use DatePickerText");
        }

        partial void MediateTextMeshProText(string text)
        {
            var tmp = mTextObject as TMPro.TextMeshProUGUI;
            if (tmp != null)
                tmp.text = text;
        }

        partial void MediateTextMeshAlignment(TextAnchor alignment)
        {
            var tmp = mTextObject as TMPro.TextMeshProUGUI;
            if (tmp != null)
            {
                switch(alignment)
                {
                    case TextAnchor.LowerCenter:
                        tmp.alignment = TMPro.TextAlignmentOptions.Bottom;
                        break;
                    case TextAnchor.LowerLeft:
                        tmp.alignment = TMPro.TextAlignmentOptions.BottomLeft;
                        break;
                    case TextAnchor.LowerRight:
                        tmp.alignment = TMPro.TextAlignmentOptions.BottomRight;
                        break;
                    case TextAnchor.MiddleCenter:
                        tmp.alignment = TMPro.TextAlignmentOptions.Center;
                        break;
                    case TextAnchor.MiddleLeft:
                        tmp.alignment = TMPro.TextAlignmentOptions.Left;
                        break;
                    case TextAnchor.MiddleRight:
                        tmp.alignment = TMPro.TextAlignmentOptions.Right;
                        break;
                    case TextAnchor.UpperCenter:
                        tmp.alignment = TMPro.TextAlignmentOptions.Top;
                        break;
                    case TextAnchor.UpperLeft:
                        tmp.alignment = TMPro.TextAlignmentOptions.TopLeft;
                        break;
                    case TextAnchor.UpperRight:
                        tmp.alignment = TMPro.TextAlignmentOptions.TopRight;
                        break;
                }
                
            }
        }
        partial void MediateTextMeshStyle(FontStyle style)
        {
            var tmp = mTextObject as TMPro.TextMeshProUGUI;
            if (tmp != null)
            {
                switch(style)
                {
                    case FontStyle.Bold:
                        tmp.fontStyle = TMPro.FontStyles.Bold;
                        break;
                    case FontStyle.BoldAndItalic:
                        tmp.fontStyle = TMPro.FontStyles.Bold | TMPro.FontStyles.Italic;
                        break;
                    case FontStyle.Normal:
                        tmp.fontStyle = TMPro.FontStyles.Normal;
                        break;
                    case FontStyle.Italic:
                        tmp.fontStyle = TMPro.FontStyles.Italic;
                        break;
                }
            }
        }
        partial void MediateTextMeshSize(int size)
        {
            var tmp = mTextObject as TMPro.TextMeshProUGUI;
            if (tmp != null)
                tmp.fontSize = size;
        }

        partial void MediateTextMeshProColor(Color color)
        {
            var tmp = mTextObject as TMPro.TextMeshProUGUI;
            if (tmp != null)
                tmp.color = color;

        }

        private void MMain_TextTypeChanged()
        {
            RecreateTextObject();
        }
    }
}
