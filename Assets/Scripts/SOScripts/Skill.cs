using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill", order = 1)]
public class Skill : ScriptableObject
{
    [SerializeField] SkillClass _skillClass;
    [SerializeField] SkillType _skillType;
    [SerializeField] private int _levelRequired;
    [SerializeField] private Sprite _skillImage;
    [SerializeField] private float _SkillCooldown;
    [TextArea(10,10)]
    [SerializeField] private string _description;

    public SkillClass SkillClass => _skillClass;
    public SkillType SkillType => _skillType;
    public int LevelRequired => _levelRequired;
    public Sprite SkillImage => _skillImage;
    public float SkillCooldown => _SkillCooldown;
    public string Description => _description;
}