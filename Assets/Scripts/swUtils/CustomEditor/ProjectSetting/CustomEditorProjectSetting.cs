using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomEditorSymbolSO))]
public class CustomEditorSymbol : Editor
{
    #region 함수
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var projectSettingSO = (CustomEditorSymbolSO)target;
        var addSymbols = projectSettingSO.SymBols;

        if (GUILayout.Button("SymBols Update", GUILayout.Height(50)))
        {
            UpdateScriptingDefineSymbols(addSymbols);
        }   

        if(GUILayout.Button("SymBols Reset", GUILayout.Height(50)))
        {
            ResetScriptingDefineSymbols(addSymbols);
        }
    }

    private void UpdateScriptingDefineSymbols(IReadOnlyList<string> newSymbols)
    {
        BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

        List<string> symbolsList = new List<string>(currentSymbols.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

        bool changed = false;
        foreach (string symbol in newSymbols)
        {
            if (!symbolsList.Contains(symbol))
            {
                symbolsList.Add(symbol);
                changed = true;
            }
        }

        if (changed)
        {
            string updatedSymbols = string.Join(";", symbolsList);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, updatedSymbols);
            Debug.Log("Scripting Define Symbols updated: " + updatedSymbols);
        }
        else
        {
            Debug.Log("No new symbols to add.");
        }
    }

    private void ResetScriptingDefineSymbols(IReadOnlyList<string> symbolsToRemove)
    {
        BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string currentSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);

        List<string> symbolsList = new List<string>(currentSymbols.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));

        bool changed = false;
        foreach (string symbol in symbolsToRemove)
        {
            if (symbolsList.Remove(symbol))
            {
                changed = true;
            }
        }

        if (changed)
        {
            string updatedSymbols = string.Join(";", symbolsList);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, updatedSymbols);
            Debug.Log("Scripting Define Symbols reset: " + updatedSymbols);
        }
        else
        {
            Debug.Log("No symbols to remove.");
        }
    }
    #endregion // 함수
}
