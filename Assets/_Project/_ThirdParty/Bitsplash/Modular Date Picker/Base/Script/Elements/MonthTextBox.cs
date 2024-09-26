using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bitsplash.DatePicker
{
    public class MonthTextBox : DatePickerText , IDatePickerSettingsItem
    {

        [SerializeField]
        private string dateFormat = "MMMM, yyyy";

        public string DateFormat
        {
            get { return dateFormat; }
            set
            {
                dateFormat = value;
                RefreshText();
            }
        }

        public int Order { get { return 8; } }

        public string EditorTitle
        {
            get { return "Month Text Box - " + gameObject.name; }
        }

        protected override void SetContent(DatePickerContent content)
        {
            if (Content != null)
                Content.OnDisplayChanged.RemoveListener(DisplayChanged);
            base.SetContent(content);
            if (Content != null)
            {
                Content.OnDisplayChanged.AddListener(DisplayChanged);
                DisplayChanged();
            }
        }

        void RefreshText()
        {
            if (Content == null)
                return;
            try
            {
                Text = (Content.DisplayDate.ToString(dateFormat));
            }
            catch (Exception)
            {
                Debug.LogWarning("Invalid date format for text box - " + DateFormat);
            }

        }
        void DisplayChanged()
        {
            RefreshText();
        }

        void OnDestroy()
        {
            if (Content != null)
                Content.OnDisplayChanged.RemoveListener(DisplayChanged);
        }

    }
}
