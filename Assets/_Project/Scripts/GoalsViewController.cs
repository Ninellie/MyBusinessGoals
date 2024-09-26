using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum GoalView
{
    Calendar,
    AllGoals,
}

public class GoalsViewController : MonoBehaviour
{
    [Header("Services")]
    [SerializeField] private GoalsRepository repository;
    [SerializeField] private GoalDisplayScroll display;
    [SerializeField] private GoalFormController inputForm;
    [SerializeField] private CalendarScroll calendar;
    
    [Header("Panels")]
    [SerializeField] private GameObject emptyGoalsPanel;
    [SerializeField] private GameObject emptyAllGoalsPanel;
    [SerializeField] private GameObject allGoalsInitialPanel;
    [SerializeField] private GameObject tabBar;
    [SerializeField] private GameObject goalForm;
    [SerializeField] private GameObject displayBar;
    
    [SerializeField] private List<GameObject> calendarViewObjects;
    [SerializeField] private List<GameObject> allGoalsViewObjects;

    [Header("Panels")]
    [SerializeField] private GoalView initialView;
    
    private bool _isAllGoalsViewOpen;

    private void Start()
    {
        display.Initialize();
        repository.Initialize();
        calendar.Initiate();
        inputForm.Initiate();
        
        this.Initialize();
    }

    private void Initialize()
    {
        if (initialView == GoalView.Calendar)
        {
            ShowCalendarView();
            return;
        }
        ShowAllGoalsView();
    }

    private void OnEnable()
    {
        calendar.OnDayClick.AddListener(DisplayGoalsByDate);
        
        display.OnEdit.AddListener(OpenFormForEditingGoal);
        display.OnDelete.AddListener(DeleteGoal);
    }
    
    private void OnDisable()
    {
        calendar.OnDayClick.RemoveListener(DisplayGoalsByDate);
        
        display.OnEdit.RemoveListener(OpenFormForEditingGoal);
        display.OnDelete.RemoveListener(DeleteGoal);
    }

    public void ShowAllGoalsView()
    {
        displayBar.SetActive(false);
        
        allGoalsInitialPanel.SetActive(true);
        emptyAllGoalsPanel.SetActive(false);
        emptyGoalsPanel.SetActive(false);
        
        SwitchMainMenuView(false);
        
        display.Clear();
    }

    public void ShowCalendarView()
    {
        SwitchMainMenuView(true);
        allGoalsInitialPanel.SetActive(false);
        emptyAllGoalsPanel.SetActive(false);
        DisplayGoalsByDate(calendar.SelectedDay.DateTime);
    }

    private void SwitchMainMenuView(bool showCalendar)
    {
        foreach (var viewObject in calendarViewObjects)
        {
            viewObject.SetActive(showCalendar);
        }
        
        foreach (var viewObject in allGoalsViewObjects)
        {
            viewObject.SetActive(!showCalendar);
        }
        
        //_isAllGoalsViewOpen = !showCalendar; 
    }

    public void DisplayGoalsBySearch(GoalData searchPattern)
    {
        var goals = repository.GetGoalsByPattern(searchPattern);
        var goalsExists = goals.Count != 0;
        
        displayBar.SetActive(goalsExists);
        emptyAllGoalsPanel.SetActive(!goalsExists);
        allGoalsInitialPanel.SetActive(false);
        
        display.Display(goals);
    }
    
    // МЕТОДЫ ИНПУТ ФОРМЫ
    public void ShowGoalForm()
    {
        inputForm.Clear();
        tabBar.SetActive(false);
        goalForm.SetActive(true);
    }

    public void CloseGoalForm()
    {
        inputForm.Clear();
        tabBar.SetActive(true);
        goalForm.SetActive(false);
    }

    public void SaveAndCloseGoalForm(GoalData goal)
    {
        // Сохранение цели в репозитории
        repository.SaveGoal(goal);
        
        // Скрыть и очистить форму
        CloseGoalForm();
        
        DisplayGoalsByDate(calendar.SelectedDay.DateTime);
    }
    
    private void DeleteGoal(GoalData goal)
    {
        GoalsRepository.DeleteGoal(goal.FilePath);
        DisplayGoalsByDate(goal.Date);
    }
    
    private void DisplayGoalsByDate(DateTime date)
    {
        var goals = repository.GetGoalsByDate(date);
        
        var goalsExists = goals.Count != 0; 
        
        displayBar.SetActive(goalsExists);

        if (_isAllGoalsViewOpen)
        {
            emptyAllGoalsPanel.SetActive(!goalsExists);
        }
        else
        {
            emptyGoalsPanel.SetActive(!goalsExists);
        }
        
        display.Display(goals);
    }
    
    private void OpenFormForEditingGoal(GoalData goal)
    {
        ShowGoalForm();
        inputForm.Fill(goal);
    }
}