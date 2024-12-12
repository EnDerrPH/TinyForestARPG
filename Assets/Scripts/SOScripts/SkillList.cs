using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillList", menuName = "Scriptable Objects/SkillList", order = 1)]
public class SkillList : ScriptableObject
{
    [SerializeField] private List<Skill> SkillDataList = new List<Skill>();

}
