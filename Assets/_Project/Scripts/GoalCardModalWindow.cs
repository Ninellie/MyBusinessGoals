using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GoalCardModalWindow : MonoBehaviour
{
    [SerializeField] private Button editButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private GameObject modalWindow;

    public UnityEvent OnEdit => editButton.onClick;
    public UnityEvent OnDelete => deleteButton.onClick;

    private void OnDisable()
    {
        OnEdit.RemoveAllListeners();
        OnDelete.RemoveAllListeners();
    }

    public void Show()
    {
        modalWindow.SetActive(true);
    }

    public void Hide()
    {
        modalWindow.SetActive(false);
        OnEdit.RemoveAllListeners();
        OnDelete.RemoveAllListeners();
    }
}