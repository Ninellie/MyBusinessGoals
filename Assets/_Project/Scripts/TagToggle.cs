using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TagToggle : MonoBehaviour
{
    [SerializeField] private CalendarDayColorPreset selectedColors;
    [SerializeField] private CalendarDayColorPreset deselectedColors;
    
    [SerializeField] private ToggleGroup toggleGroup;
    
    [SerializeField] private TMP_Text text;
    [SerializeField] private ElementStyler iconStyler;
    
    [SerializeField] private ElementStyler backgroundStyler;
    [SerializeField] private UnityEvent<string> onSelect;
    [SerializeField] private UnityEvent onDeselectAll;

    public string TagText => text.text;

    public void Switch(bool isSelected)
    {
        if (isSelected)
        {
            Select();
            return;
        }
        Deselect();

        if (toggleGroup == null)
        {
            return;
        }
        
        if (!toggleGroup.AnyTogglesOn())
        {
            onDeselectAll.Invoke();
        }
    }

    private void Select()
    {
        text.colorGradientPreset = selectedColors.DayText;
        iconStyler.SetStyle(selectedColors.DayText);
        
        backgroundStyler.SetStyle(selectedColors.Background);
        
        onSelect.Invoke(text.text);
    }

    private void Deselect()
    {
        text.colorGradientPreset = deselectedColors.DayText;
        iconStyler.SetStyle(deselectedColors.DayText);
        
        backgroundStyler.SetStyle(deselectedColors.Background);
    }
}