﻿using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GoalsSearch : MonoBehaviour
{
    [SerializeField] private string selectedTag;
    [SerializeField] private TMP_InputField searchInputField;
    [SerializeField] private UnityEvent<GoalData> onSearch;
    
    public void SelectTag(string tagToggle)
    {
        selectedTag = tagToggle;
        Search();
    }

    public void DeselectTag()
    {
        selectedTag = null;
        Search();
    }

    public void Search()
    {
        var data = new GoalData
        {
            Title = searchInputField.text,
            Category = selectedTag
        };
        
        onSearch.Invoke(data);
    }
}