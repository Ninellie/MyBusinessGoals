using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bitsplash.DatePicker
{
    public class PrevMonthButton : DatePickerButton , IDatePickerSettingsItem
    {
        public int Order { get { return 8; } }
        public override void RaiseClicked()
        {
            if (Content != null)
                Content.PrevMonth();
        }
        public string EditorTitle
        {
            get { return "Prev Month Button"; }
        }
    }
}
