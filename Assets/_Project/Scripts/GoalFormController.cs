using System;
using System.Linq;
using Bitsplash.DatePicker;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GoalFormController : MonoBehaviour
{
    [SerializeField] private TMP_InputField titleInput;
    [SerializeField] private TMP_InputField purposeInput;
    [SerializeField] private TMP_InputField descriptionInput;
    [SerializeField] private TMP_Dropdown categoryDropdown;
    
    //Input
    [SerializeField] private TMP_Text datePickerLabel;
    [SerializeField] private DatePickerDropDownBase datePicker;
    [SerializeField] private DatePickerContent dateContent;

    [SerializeField] private Button saveButton;
    
    [SerializeField] private GoalData goalData;
    [SerializeField] private UnityEvent<GoalData> onNewGoalCreate;

    public void Initiate()
    {
        Clear();
        saveButton.onClick.AddListener(() => onNewGoalCreate.Invoke(CollectData()));
        datePicker.DropDownButton.onClick.AddListener(ChangeDatePickerLabelToCurrentDate);
    }

    public void Fill(GoalData goal)
    {
        goalData = goal;
        
        titleInput.text = goal.Title;
        purposeInput.text = goal.Purpose;
        
        var optionDataList = categoryDropdown.options.Where(o => o.text == goal.Category).ToList();
        if (optionDataList.Count != 0)
        {
            var option = optionDataList.First();
            var index = categoryDropdown.options.IndexOf(option);
            categoryDropdown.value = index;
        }
        
        descriptionInput.text = goal.Description;
        
        ChangeDatePickerLabelToCurrentDate();
    }

    private void ChangeDatePickerLabelToCurrentDate()
    {
        var date = datePicker.GetSelectedDate();
        if (date == null)
        {
            if (string.IsNullOrEmpty(goalData.FilePath))
            {
                goalData.Date = DateTime.Today;
                date = DateTime.Today;
            }
            else
            {
                date = goalData.Date;
            }
        }
        dateContent.SelectOne(date.Value);
        datePickerLabel.text = date.Value.ToShortDateString();
    }

    public void Clear()
    {
        titleInput.text = string.Empty;
        purposeInput.text = string.Empty;

        datePicker.SetStartDate(DateTime.Today);
        dateContent.SelectOne(DateTime.Today);
        datePickerLabel.text = "Business date";

        categoryDropdown.SetValueWithoutNotify(0);
        descriptionInput.text = string.Empty;
        goalData = new GoalData
        {
            Date = DateTime.Today
        };
    }

    private GoalData CollectData()
    {
        goalData.Title = titleInput.text;
        goalData.Purpose = purposeInput.text;
        goalData.Date = (DateTime)datePicker.GetSelectedDate()!;
        goalData.Category = categoryDropdown.captionText.text;
        goalData.Description = descriptionInput.text;
        
        return goalData;
    }
}