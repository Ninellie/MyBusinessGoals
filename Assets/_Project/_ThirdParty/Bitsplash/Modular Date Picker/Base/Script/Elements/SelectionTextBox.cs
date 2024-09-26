using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bitsplash.DatePicker
{
    public class SelectionTextBox : DatePickerText , IDatePickerSettingsItem
    {
        public string DateFormat = "ddd, MMM d";
        public string Seperator = " ... ";
        public string NothingSelectedString = "";

        public int Order { get { return 8; } }

        protected override void SetContent(DatePickerContent content)
        {
            if (Content != null)
                Content.OnSelectionChanged.RemoveListener(SelectionChanged);
            base.SetContent(content);
            if (Content != null)
            {
                Content.OnSelectionChanged.AddListener(SelectionChanged);
                SelectionChanged();
            }
        }
        void RefreshText()
        {
            if (Content == null)
                return;
            try
            {
                string text = NothingSelectedString;
                if (Content.Selection.Count > 0)
                {
                    text = Content.Selection.GetItem(0).ToString(DateFormat);
                    if (Content.Selection.Count > 1)
                    {
                        text += Seperator + Content.Selection.GetItem(Content.Selection.Count - 1).ToString(DateFormat);
                    }
                }
                Text = text;
            }
            catch (Exception)
            {
                Debug.LogWarning("Invalid date format for text box - " + DateFormat);
            }

        }
        void SelectionChanged()
        {
            RefreshText();
        }

        void OnDestroy()
        {
            if (Content != null)
                Content.OnSelectionChanged.RemoveListener(SelectionChanged);
        }
        public string EditorTitle
        {
            get { return "Selection Text Box - " + gameObject.name; }
        }
    }
}
