using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CustomEditorCollectionData
{
    public string fileGuid;
    public string fileName;
    public List<string> referenceGids = new List<string>();
    public DateTime timeStamp;
}

[System.Serializable]
public class TypeDate
{
    public string guid;
    public string fileName;
    public DateTime timeStamp;
    public List<string> typeFullName = new List<string>();
    public string assemblly;

    public void Add(System.Type addtype)
    {
        assemblly = addtype.Assembly.FullName;
        var typeName = addtype.FullName;
        if (typeFullName.Contains(typeName) == false)
        {
            typeFullName.Add(typeName);
        }
    }

    public System.Type[] types
    {
        get
        {
            return typeFullName.Select(c => System.Type.GetType(c)).ToArray();
        }
    }
}

public interface IReferenceCollection
{
    void CollectionFiles();
    void Init(List<CustomEditorCollectionData> refs);
}
