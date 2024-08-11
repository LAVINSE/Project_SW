using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "DefenseGame/Data/Enemy")]
public class EnemyDataSO : ScriptableObject
{
    #region 변수
    [SerializeField] private string enemyName;
    [SerializeField] private int enemyId;
    [SerializeField] private GameObject enemyPrefab;
    #endregion // 변수

    #region 프로퍼티
    public string EnemayName => enemyName;
    public int EnemyId => enemyId;
    public GameObject EnemyPrefab => enemyPrefab;
    #endregion // 프로퍼티
}
