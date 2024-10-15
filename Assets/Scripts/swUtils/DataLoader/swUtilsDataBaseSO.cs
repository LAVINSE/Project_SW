using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class swUtilsDataBaseSO : ScriptableObject
{
    public abstract string SheetName { get; }
    public abstract void LoadCSVData(Dictionary<string, Dictionary<string, string>> dataDict);
}
