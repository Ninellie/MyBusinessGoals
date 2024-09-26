using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsplash.DatePicker
{
    class DatePickerDropDownTextMeshPro : DatePickerDropDownBase
    {
        public TMPro.TextMeshProUGUI Label= null;

        protected override void SetText(string text)
        {
            if (Label != null)
                Label.text = text;
        }
    }
}
