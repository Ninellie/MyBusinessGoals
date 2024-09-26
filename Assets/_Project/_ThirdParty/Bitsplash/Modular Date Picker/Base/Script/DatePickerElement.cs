using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bitsplash.DatePicker
{
    /// <summary>
    /// base class for date picker elements. They included buttons and texts
    /// </summary>
    public abstract class DatePickerElement : MonoBehaviour
    {

        [SerializeField]
        [HideInInspector]
        private bool isOpen;

        protected virtual void Start()
        {
            SetLinkedElements();
        }

        /// <summary>
        /// notifys the date picker element of the DatePickerSettings and DatePickerContent objects that govern it. 
        /// these objects are used to query and modify the date picker
        /// </summary>
        void SetLinkedElements()
        {
            var main = GetComponentInParent<DatePickerSettings>();
            if (main == null)
                Debug.LogError("Date Picker elements must have a parent GameObject with the behviour DatePickerSettings");
            else
            {
                var content = main.Content;
                if (content != null)
                {
                    SetMain(main);
                    SetContent(content);
                    content.SettingsChanged -= OnSettingsChanged;
                    content.SettingsChanged += OnSettingsChanged;
                }
            }
        }

        protected virtual void OnSettingsChanged()
        {

        }

        public virtual void OnValidate()
        {
            if(isActiveAndEnabled)
                SetLinkedElements();
        }

        /// <summary>
        /// sets the main DatePickerSettings object assicuated with this script
        /// </summary>
        /// <param name="main"></param>
        protected abstract void SetMain(DatePickerSettings main);
        /// <summary>
        /// sets the main DatePickerContent object assicuated with this script
        /// </summary>
        /// <param name="content"></param>
        protected abstract void SetContent(DatePickerContent content);
    }
}