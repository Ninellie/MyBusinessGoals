using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace Bitsplash.DatePicker.Editors
{
    [CustomEditor(typeof(DatePickerSettings))]
    public class DatePickerSettingsEditor : Editor
    {
        void DoButton(SerializedObject serialized)
        {
            var im = serialized.FindProperty("TargetImage");
            if (im.objectReferenceValue != null)
            {
                var serilizedIm = new SerializedObject(im.objectReferenceValue);
                EditorGUILayout.LabelField("Button Image", EditorStyles.boldLabel);
                var imSprite = serilizedIm.FindProperty("m_Sprite");
                EditorGUILayout.PropertyField(imSprite);
                var imColor = serilizedIm.FindProperty("m_Color");
                EditorGUILayout.PropertyField(imColor);
                serilizedIm.ApplyModifiedProperties();
            }
            var tx = serialized.FindProperty("TargetText");
            if (tx.objectReferenceValue != null)
            {
                EditorGUILayout.LabelField("Button Text", EditorStyles.boldLabel);
                var serilizedTx = new SerializedObject(tx.objectReferenceValue);
                var itTx = serilizedTx.GetIterator();
                bool flagTx = true;
                while (itTx.NextVisible(flagTx))
                {
                    flagTx = false;
                    if (itTx.name.ToLower() == "m_script")
                        continue;
                    EditorGUILayout.PropertyField(itTx, includeChildren: true);
                }
                serilizedTx.ApplyModifiedProperties();
            }
        }
        void DoBackgroundSetting(UnityEngine.Object obj)
        {
            var mono = (obj as MonoBehaviour);
            if (mono == null)
                return;
            var image = mono.GetComponent<Image>();
            if (image == null)
                return;
            var serilizedIm = new SerializedObject(image);
            var imSprite = serilizedIm.FindProperty("m_Sprite");
            EditorGUILayout.PropertyField(imSprite);
            var imColor = serilizedIm.FindProperty("m_Color");
            EditorGUILayout.PropertyField(imColor);
            serilizedIm.ApplyModifiedProperties();

        }
        void DoCellTemplate(UnityEngine.Object obj)
        {
            var mono = (obj as MonoBehaviour);
            if (mono == null)
                return;
            var cell = mono.GetComponent<StandardDatePickerCell>();
            if (cell == null)
                return; 
            var serialized = new SerializedObject(cell);

            var it = serialized.GetIterator();
            bool flag = true;
            while (it.NextVisible(flag))
            {
                flag = false;
                if (it.name.ToLower() == "m_script")
                    continue;
                if (it.name.ToLower() == "textitem")
                    continue;
                if (it.name.ToLower() == "mark")
                    continue;
                if (it.name.ToLower() == "background")
                    continue;
                EditorGUILayout.PropertyField(it, includeChildren: true);
            }

            serialized.ApplyModifiedProperties();
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var picker = target as DatePickerSettings;
            foreach(var item in picker.GetComponentsInChildren<IDatePickerSettingsItem>(true).OrderBy(x=> x.Order))
            {
                var obj = (UnityEngine.Object)item;
                var serialized = new SerializedObject(obj);
                var openProp = serialized.FindProperty("isOpen");
                if (openProp == null)
                    continue;
                openProp.boolValue = EditorGUILayout.Foldout(openProp.boolValue, item.EditorTitle);
                if (openProp.boolValue)
                {
                    EditorGUI.indentLevel++;
                    if(item is DatePickerBackgroundSetting)
                    {
                        DoBackgroundSetting(obj);
                    }
                    if (item is DatePickerCellTemplate)
                    {
                        DoCellTemplate(obj);
                    }
                    else if (item is DatePickerButton)
                    {
                        DoButton(serialized);
                    }
                    else
                    {
                        var it = serialized.GetIterator();
                        bool flag = true;
                        while (it.NextVisible(flag))
                        {
                            flag = false;
                            if (it.name.ToLower() == "m_script")
                                continue;
                            if (it.name.ToLower() == "m_material")
                                continue;
                            if (it.name.ToLower() == "m_raycasttarget")
                                continue;
                            if (it.name.ToLower() == "cellprefab")
                                continue;
                            if (it.name.ToLower() == "totalrows")
                                continue;
                            if (it.name.ToLower() == "totalcolumns")
                                continue;
                            if (it.name.ToLower() == "texturetile")
                                continue;
                            if (it.name.ToLower() == "m_oncullstatechanged")
                                continue;
                            if (it.name.ToLower() == "targettext")
                                continue;
                            if (it.name.ToLower() == "targetimage")
                                continue;

                            EditorGUILayout.PropertyField(it, includeChildren: true);
                        }
                    }
                    EditorGUI.indentLevel--;
                }
                serialized.ApplyModifiedProperties();
            }
        }
    }
}
