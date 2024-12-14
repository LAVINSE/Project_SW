using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CSVLoader", menuName = "SwUtils/DataLoader/CSVLoader")]
public class SwUtilsCSVLoader : ScriptableObject
{ 
    // 사용 예시
    // Dictionary<string, string> data = GetDataById(sheetMappings[0].sheetName, "1");
    // Debug.Log($"{data["ID"]} {data["이름"]} {data["HP"]} {data["MP"]}");

    #region 변수
    [System.Serializable]
    public class SheetMapping
    {
        public string sheetName;
        public TextAsset csvFile;
    }

    [SerializeField] private List<SheetMapping> sheetMappings = new List<SheetMapping>();
    [SerializeField] private List<SwUtilsDataBaseSO> databaseSOList = new List<SwUtilsDataBaseSO>();

    private Dictionary<string, Dictionary<string, Dictionary<string, string>>> excelSheetData =
        new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
    #endregion // 변수

    #region 함수
    /** 데이터 초기 설정을 한다 */
    public void InitializeAllSheetData()
    {
        excelSheetData.Clear();
        foreach (var mapping in sheetMappings)
        {
            if (mapping.csvFile != null)
            {
                LoadCSVDataFromTextAsset(mapping.sheetName, mapping.csvFile);
                var database = databaseSOList.Find(x => x.SheetName == mapping.sheetName);
                if (database != null)
                {
                    database.LoadCSVData(GetDataSheet(database.SheetName));
                }
            }
            else
            {
                Debug.LogError($"CSV file not assigned for sheet: {mapping.sheetName}");
            }
        }
    }

    /** SheetName에 해당하는 모든 데이터를 가져온다 */
    public Dictionary<string, Dictionary<string, string>> GetDataSheet(string sheetName)
    {
        if (excelSheetData.TryGetValue(sheetName, out var sheet))
            return sheet;
        else
            return null;
    }

    /** ID에 해당하는 모든 데이터를 가져온다 */
    public Dictionary<string, string> GetDataById(string sheetName, string id)
    {
        if (excelSheetData.TryGetValue(sheetName, out var sheet))
        {
            // ID 열을 찾아서 해당 ID의 데이터를 반환
            foreach (var row in sheet)
            {
                if (row.Value.TryGetValue("ID", out string rowId) && rowId == id)
                {
                    return row.Value;
                }
            }
        }
        Debug.LogWarning($"Data not found for sheet: {sheetName}, ID: {id}");
        return null;
    }

    /** ID에 해당하는 특정 데이터를 가져온다 */
    public string GetValueById(string sheetName, string id, string columnKey)
    {
        var rowData = GetDataById(sheetName, id);
        if (rowData != null && rowData.TryGetValue(columnKey, out string value))
        {
            return value;
        }
        Debug.LogWarning($"Column {columnKey} not found for ID {id} in sheet {sheetName}");
        return null;
    }

    /** CSV 데이터 파일을 로드한다 */
    private void LoadCSVDataFromTextAsset(string sheetName, TextAsset csvFile)
    {
        try
        {
            string csvText = DetectAndConvertEncoding(csvFile.bytes);
            Dictionary<string, Dictionary<string, string>> sheetData =
                new Dictionary<string, Dictionary<string, string>>();

            using (StringReader reader = new StringReader(csvText))
            {
                // 헤더 읽기
                string headerLine = reader.ReadLine();
                if (headerLine == null) return;

                string[] headers = headerLine.Split(',');
                for (int i = 0; i < headers.Length; i++)
                {
                    headers[i] = headers[i].Trim();
                }

                // 데이터 행 읽기
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // 빈 행 건너뛰기
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] values = line.Split(',');
                    if (values.Length > 0)
                    {
                        Dictionary<string, string> rowData = new Dictionary<string, string>();
                        bool hasValidData = false;

                        // 모든 컬럼에 대해 데이터 저장
                        for (int i = 0; i < values.Length && i < headers.Length; i++)
                        {
                            string value = values[i].Trim();
                            rowData[headers[i]] = value;
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                hasValidData = true;
                            }
                        }

                        // 유효한 데이터가 있고 ID가 존재하는 경우에만 추가
                        if (hasValidData && rowData.ContainsKey("ID") && !string.IsNullOrWhiteSpace(rowData["ID"]))
                        {
                            string id = rowData["ID"];
                            sheetData[id] = rowData;
                        }
                    }
                }
            }

            excelSheetData[sheetName] = sheetData;
            Debug.Log($"Successfully loaded {sheetData.Count} rows from sheet: {sheetName}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading CSV file for sheet {sheetName}: {e.Message}");
        }
    }

    /** 파일의 인코딩을 감지하고 필요한 경우 UTF-8로 변환 */
    private string DetectAndConvertEncoding(byte[] fileBytes)
    {
        // UTF-8 BOM 확인
        if (fileBytes.Length >= 3 &&
            fileBytes[0] == 0xEF &&
            fileBytes[1] == 0xBB &&
            fileBytes[2] == 0xBF)
        {
            return Encoding.UTF8.GetString(fileBytes, 3, fileBytes.Length - 3);
        }

        // 인코딩 감지 시도
        Encoding[] encodingsToTry = {
            Encoding.UTF8,
            Encoding.GetEncoding(949), // EUC-KR
            Encoding.GetEncoding(51949), // EUC-KR
            Encoding.GetEncoding("euc-kr")
        };

        foreach (Encoding encoding in encodingsToTry)
        {
            try
            {
                string content = encoding.GetString(fileBytes);
                // UTF-8로 변환
                byte[] utf8Bytes = Encoding.Convert(encoding, Encoding.UTF8, fileBytes);
                return Encoding.UTF8.GetString(utf8Bytes);
            }
            catch (Exception)
            {
                continue;
            }
        }

        // 기본적으로 UTF-8 사용
        return Encoding.UTF8.GetString(fileBytes);
    }
    #endregion // 함수
}

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
