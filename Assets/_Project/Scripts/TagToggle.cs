using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TagToggle : MonoBehaviour
{
    [SerializeField] private CalendarDayColorPreset selectedColors;
    [SerializeField] private CalendarDayColorPreset deselectedColors;
    
    [SerializeField] private TMP_Text text;
    [SerializeField] private ElementStyler backgroundStyler;
    [SerializeField] private UnityEvent<string> onSelect;

    public string TagText => text.text;
    
    public void Switch(bool isSelected)
    {
        if (isSelected)
        {
            Select();
            return;
        }
        Deselect();
    }

    private void Select()
    {
        text.colorGradientPreset = selectedColors.DayText;
        backgroundStyler.SetStyle(selectedColors.Background);
        onSelect.Invoke(text.text);
    }

    private void Deselect()
    {
        text.colorGradientPreset = deselectedColors.DayText;
        backgroundStyler.SetStyle(deselectedColors.Background);
    }
}