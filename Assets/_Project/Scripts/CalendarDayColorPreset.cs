using TMPro;
using UnityEngine;

[CreateAssetMenu]
public class CalendarDayColorPreset : ScriptableObject
{
    [SerializeField] private TMP_ColorGradient background;
    [SerializeField] private TMP_ColorGradient dayText;
    [SerializeField] private TMP_ColorGradient dayOfWeek;

    public TMP_ColorGradient Background => background;
    public TMP_ColorGradient DayText => dayText;
    public TMP_ColorGradient DayOfWeek => dayOfWeek;
}