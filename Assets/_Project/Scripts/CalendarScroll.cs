using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CalendarScroll : MonoBehaviour
{
    [SerializeField] private int itemsCount;
    [SerializeField] private GameObject dateComponentPrefab;
    [SerializeField] private RectTransform content;
    
    [Header("Item Properties")]
    [SerializeField] private CalendarDayColorPreset unselectedDayColors;
    [SerializeField] private CalendarDayColorPreset selectedDayColors;
    
    [SerializeField] private List<CalendarDay> items;
    
    [SerializeField] private UnityEvent<DateTime> onDayClick;
    
    public CalendarDay SelectedDay { get; private set; }
    public UnityEvent<DateTime> OnDayClick => onDayClick;
    
    private LinkedList<CalendarDay> _items;

    public void Initiate()
    {
        _items = new LinkedList<CalendarDay>();
        var startDate = DateTime.Today.AddDays(Mathf.CeilToInt(-itemsCount / 2));
        CreateItems(startDate);
    }

    private void CreateItems(DateTime startDate)
    {
        for (var i = 0; i < itemsCount; i++)
        {
            var itemGameObject = Instantiate(dateComponentPrefab, content);
            var date = startDate.AddDays(i);
            
            var calendarDay = itemGameObject.GetComponent<CalendarDay>();
            _items.AddLast(calendarDay);

            calendarDay.SetDate(date);

            if (date == DateTime.Today)
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
    }

    private void UnselectDay(CalendarDay day)
    {
        day.SetBackgroundColorGradient(unselectedDayColors.Background);
        day.SetDayOfWeekColorGradient(unselectedDayColors.DayOfWeek);
        day.SetDayColorGradient(unselectedDayColors.DayText);
    }
    
    // Отображение для удобного просмотра списка в редактор
    private void OnValidate()
    {
        if (_items == null)
        {
            return;
        }
        items = _items.ToList();
    }
}