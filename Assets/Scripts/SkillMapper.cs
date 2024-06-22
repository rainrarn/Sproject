using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSkill
{
    public string SkillName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float BaseDamaged { get; set; }
    public float DamageMultiSkillLevelName { get; set; }
    
    public List<string> AttackSkillNameList { get; set; }
}
public class DefenceSkill
{
    public string SkillName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float BaseDamaged { get; set; }
    public float DamageMultiSkillLevelName { get; set; }

    public List<string> DefenceSkillNameList { get; set; }
}
public class UtillSkill
{
    public string SkillName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float BaseDamaged { get; set; }
    public float DamageMultiSkillLevelName { get; set; }

    public List<string> UtillSkillNameList { get; set; }
}
