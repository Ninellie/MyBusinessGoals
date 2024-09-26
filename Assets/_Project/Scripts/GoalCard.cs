using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GoalCard : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text purpose;
    [SerializeField] private TMP_Text category;
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text description;
    
    [SerializeField] private GoalCategoryProperties categoryProperties;
    
    [SerializeField] private Button menuButton;
    public UnityEvent OnOpenMenu => menuButton.onClick;

    public void DisplayGoal(GoalData goal)
    {
        title.text = goal.Title;
        purpose.text = goal.Purpose;
        category.text = goal.Category;
        SetIcon(goal.Category);
        description.text = goal.Description;
        
        Canvas.ForceUpdateCanvases();
    }

    private void SetIcon(string categoryName)
    {
        icon.sprite = categoryProperties.GetIcon(categoryName);
    }
}