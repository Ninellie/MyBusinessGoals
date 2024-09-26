using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

namespace Bitsplash.DatePicker
{
    /// <summary>
    /// this is a UI.Text implementation of the drop down
    /// </summary>
    public class DatePickerDropDown : DatePickerDropDownBase
    {
        public TMP_Text Label;

        protected override void SetText(string text)
        {
            if (Label != null)
                Label.text = text;
        }
    }
}
