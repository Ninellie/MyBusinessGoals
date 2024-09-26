using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Bitsplash.DatePicker
{

    public partial class DatePickerSettings : MonoBehaviour
    {
        [SerializeField]
        private Font textFont;

        /// <summary>
        /// the text font used for UI.Text 
        /// </summary>
        public Font TextFont
        {
            get { return textFont; }
            set
            {
                textFont = value;
                if (TextTypeChanged != null)
                    TextTypeChanged();
            }
        }

        public event Action TextTypeChanged;
        DatePickerContent mContent = null;

        /// <summary>
        /// the datepicker content object for this date picker. 
        /// </summary>
        public DatePickerContent Content
        {
            get
            {
                if (mContent == null)
                {
                    var contents = GetComponentsInChildren<DatePickerContent>();
                    if (contents.Length == 0)
                        Debug.LogError("A DatePickerSettings behaviour must parent a DatePickerContent behaviour in a child GameObject");
                    else
                        if (contents.Length > 1)
                        Debug.LogError("A DatePickerSettings behaviout may only have one child DatePickerContent behaviour ");
                    else
                        mContent = contents[0];
                }
                return mContent;
            }
        }

        public void SetStartDate(DateTime date)
        {
            if (mContent == null)
                Debug.LogWarning("Drop Down Content Not Assigned");// show warninig
            else
            {
                mContent.SelectOne(date);
            }
        }

        private void Start()
        {

        }

        private void OnValidate()
        {
            foreach (var elem in GetComponentsInChildren<DatePickerElement>())
            {
                elem.OnValidate();
            }
        }
    }
}
