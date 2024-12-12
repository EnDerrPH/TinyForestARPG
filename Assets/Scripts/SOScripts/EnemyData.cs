using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private int _HP;
    [SerializeField] private int _damage;
    [SerializeField] private int _defense;
    [SerializeField] private int _experience;
    [SerializeField] private Sprite _startingSprite;
    [SerializeField] private RuntimeAnimatorController _animatorController;

    public int HP { get => _HP; set { _HP = value; } }
    public int Damage { get => _damage; set { _damage = value; } }
    public int Defence { get => _defense; set { _defense = value; } }
    public int Experience { get => _experience; set { _experience = value; } }
    public Sprite StartingSprite => _startingSprite;
    public RuntimeAnimatorController AnimatorController => _animatorController;
}
