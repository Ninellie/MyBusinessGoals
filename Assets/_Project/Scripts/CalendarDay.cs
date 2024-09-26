using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CalendarDay : MonoBehaviour
{
    [SerializeField] private TMP_Text weekDay;
    [SerializeField] private TMP_Text day;
    [SerializeField] private Button button;
    [SerializeField] private ElementStyler backgroundStyler;
    
    [SerializeField] private DayOfWeek dayOfWeek;
    [SerializeField] private string dateText;
    
    public DateTime DateTime { get; set; }
    public UnityEvent OnClick => button.onClick;

    public void SetDate(DateTime dateTime)
    {
        DateTime = dateTime;
        weekDay.text = dateTime.DayOfWeek.ToString()[..3];
        day.text = dateTime.Day.ToString();

        dateText = dateTime.ToShortDateString();
        dayOfWeek = dateTime.DayOfWeek;

        name = dateText;
    }
    
    public void SetDayColorGradient(TMP_ColorGradient colorGradient)
    {
        day.colorGradientPreset = colorGradient;
    }
    
    public void SetDayOfWeekColorGradient(TMP_ColorGradient colorGradient)
    {
        weekDay.colorGradientPreset = colorGradient;
    }
    
    public void SetBackgroundColorGradient(TMP_ColorGradient colorGradient)
    {
        backgroundStyler.SetStyle(colorGradient);
    }
}