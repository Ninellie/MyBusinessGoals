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
    [SerializeField] private GoalsSearch goalsSearch;
    
    [Header("Panels")]
    [SerializeField] private GameObject emptyGoalsPanel;
    [SerializeField] private GameObject nothingWasFoundPanel;
    //[SerializeField] private GameObject allGoalsInitialPanel;
    [SerializeField] private GameObject tabBar;
    [SerializeField] private GameObject goalForm;
    [SerializeField] private GameObject displayBar;
    [SerializeField] private GameObject topIndicatorsBar;
    [SerializeField] private GameObject mainMenuView;
    
    [SerializeField] private List<GameObject> calendarViewObjects;
    [SerializeField] private List<GameObject> allGoalsViewObjects;
    [SerializeField] private List<GameObject> firstOnboardingViewObjects;
    [SerializeField] private List<GameObject> secondOnboardingViewObjects;

    [Header("Settings")]
    [SerializeField] private GoalView currentView;

    private bool _showOnboarding;
    private const string FirstLaunchKey = "FirstLaunch";
    
    private void Awake()
    {
        _showOnboarding = !PlayerPrefs.HasKey(FirstLaunchKey);
    }

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
        if (_showOnboarding)
        {
            topIndicatorsBar.SetActive(false);
            ShowOnboarding();
            return;
        }
        topIndicatorsBar.SetActive(true);
        HideOnboarding();
        ShowCalendarView();
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

    public void ShowCalendarView()
    {
        mainMenuView.SetActive(true);
        
        SwitchMainMenuView(true);
        nothingWasFoundPanel.SetActive(false);
        DisplayGoalsByDate(calendar.SelectedDay.DateTime);
    }

    public void ShowAllGoalsView()
    {
        mainMenuView.SetActive(true);
        nothingWasFoundPanel.SetActive(false);
        SwitchMainMenuView(false);
        
        DisplayAllGoals();
    }

    public void DisplayGoalsBySearch(GoalData searchPattern)
    {
        var goals = repository.GetGoalsByPattern(searchPattern);
        
        // Вернулось null, поле поиска пустое и ни одна категория не выбрана или не валиден
        if (goals == null)
        {
            // Показать все существующие цели
            DisplayAllGoals();
            return;
        }
        
        // Поля валидны, но поиск не дал результатов
        if (goals.Count == 0)
        {
            displayBar.SetActive(false);
            nothingWasFoundPanel.SetActive(true);
            emptyGoalsPanel.SetActive(false);
            return;
        }
        
        // Поля валидны, поиск дал результаты
        nothingWasFoundPanel.SetActive(false);
        displayBar.SetActive(true);
        
        display.Display(goals);
    }

    #region InputFormMethods
    
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
        // Сохраненить новую цель
        repository.SaveGoal(goal);
        
        // Скрыть форму
        CloseGoalForm();
        
        // Обновить 
        
        DisplayGoalsByDate(calendar.SelectedDay.DateTime);
    }
    
    #endregion

    /// <summary>
    /// Закрывает онбоардинг панель, а также делает пометку в PlayerPrefs что первый запуск приложения был 
    /// </summary>
    public void HideOnboarding()
    {
        PlayerPrefs.SetString(FirstLaunchKey, "false");
        topIndicatorsBar.SetActive(true);
        foreach (var viewObject in firstOnboardingViewObjects)
        {
            viewObject.SetActive(false);
        }
        foreach (var viewObject in secondOnboardingViewObjects)
        {
            viewObject.SetActive(false);
        }
    }

    private void ShowOnboarding()
    {
        foreach (var viewObject in firstOnboardingViewObjects)
        {
            viewObject.SetActive(true);
        }
    }

    private void DisplayAllGoals()
    {
        var allGoals = repository.GetAllGoals();
        DisplayGoals(allGoals);
    }
    
    private void DisplayGoalsByDate(DateTime date)
    {
        var goals = repository.GetGoalsByDate(date);
        DisplayGoals(goals);
    }

    private void DisplayGoals(List<GoalData> goals)
    {
        display.Clear();
        nothingWasFoundPanel.SetActive(false);
        if (goals.Count == 0)
        {
            displayBar.SetActive(false);
            emptyGoalsPanel.SetActive(true);
            return;
        }
        displayBar.SetActive(true);
        emptyGoalsPanel.SetActive(false);
        display.Display(goals);
    }
    
    private void SwitchMainMenuView(bool showCalendar)
    {
        currentView = showCalendar ? GoalView.Calendar : GoalView.AllGoals;
        foreach (var viewObject in calendarViewObjects)
        {
            viewObject.SetActive(showCalendar);
        }
        
        foreach (var viewObject in allGoalsViewObjects)
        {
            viewObject.SetActive(!showCalendar);
        }
    }
    
    private void DeleteGoal(GoalData goal)
    {
        GoalsRepository.DeleteGoal(goal.FilePath);

        if (currentView == GoalView.Calendar)
        {
            DisplayGoalsByDate(goal.Date);
            return;
        }
        goalsSearch.Search();
    }
    
    private void OpenFormForEditingGoal(GoalData goal)
    {
        ShowGoalForm();
        inputForm.Fill(goal);
    }
}