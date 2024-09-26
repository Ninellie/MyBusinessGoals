using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bitsplash.DatePicker
{
    public class DatePickerCellTemplate : MonoBehaviour, IDatePickerSettingsItem
    {
        [SerializeField]
        [HideInInspector]
        private bool isOpen;

        public string EditorTitle { get { return gameObject.name; } }

        public int Order { get { return 2; } }
    }
}
