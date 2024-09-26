using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GoalDisplayScroll : MonoBehaviour
{
    [SerializeField] private GameObject goalCardPrefab;
    [SerializeField] private RectTransform content;
    
    [SerializeField] private GoalCardModalWindow modalWindow;
    
    [SerializeField] private UnityEvent<GoalData> onEdit;
    [SerializeField] private UnityEvent<GoalData> onDelete;

    public UnityEvent<GoalData> OnEdit => onEdit;
    public UnityEvent<GoalData> OnDelete => onDelete;

    private List<GoalCard> _cards;

    public void Initialize()
    {
        _cards = new List<GoalCard>();
    }

    public void Display(List<GoalData> goals)
    {
        Clear();

        foreach (var goalData in goals)
        {
            var card = Instantiate(goalCardPrefab, content).GetComponent<GoalCard>();
            card.DisplayGoal(goalData);
            card.OnOpenMenu.AddListener(()=>ShowModalWindow(goalData));
            _cards.Add(card);
        }
    }

    public void Clear()
    {
        if (_cards == null)
        {
            return;
        }

        if (_cards.Count == 0)
        {
            return;
        }
        
        foreach (var cardGameObject in _cards.Select(card => card.gameObject))
        {
            cardGameObject.SetActive(false);
            Destroy(cardGameObject);
        }
        
        _cards.Clear();
    }

    private void ShowModalWindow(GoalData goalData)
    {
        modalWindow.OnEdit.AddListener(() => OnEdit.Invoke(goalData));
        modalWindow.OnDelete.AddListener(() => OnDelete.Invoke(goalData));
        modalWindow.Show();
    }
}
