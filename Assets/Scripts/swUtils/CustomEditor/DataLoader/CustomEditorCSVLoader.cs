#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SwUtilsCSVLoader))]
public class CustomEditorCSVLoader : Editor
{

    public override void OnInspectorGUI()
    {
        SwUtilsCSVLoader manager = (SwUtilsCSVLoader)target;
        DrawDefaultInspector();
        EditorGUILayout.Space();

        if (GUILayout.Button("Reload All Sheets"))
        {
            manager.InitializeAllSheetData();
        }

        GUIStyle customGUIStyle = new GUIStyle(GUI.skin.box
            )
        {
            padding = new RectOffset(15, 15, 15, 15),
            margin = new RectOffset(0, 0, 5, 5)
        };

        GUILayout.Space(10);

        EditorGUILayout.BeginVertical(customGUIStyle);
        EditorGUILayout.LabelField
            (
            "1. Sheet Mappings에 시트 이름과 CSV 파일을 할당하세요.\n" +
            "2. GetValueById(시트이름, ID, 컬럼명)으로 특정 ID의 컬럼 데이터를 가져올 수 있습니다." +
            "3. GetDataById(시트이름, ID)으로 특정 ID의 모든 데이터를 가져올 수 있습니다.",
            EditorStyles.wordWrappedLabel);
        EditorGUILayout.EndVertical();
    }
}
#endif // UNITY_EDITOR