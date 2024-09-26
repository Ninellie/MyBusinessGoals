using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bitsplash.DatePicker
{
    public abstract class DatePickerInput : MonoBehaviour
    {
        public abstract MultipleSelectionInputValue MultipleSelectionValue { get; }
    }
}
