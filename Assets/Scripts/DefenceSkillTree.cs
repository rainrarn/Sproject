using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceSkillTree : MonoBehaviour
{
    public List<DefenceSkillNode> allSkills;

    void Start()
    {
        InitializeSkillTree();
    }

    void InitializeSkillTree()
    {
        DefenceSkillNode skillA = new DefenceSkillNode("Skill A");
        DefenceSkillNode skillB = new DefenceSkillNode("Skill B");
        DefenceSkillNode skillC = new DefenceSkillNode("Skill C");
        DefenceSkillNode skillD = new DefenceSkillNode("Skill D");
        DefenceSkillNode skillE = new DefenceSkillNode("Skill E");
        DefenceSkillNode skillF = new DefenceSkillNode("Skill F");
        DefenceSkillNode skillG = new DefenceSkillNode("Skill G");
        DefenceSkillNode skillH = new DefenceSkillNode("Skill H");

        skillB.prerequisites.Add(skillA); // Skill B는 Skill A를 선행 스킬로 가짐
        skillC.prerequisites.Add(skillA);
        skillD.prerequisites.Add(skillB);
        skillE.prerequisites.Add(skillB);
        skillF.prerequisites.Add(skillC);
        skillG.prerequisites.Add(skillC);

        allSkills = new List<DefenceSkillNode> { skillA, skillB, skillC, skillD, skillE, skillF, skillG, skillH };
    }

    public bool CanLevelUp(DefenceSkillNode skill)
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

    public void LevelUpSkill(DefenceSkillNode skill)
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
}
