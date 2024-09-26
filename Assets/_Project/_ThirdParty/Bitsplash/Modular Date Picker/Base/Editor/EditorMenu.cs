using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorMenu
{
    private static void InstanciateCanvas(string path)
    {
        Canvas[] canvases = GameObject.FindObjectsOfType<Canvas>();
        if (canvases == null || canvases.Length == 0)
        {
            EditorUtility.DisplayDialog("No canvas in scene", "Please add a canvas to the scene and try again", "Ok");
            return;
        }
        Canvas canvas = null;
        foreach (Canvas c in canvases)
        {
            if (c.transform.parent == null)
            {
                canvas = c;
                break;
            }
        }

        if (canvas == null)
        {
            EditorUtility.DisplayDialog("No canvas in scene", "Please add a canvas to the scene and try again", "Ok");
            return;
        }
        GameObject obj = Resources.Load<GameObject>(path);
        GameObject newObj = (GameObject)GameObject.Instantiate(obj);
        newObj.transform.SetParent(canvas.transform, false);
        newObj.name = newObj.name.Replace("(Clone)", "");
        Undo.RegisterCreatedObjectUndo(newObj, "Create Object");
    }


    [MenuItem("Tools/Date Picker/Light Theme")]
    public static void AddLightTheme()
    {
        InstanciateCanvas("MenuPrefabs/MenuLight");
    }

    [MenuItem("Tools/Date Picker/Dark Theme")]
    public static void AddDarkTheme()
    {
        InstanciateCanvas("MenuPrefabs/MenuDark");
    }
}
