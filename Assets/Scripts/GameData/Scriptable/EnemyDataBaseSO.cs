using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataBase", menuName = "DataBase/EnemyData")]
public class EnemyDataBaseSO : SwUtilsDataBaseSO
{
    #region 데이터
    [System.Serializable]
    public class EnemyData
    {
        public string monsterId;
        public string monsterName;
        public string rating;
        public string type;
        public int maxHp;
        public int attack;

        // 스킬 데이터는 따로 데이터베이스 받아오는 형식으로 바꿀 예정
        public string skillActiveName;
        public string skillPassiveName;
    }
    #endregion // 데이터

    #region 변수
    [SerializeField] private string sheetName;
    [SerializeField] private List<EnemyData> enemyDataList;
    #endregion // 변수

    #region 프로퍼티
    public override string SheetName => sheetName;
    public List<EnemyData> EnemyDataList => enemyDataList;
    #endregion // 프로퍼티

    #region 함수
    public override void LoadCSVData(Dictionary<string, Dictionary<string, string>> dataDict)
    {
        EnemyDataList.Clear();

        foreach (var row in dataDict.Values)
        {
            try
            {
                EnemyData enemy = new EnemyData
                {
                    monsterId = row["ID"], // ID는 첫 번째 열이라고 가정
                    monsterName = row["NAME"],
                    rating = row["RATING"],
                    type = row["TYPE"],
                    maxHp = int.TryParse(row["HP"], out int hp) ? hp : 0,
                    attack = int.TryParse(row["BASIC_ATTACK"], out int attack) ? attack : 0,
                    skillActiveName = row["SKILL_ACTIVE"],
                    skillPassiveName = row["SKILL_PASSIVE"]
                };

                EnemyDataList.Add(enemy);
            }
            catch (Exception e)
            {
                LoadErrorLog(row["ID"], e);
            }
        }

        LoadCompleteLog(enemyDataList);
    }
    #endregion // 함수
}