using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bitsplash.DatePicker
{
    public class NextYearButton : DatePickerButton , IDatePickerSettingsItem
    {

        public int Order { get { return 8; } }
        public override void RaiseClicked()
        {
            if (Content != null)
                Content.NextYear();
        }

        public string EditorTitle
        {
            get { return "Next Year Button"; }
        }
    }
}
