using Bitsplash.DatePicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bitsplash.DatePicker
{
    public class TodayButton : DatePickerButton , IDatePickerSettingsItem
    {
        public int Order { get { return 8; } }
        public override void RaiseClicked()
        {
            if (Content != null)
                Content.Selection.SelectOne(DateTime.Today);
        }
        public string EditorTitle
        {
            get { return "Today Button"; }
        }
    }
}
