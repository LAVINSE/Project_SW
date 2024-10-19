using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class swUtilsDataBaseSO : ScriptableObject
{
    public abstract string SheetName { get; }
    public abstract void LoadCSVData(Dictionary<string, Dictionary<string, string>> dataDict);

    protected void LoadErrorLog(string id, Exception e)
    {
        Debug.LogError($"Error parsing row with ID {id}: {e.Message}");
    }

    protected void LoadCompleteLog<T>(List<T> dataList)
    {
        Debug.Log($"Loaded {dataList.Count} {typeof(T).Name} from CSV");
    }
}
