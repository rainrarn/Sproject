using System.Collections.Generic;
using UnityEngine;

public class AttackSkillTree : MonoBehaviour
{
    public List<AttackSkillNode> allSkills;

    void Start()
    {
        InitializeSkillTree();
    }

    void InitializeSkillTree()
    {
        AttackSkillNode skillA = new AttackSkillNode("Skill A");
        AttackSkillNode skillB = new AttackSkillNode("Skill B");
        AttackSkillNode skillC = new AttackSkillNode("Skill C");
        AttackSkillNode skillD = new AttackSkillNode("Skill D");
        AttackSkillNode skillE = new AttackSkillNode("Skill E");
        AttackSkillNode skillF = new AttackSkillNode("Skill F");
        AttackSkillNode skillG = new AttackSkillNode("Skill G");
        AttackSkillNode skillH = new AttackSkillNode("Skill H");

        skillB.prerequisites.Add(skillA); // Skill B는 Skill A를 선행 스킬로 가짐
        skillC.prerequisites.Add(skillA);
        skillD.prerequisites.Add(skillB);
        skillE.prerequisites.Add(skillB);
        skillF.prerequisites.Add(skillC);
        skillG.prerequisites.Add(skillC);

        allSkills = new List<AttackSkillNode> { skillA, skillB, skillC, skillD, skillE, skillF, skillG, skillH };
    }

    public bool CanLevelUp(AttackSkillNode skill)
    {
        foreach (var prerequisite in skill.prerequisites)
        {
            if (!prerequisite.isUnlocked)
            {
                return false; // 선행 스킬 중 하나라도 잠금 해제되지 않았다면 false 반환
            }
        }
        return true; // 모든 선행 스킬이 잠금 해제된 경우 true 반환
    }

    public void LevelUpSkill(AttackSkillNode skill)
    {
        if (CanLevelUp(skill))
        {
            skill.LevelUp(); // 스킬 레벨업
        }
        else
        {
            Debug.Log(skill.skillName + " cannot be leveled up yet.");
        }
    }
    public void LevelDownSkill(AttackSkillNode skill)
    {
        skill.LevelDown(); // 스킬 레벨 다운
    }
}
