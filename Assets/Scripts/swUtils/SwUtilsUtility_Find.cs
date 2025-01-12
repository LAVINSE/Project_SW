using UnityEditor;
using UnityEngine;

public static partial class SwUtilsUtility
{
    #region 함수
    public static T LoadScriptable<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T)}");

        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            var loadScriptable = AssetDatabase.LoadAssetAtPath<T>(assetPath);

            if (loadScriptable.GetType() == typeof(T))
            {
                return loadScriptable;
            }
        }

        return null;
    }
    #endregion // 함수
}
