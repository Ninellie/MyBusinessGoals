using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bitsplash.DatePicker
{
    public class NextMonthButton : DatePickerButton , IDatePickerSettingsItem
    {
        public int Order { get { return 8; } }
        public override void RaiseClicked()
        {
            if(Content != null)
                Content.NextMonth();
        }
        public string EditorTitle
        {
            get { return "Next Month Button"; }
        }

    }
}
