using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataBase", menuName = "DataBase/StageData")]
public class StageDataBaseSO : swUtilsDataBaseSO
{
    #region 데이터
    [System.Serializable]
    public class StageData
    {
        public int stageId;
        public int totalWave;
        public int waveId;
    }
    #endregion // 데이터

    #region 변수
    [SerializeField] private string sheetName;
    [SerializeField] private List<StageData> stageDataList;
    #endregion // 변수

    #region 프로퍼티
    public override string SheetName => sheetName;
    public List<StageData> StageDataList => stageDataList;
    #endregion // 프로퍼티

    #region 함수
    public override void LoadCSVData(Dictionary<string, Dictionary<string, string>> dataDict)
    {
        StageDataList.Clear();

        foreach (var row in dataDict.Values)
        {
            try
            {
                StageData enemy = new StageData
                {
                    stageId = int.TryParse(row["ID"], out int id) ? id : -1, // ID는 첫 번째 열이라고 가정
                    totalWave = int.TryParse(row["TOTAL_WAVE"], out int totalWave) ? totalWave : 0,
                    waveId = int.TryParse(row["WAVE_ID"], out int waveId) ? waveId : 0
                };

                StageDataList.Add(enemy);
            }
            catch (Exception e)
            {
                LoadErrorLog(row["ID"], e);
            }
        }

        LoadCompleteLog(StageDataList);
    }
    #endregion // 함수
}
