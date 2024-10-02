using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEngine;

public class GoalsRepository : MonoBehaviour
{
    [SerializeField] private string goalsDirectory;
    [SerializeField] private string directoryName = "MyGoals";

    [SerializeField] private List<GoalData> goals;
    
    public void Initialize()
    {
        goalsDirectory = Path.Combine(Application.persistentDataPath, directoryName);
        Directory.CreateDirectory(goalsDirectory);
    }
    
    public void SaveGoal(GoalData goal)
    {
        if (File.Exists(goal.FilePath))
        {
            EditGoal(goal);
        }
        
        var title = goal.Title.Trim();
        
        var sanitizedTitle = SanitizeFileName(title);
        
        if (string.IsNullOrEmpty(sanitizedTitle) || string.IsNullOrWhiteSpace(sanitizedTitle))
        {
            sanitizedTitle = goal.Date.ToShortDateString() + " - New Goal";
        }

        if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title))
        {
            goal.Title = sanitizedTitle;
        }
        
        goal.FilePath = Path.Combine(goalsDirectory, sanitizedTitle + ".json");

        SaveGoalToFile(goal.FilePath, goal);
    }

    public static void DeleteGoal(string filePath)
    {
        File.Delete(filePath);
        Debug.Log($"Goal deleted: {filePath}");
    }

    public List<GoalData> GetGoalsByDate(DateTime date)
    {
        var goalList = LoadGoals().
            Where(s => s.Date.ToShortDateString() == date.ToShortDateString());
        return goalList.ToList();
    }

    public List<GoalData> GetGoalsByCategory(string category)
    {
        var goalList = LoadGoals().
            Where(s => s.Category == category);
        return goalList.ToList();
    }

    public List<GoalData> GetGoalsByTitle(string title)
    {
        var goalList = LoadGoals().
            Where(s => s.Title.Contains(title));
        return goalList.ToList();
    }

    /// <summary>
    /// Если и title и category навалидны, возвращает все цели
    /// </summary>
    /// <param name="searchPattern"></param>
    /// <returns></returns>
    public List<GoalData> GetGoalsByPattern(GoalData searchPattern)
    {
        var title = searchPattern.Title;
        var category = searchPattern.Category;

        var searchByTitle = !string.IsNullOrEmpty(title) && !string.IsNullOrWhiteSpace(title);
        var searchByCategory = !string.IsNullOrEmpty(category) && !string.IsNullOrWhiteSpace(category);

        if (searchByCategory && searchByTitle)
        {
            var goalList = LoadGoals().
                Where(s => s.Title.Contains(searchPattern.Title)).
                Where(s => s.Category == searchPattern.Category);
            return goalList.ToList();
        }
        if (searchByCategory)
        {
            return GetGoalsByCategory(category);
        }
        if (searchByTitle)
        {
            return GetGoalsByTitle(title);
        }
        
        return null;
    }

    public List<GoalData> GetAllGoals()
    {
        return LoadGoals().ToList();
    }

    private IEnumerable<GoalData> LoadGoals()
    {
        goals = new List<GoalData>();

        if (!Directory.Exists(goalsDirectory))
        {
            Debug.LogWarning($"Directory for goals does not exist: {goalsDirectory}");
            return goals;
        }
        
        var files = Directory.GetFiles(goalsDirectory, "*.json");
        
        foreach (var file in files)
        {
            var json = File.ReadAllText(file);
            
            var goal = JsonConvert.DeserializeObject<GoalData>(json);

            if (goal != null)
            {
                goals.Add(goal);
            }
            else
            {
                Debug.LogWarning($"Failed to load goal from file: {file}");
            }
        }

        return goals;
    }

    private void EditGoal(GoalData goal)
    {
        DeleteGoal(goal.FilePath);
        var newGoalFilePath = Path.Combine(goalsDirectory, goal.Title + ".json");
        goal.FilePath = newGoalFilePath;
        SaveGoal(goal);
    }
    
    private static void SaveGoalToFile(string filePath, GoalData goal)
    {
        var json = JsonConvert.SerializeObject(goal, Formatting.Indented);
        File.WriteAllText(filePath, json);
        Debug.Log($"Goal saved: {filePath}");
    }

    private static string SanitizeFileName(string name)
    {
        var invalidChars = new string(Path.GetInvalidFileNameChars());
        var invalidRegex = $"[{Regex.Escape(invalidChars)}]";
        return Regex.Replace(name, invalidRegex, "_");
    }
}