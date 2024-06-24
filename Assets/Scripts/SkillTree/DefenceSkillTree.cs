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
        DefenceSkillNode skillA = new DefenceSkillNode("Patience");
        DefenceSkillNode skillB = new DefenceSkillNode("Endurance");
        DefenceSkillNode skillC = new DefenceSkillNode("Protect");
        DefenceSkillNode skillD = new DefenceSkillNode("Persistent");
        DefenceSkillNode skillE = new DefenceSkillNode("Toughness");
        DefenceSkillNode skillF = new DefenceSkillNode("Dodge");
        DefenceSkillNode skillG = new DefenceSkillNode("Castle");
        DefenceSkillNode skillH = new DefenceSkillNode("Impregnable");

        skillB.prerequisites.Add(skillA); // Skill B는 Skill A를 선행 스킬로 가짐
        skillC.prerequisites.Add(skillA);
        skillD.prerequisites.Add(skillB);
        skillE.prerequisites.Add(skillB);
        skillF.prerequisites.Add(skillC);
        skillG.prerequisites.Add(skillC);

        // Skill H는 Skill D, E, F, G 중 하나라도 잠금 해제되면 열림
        skillH.prerequisites.Add(skillD);
        skillH.prerequisites.Add(skillE);
        skillH.prerequisites.Add(skillF);
        skillH.prerequisites.Add(skillG);

        allSkills = new List<DefenceSkillNode> { skillA, skillB, skillC, skillD, skillE, skillF, skillG, skillH };
    }

    public bool CanLevelUp(DefenceSkillNode skill)
    {
        if (skill.skillName == "Impregnable")
        {
            foreach (var prerequisite in skill.prerequisites)
            {
                if (prerequisite.isUnlocked)
                {
                    return true; // Skill H는 D, E, F, G 중 하나라도 잠금 해제되면 true 반환
                }
            }
            return false;
        }
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
    public void LevelDownSkill(DefenceSkillNode skill)
    {
        skill.LevelDown(); // 스킬 레벨 다운
    }
}
