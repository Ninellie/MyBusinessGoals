using System;
using UnityEngine;

[Serializable]
public class CategoryProperties
{
    [SerializeField] private string title;
    [SerializeField] private Sprite icon;

    public string Title => title;
    public Sprite Icon => icon;
}