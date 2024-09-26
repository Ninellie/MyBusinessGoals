using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Bitsplash.DatePicker
{
    [ExecuteInEditMode]
    public abstract class DatePickerButton : DatePickerElement
    {
        Button mButton;
        public Image TargetImage;
        public DatePickerText TargetText;

        protected DatePickerContent Content { get; private set; } 

        public abstract void RaiseClicked();

        protected override void SetContent(DatePickerContent content)
        {
            Content = content;
        }

        protected override void SetMain(DatePickerSettings main)
        {

        }
        
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            mButton = GetComponent<Button>();
            if (mButton != null)
                mButton.onClick.AddListener(RaiseClicked);
        }

        void OnDestroy()
        {
            if(mButton != null)
                mButton.onClick.RemoveListener(RaiseClicked);
        }
    }
}