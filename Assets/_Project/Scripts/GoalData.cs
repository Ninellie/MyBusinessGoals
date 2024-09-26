using System;
using UnityEngine;

[Serializable]
public class GoalData
{
    [SerializeField] private string title;
    [SerializeField] private string purpose;
    [SerializeField] private string category;
    private DateTime _date;
    [SerializeField] private string description;
    [SerializeField] private string filePath;

    public string Title
    {
        get => title;
        set => title = value;
    }

    public string Purpose
    {
        get => purpose;
        set => purpose = value;
    }

    public string Category
    {
        get => category;
        set => category = value;
    }

    public DateTime Date
    {
        get => _date;
        set => _date = value;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }
        
    public string FilePath
    {
        get => filePath;
        set => filePath = value;
    }
}