using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "DefenseGame/Data/Enemy")]
public class EnemyDataSO : ScriptableObject
{
    #region ����
    [SerializeField] private string enemyName;
    [SerializeField] private int enemyId;
    [SerializeField] private GameObject enemyPrefab;
    #endregion // ����

    #region ������Ƽ
    public string EnemayName => enemyName;
    public int EnemyId => enemyId;
    public GameObject EnemyPrefab => enemyPrefab;
    #endregion // ������Ƽ
}
