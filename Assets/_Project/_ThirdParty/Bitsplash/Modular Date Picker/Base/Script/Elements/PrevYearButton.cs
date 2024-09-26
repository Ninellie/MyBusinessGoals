using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bitsplash.DatePicker
{
    public class PrevYearButton : DatePickerButton , IDatePickerSettingsItem
    {
        public int Order { get { return 8; } }
        public override void RaiseClicked()
        {
            if (Content != null)
                Content.PrevYear();
        }
        public string EditorTitle
        {
            get { return "Prev Year Button"; }
        }

    }
}
