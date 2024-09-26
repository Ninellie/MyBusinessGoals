using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsplash.DatePicker
{
    interface IDatePickerCollectionPrivate
    {
        bool Changed { get; set; }
        bool AllowEmpty { get; set; }
        event Action SelectionModified;
    }
}
