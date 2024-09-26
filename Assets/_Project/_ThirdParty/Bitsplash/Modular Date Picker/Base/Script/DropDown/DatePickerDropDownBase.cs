using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Bitsplash.DatePicker
{
    public abstract class DatePickerDropDownBase : MonoBehaviour
    {

        /// <summary>
        /// 
        /// </summary>
        public string NoSelectionPrompt = "Select a date...";
        /// <summary>
        /// the date format of the label
        /// </summary>
        public string labelDateFormat = "d";
        /// <summary>
        /// the date picker settings object for the drop down
        /// </summary>
        public DatePickerSettings DropDownContent;
        /// <summary>
        /// the drop down button
        /// </summary>
        public Button DropDownButton;

        GameObject mBlocker;

        public Action<DateTime?> DateChanged;
        // Start is called before the first frame update
        void Start()
        {
            InitDropDown();
        }

        /// <summary>
        /// initializes the drop down events
        /// </summary>
        void InitDropDown()
        {
            if(DropDownButton == null)
                Debug.LogWarning("Drop Down Button Not Assigned"); // show warninig
            else
                DropDownButton.onClick.AddListener(ButtonClicked); // listen to drop down button clicks
            if (DropDownContent == null)
                Debug.LogWarning("Drop Down Content Not Assigned");// show warninig
            else
            {
                // set the selection mode to single.
                DropDownContent.Content.SelectionMode = SelectionType.Single;
                // listen to selection changed events on the date picker
                DropDownContent.Content.OnSelectionChanged.AddListener(SelectionChanged);
                // disable the drop down object
                DropDownContent.gameObject.SetActive(false);
                Canvas canvas = CommonMethods.EnsureComponent<Canvas>(DropDownContent.gameObject);
                CommonMethods.EnsureComponent<GraphicRaycaster>(DropDownContent.gameObject);

                
            }
        }

        public void SetStartDate(DateTime date)
        {
            if (DropDownContent == null)
                Debug.LogWarning("Drop Down Content Not Assigned");// show warninig
            else
            {
                DropDownContent.Content.SelectOne(date);               
            }
        }

        protected abstract void SetText(string text);
        
        /// <summary>
        /// shows the drop down
        /// </summary>
        void Show()
        {
            var canvas = DropDownContent.GetComponent<Canvas>();
            if (canvas == null)
                return;
            DropDownContent.gameObject.SetActive(true);
            canvas.overrideSorting = true;
            canvas.sortingOrder = 30000;
            mBlocker = CreateBlocker();   
        }
        /// <summary>
        /// returnes the selected date from the drop down , or null if non is selected
        /// </summary>
        /// <returns></returns>
        public System.DateTime? GetSelectedDate()
        {
            if (DropDownContent == null)
                return null;
            if (DropDownContent.Content.Selection.Count != 1)
                return null;
            return DropDownContent.Content.Selection.GetItem(0);
        }
        //hides the drop down
        void Hide()
        {
            DropDownContent.gameObject.SetActive(false);
            CommonMethods.SafeDestroy(mBlocker);
        }
        /// <summary>
        /// called when the date picker selection has changed
        /// </summary>
        void SelectionChanged()
        {
            var d = GetSelectedDate(); // get the selected date
            string t = NoSelectionPrompt;
            try
            {
                if (d.HasValue)
                    t = d.Value.ToString(labelDateFormat); // find the correct string to show for the selected date
            }
            catch(Exception)
            {
                Debug.LogWarning("the format specified for the drop down is not valid");
            }
            SetText(t); // show the selected date
            DateChanged?.Invoke(d);
            Hide();
        }

        protected virtual GameObject CreateBlocker()
        {
            var canvasItems = GetComponentsInParent<Canvas>();
            if (canvasItems.Length == 0)
                return null;

            Canvas rootCanvas = canvasItems[0];
            GameObject gameObject = new GameObject("Blocker");
            RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.SetParent(rootCanvas.transform, false);
            rectTransform.anchorMin = (Vector2)Vector3.zero;
            rectTransform.anchorMax = (Vector2)Vector3.one;
            rectTransform.sizeDelta = Vector2.zero;
            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            Canvas component = DropDownContent.GetComponent<Canvas>();
            canvas.sortingLayerID = component.sortingLayerID;
            canvas.sortingOrder = component.sortingOrder - 1;
            gameObject.AddComponent<GraphicRaycaster>();
            gameObject.AddComponent<Image>().color = Color.clear;
            gameObject.AddComponent<Button>().onClick.AddListener(new UnityAction(this.Hide));
            return gameObject;
        }

        /// <summary>
        /// handle the drop down button click
        /// </summary>
        void ButtonClicked()
        {
            if (DropDownContent != null)
            {
                if (DropDownContent.gameObject.activeSelf)
                    Hide();
                else
                    Show();
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
