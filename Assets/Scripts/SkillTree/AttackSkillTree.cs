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
        AttackSkillNode skillA = new AttackSkillNode("StrongStrike");
        AttackSkillNode skillB = new AttackSkillNode("Sharpness");
        AttackSkillNode skillC = new AttackSkillNode("Crush");
        AttackSkillNode skillD = new AttackSkillNode("Continual");
        AttackSkillNode skillE = new AttackSkillNode("Violent");
        AttackSkillNode skillF = new AttackSkillNode("Perfect");
        AttackSkillNode skillG = new AttackSkillNode("Berserek");
        AttackSkillNode skillH = new AttackSkillNode("Master");

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

        allSkills = new List<AttackSkillNode> { skillA, skillB, skillC, skillD, skillE, skillF, skillG, skillH };
    }

    public bool CanLevelUp(AttackSkillNode skill)
    {
        if (skill.skillName == "Master")
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
