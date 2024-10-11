using System;
using UnityEngine;
using UnityEngine.Events;

public class CalendarScroll : MonoBehaviour
{
    [SerializeField] private int itemsCount;
    [SerializeField] private GameObject dateComponentPrefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private EndlessScroll endlessScroll;
    
    [Header("Item Properties")]
    [SerializeField] private CalendarDayColorPreset unselectedDayColors;
    [SerializeField] private CalendarDayColorPreset selectedDayColors;

    [SerializeField] private UnityEvent<DateTime> onDayClick;
    
    public CalendarDay SelectedDay { get; private set; }
    public UnityEvent<DateTime> OnDayClick => onDayClick;
    
    private DateTime _selectedDayTime;
    private DateTime _maxDateTime;
    private DateTime _minDateTime;

    public void Initiate()
    {
        var startDate = DateTime.Today.AddDays(Mathf.CeilToInt(-itemsCount / 2));
        CreateItems(startDate);
    }

    private void OnEnable() => endlessScroll.onShift.AddListener(OnShift);

    private void OnDisable() => endlessScroll.onShift.RemoveListener(OnShift);

    private void CreateItems(DateTime startDate)
    {
        _minDateTime = startDate;
        _selectedDayTime = DateTime.Today;
        for (var i = 0; i < itemsCount; i++)
        {
            var itemGameObject = Instantiate(dateComponentPrefab, content);
            var date = startDate.AddDays(i);
            
            var calendarDay = itemGameObject.GetComponent<CalendarDay>();

            calendarDay.SetDate(date);

            if (date == _selectedDayTime)
            {
                SelectDay(calendarDay);
                onDayClick.Invoke(date);
            }
            else
            {
                UnselectDay(calendarDay);
            }
            
            calendarDay.OnClick.AddListener(() => onDayClick.Invoke(date));
            calendarDay.OnClick.AddListener(() => SelectDay(calendarDay));
            _maxDateTime = date;
        }
    }
    
    private void OnShift(GameObject dayElement)
    {
        var day = dayElement.GetComponent<CalendarDay>();
        
        // Если сдвигаемый элемент выбран, раскрасить его как обычный день и убрать выбранный день, но не дату
        if (day.DateTime == _selectedDayTime)
        {
            UnselectDay(day);
            SelectedDay = null;
        }
        // Если произошёл шифт, то его дата или минимальна или максимальна
        if (day.DateTime == _minDateTime)
        {
            // Элемент перешёл слево направо, значит нужно присвоить ему дату +1 день от максимальной
            var dayAfterMaxDate = _maxDateTime.AddDays(1);
            day.SetDate(dayAfterMaxDate);
            _maxDateTime = dayAfterMaxDate;
            _minDateTime = _minDateTime.AddDays(1);
        }else if (day.DateTime == _maxDateTime) 
        {
            // Элемент перешёл справа налево, значит нужно присвоить ему дату -1 день от минимальной
            var dayBeforeMinDate = _minDateTime.AddDays(-1);
            day.SetDate(dayBeforeMinDate);
            _minDateTime = dayBeforeMinDate;
            _maxDateTime = _maxDateTime.AddDays(-1);
        }

        // Если элемент после того как был сдвинут стал той же даты что был выбранный - выбрать его визуально
        if (day.DateTime == _selectedDayTime)
        {
            SelectDay(day);
        }
    }
    
    private void SelectDay(CalendarDay day)
    {
        if (day == SelectedDay)
        {
            return;
        }

        if (SelectedDay != null)
        {
            UnselectDay(SelectedDay);            
        }

        day.SetBackgroundColorGradient(selectedDayColors.Background);
        day.SetDayOfWeekColorGradient(selectedDayColors.DayOfWeek);
        day.SetDayColorGradient(selectedDayColors.DayText);
        
        SelectedDay = day;
        _selectedDayTime = day.DateTime;
    }

    private void UnselectDay(CalendarDay day)
    {
        day.SetBackgroundColorGradient(unselectedDayColors.Background);
        day.SetDayOfWeekColorGradient(unselectedDayColors.DayOfWeek);
        day.SetDayColorGradient(unselectedDayColors.DayText);
    }
}