using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitsplash.DatePicker
{
    interface IDatePickerPrivate
    {
        void RaiseClick(int childIndex);
        void RaiseStartSelection(int childIndex);
        void RaiseSelectionEnter(int childIndex, int fromChildIndex);
        void RaiseSelectionExit(int childIndex, int fromChildIndex);
        void EndSelection();
    }
}
