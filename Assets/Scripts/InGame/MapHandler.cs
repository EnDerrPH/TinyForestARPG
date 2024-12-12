using UnityEngine;

public class MapHandler : MonoBehaviour
{
    [SerializeField] EnemyData _enemyData;
    [SerializeField] private int _maximumEnemyCount;

    public int MaximumEnemyCount => _maximumEnemyCount;
    public EnemyData EnemyData => _enemyData;
}
