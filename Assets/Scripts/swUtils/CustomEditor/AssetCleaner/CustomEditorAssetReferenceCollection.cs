using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CustomEditorAssetReferenceCollection : IReferenceCollection
{
    public void Init(List<CustomEditorCollectionData> refs)
    {
        references = refs;
    }

    private List<CustomEditorCollectionData> references = null;

    public void CollectionFiles()
    {
        var allFiles = Directory.GetFiles("Assets", "*.*", SearchOption.AllDirectories)
            .Where(c => Path.GetExtension(c) != ".meta")
            .Where(c => Path.GetExtension(c) != ".shader")
            .Where(c => Path.GetExtension(c) != ".cg")
            .Where(c => Path.GetExtension(c) != ".cginc")
            .Where(c => Path.GetExtension(c) != ".cs");

        foreach (var file in allFiles)
        {
            CollectionReferenceAssets(file);
        }
    }

    public void CollectionReferenceAssets(string path)
    {
        var guid = AssetDatabase.AssetPathToGUID(path);
        if (File.Exists(path) == false)
        {
            return;
        }

        var referenceFiles = AssetDatabase.GetDependencies(new string[] { path });
        List<string> referenceList = null;
        CustomEditorCollectionData reference = null;

        if (references.Exists(c => c.fileGuid == guid) == false)
        {
            referenceList = new List<string>();
            reference = new CustomEditorCollectionData()
            {
                fileGuid = guid,
                referenceGids = referenceList,
            };
            references.Add(reference);
        }
        else
        {
            reference = references.Find(c => c.fileGuid == guid);
            referenceList = reference.referenceGids;
        }
        if (string.IsNullOrEmpty(AssetDatabase.GUIDToAssetPath(guid)) == false)
        {
            reference.timeStamp = File.GetLastWriteTime(AssetDatabase.GUIDToAssetPath(guid));
        }

        foreach (var file in referenceFiles)
        {
            if (referenceList.Contains(file) == false)
                referenceList.Add(AssetDatabase.AssetPathToGUID(file));
        }
    }
}
