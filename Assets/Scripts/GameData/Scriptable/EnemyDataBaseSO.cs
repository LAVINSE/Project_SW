using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataBase", menuName = "DataBase")]
public class EnemyDataBaseSO : swUtilsDataBaseSO
{
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

    [SerializeField] private string sheetName;
    [SerializeField] private List<EnemyData> enemyDataList;

    public override string SheetName => sheetName;

    public override void LoadCSVData(Dictionary<string, Dictionary<string, string>> dataDict)
    {
        enemyDataList.Clear();

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
                    maxHp = int.Parse(row["HP"]),
                    attack = int.Parse(row["BASIC_ATTACK"]),
                    skillActiveName = row["SKILL_ACTIVE"],
                    skillPassiveName = row["SKILL_PASSIVE"]
                };

                enemyDataList.Add(enemy);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error parsing row with ID {row["ID"]}: {e.Message}");
            }
        }

        Debug.Log($"Loaded {enemyDataList.Count} enemies from CSV.");
    }
}