using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bitsplash.DatePicker
{
    public class DatePickerStandardInput : DatePickerInput
    {
        public override MultipleSelectionInputValue MultipleSelectionValue
        {
            get
            {
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.RightCommand) || Input.GetKey(KeyCode.LeftCommand))
                    return MultipleSelectionInputValue.Append;
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    return MultipleSelectionInputValue.Append;
                return MultipleSelectionInputValue.Singular;
            }
        }
    }
}
