using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class GoalCategoryProperties : ScriptableObject
{
    [SerializeField] private List<CategoryProperties> categoryPropertiesList;

    public Sprite GetIcon(string categoryName)
    {
        var categoryProperties =categoryPropertiesList.
            Where(s => string.Equals(s.Title, categoryName, 
                StringComparison.CurrentCultureIgnoreCase)).ToArray();

        return !categoryProperties.Any() ? null : categoryProperties.First().Icon;
    }
}