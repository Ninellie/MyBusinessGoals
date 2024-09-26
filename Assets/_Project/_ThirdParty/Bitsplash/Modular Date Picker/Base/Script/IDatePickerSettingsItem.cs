using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsplash.DatePicker
{
    public interface IDatePickerSettingsItem
    {
        String EditorTitle { get; }
        int Order { get; }
    }
}
