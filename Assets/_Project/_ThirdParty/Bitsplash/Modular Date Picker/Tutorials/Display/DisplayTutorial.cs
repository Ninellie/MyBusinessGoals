using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bitsplash.DatePicker.Tutorials
{
    public class DisplayTutorial : MonoBehaviour
    {
        public DatePickerSettings Picker;
        public Text YearText;
        public Text InfoText;
        // Start is called before the first frame update
        void Start()
        {
            if(InfoText != null)
            {
                InfoText.text = Picker.Content.DisplayDate.ToString("MM-yyyy"); // shows the display date of the picker
            }
            
            Picker.Content.OnDisplayChanged.AddListener(DisplayChanged); // triggred when the used navigates the date picker display
        }
        void DisplayChanged()
        {
            InfoText.text = Picker.Content.DisplayDate.ToString("MM-yyyy"); // shows the display date of the picker
        }
        public void ModifyYear()
        {
            try
            {
                
                int newYear = int.Parse(YearText.text);
                if (newYear < 1800 && newYear > 2025)
                    Debug.Log("Invalid year");
                else
                {
                    Picker.Content.SetYear(newYear); // set the display year for the datepicker
                }
            }
            catch(Exception)
            {

            }
        }
        // Update is called once per frame
        void Update()
        {
        }
    }
}
