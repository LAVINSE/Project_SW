using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true)]
public class CustomEditorAttribute : Editor
{
    private Dictionary<string, object[]> parameterValues = new Dictionary<string, object[]>();

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MonoBehaviour mono = (MonoBehaviour)target;
        var type = mono.GetType();
        var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var method in methods)
        {
            var buttonAttributes = method.GetCustomAttributes(typeof(ButtonAttribute), false);
            if (buttonAttributes.Length > 0)
            {
                var buttonAttribute = buttonAttributes[0] as ButtonAttribute;

                if (buttonAttribute.Space > 0)
                {
                    GUILayout.Space(buttonAttribute.Space);
                }

                string buttonName = buttonAttribute.DisplayName ?? method.Name;

                var parameters = method.GetParameters();

                if (parameters.Length > 0)
                {
                    GUILayout.Space(10);
                    if (!parameterValues.ContainsKey(method.Name))
                    {
                        parameterValues[method.Name] = new object[parameters.Length];
                    }

                    GUILayout.Label($"{method.Name}:", EditorStyles.boldLabel);
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var param = parameters[i];
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(param.Name, GUILayout.Width(100));

                        if (param.ParameterType == typeof(int))
                        {
                            parameterValues[method.Name][i] = EditorGUILayout.IntField((int)(parameterValues[method.Name][i] ?? 0));
                        }
                        else if (param.ParameterType == typeof(float))
                        {
                            parameterValues[method.Name][i] = EditorGUILayout.FloatField((float)(parameterValues[method.Name][i] ?? 0f));
                        }
                        else if (param.ParameterType == typeof(string))
                        {
                            parameterValues[method.Name][i] = EditorGUILayout.TextField((string)(parameterValues[method.Name][i] ?? ""));
                        }
                        else if (param.ParameterType == typeof(bool))
                        {
                            parameterValues[method.Name][i] = EditorGUILayout.Toggle((bool)(parameterValues[method.Name][i] ?? false));
                        }
                        else
                        {
                            GUILayout.Label($"Unsupported type: {param.ParameterType.Name}");
                        }
                        GUILayout.EndHorizontal();
                    }
                }

                if (GUILayout.Button(buttonName, GUILayout.Height(35f)))
                {
                    if (parameters.Length > 0)
                    {
                        try
                        {
                            method.Invoke(mono, parameterValues[method.Name]);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"Error invoking {method.Name}: {e.Message}");
                        }
                    }
                    else
                    {
                        method.Invoke(mono, null);
                    }
                }
            }
        }
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : PropertyAttribute
{
    public string DisplayName { get; private set; }
    public float Space { get; private set; }

    public ButtonAttribute(string displayName = null)
    {
        DisplayName = displayName;
        Space = 0f;
    }

    public ButtonAttribute(float space)
    {
        DisplayName = null;
        Space = space;
    }

    public ButtonAttribute(string displayName, float space)
    {
        DisplayName = displayName;
        Space = space;
    }
}

[System.AttributeUsage(System.AttributeTargets.Field)]
public class ReadOnlyAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
