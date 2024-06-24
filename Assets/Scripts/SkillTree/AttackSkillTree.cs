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

        skillB.prerequisites.Add(skillA); // Skill B�� Skill A�� ���� ��ų�� ����
        skillC.prerequisites.Add(skillA);
        skillD.prerequisites.Add(skillB);
        skillE.prerequisites.Add(skillB);
        skillF.prerequisites.Add(skillC);
        skillG.prerequisites.Add(skillC);

        // Skill H�� Skill D, E, F, G �� �ϳ��� ��� �����Ǹ� ����
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
                    return true; // Skill H�� D, E, F, G �� �ϳ��� ��� �����Ǹ� true ��ȯ
                }
            }
            return false;
        }
        foreach (var prerequisite in skill.prerequisites)
        {
            if (!prerequisite.isUnlocked)
            {
                return false; // ���� ��ų �� �ϳ��� ��� �������� �ʾҴٸ� false ��ȯ
            }
        }
        return true; // ��� ���� ��ų�� ��� ������ ��� true ��ȯ
    }

    public void LevelUpSkill(AttackSkillNode skill)
    {
        if (CanLevelUp(skill))
        {
            skill.LevelUp(); // ��ų ������
        }
        else
        {
            Debug.Log(skill.skillName + " cannot be leveled up yet.");
        }
    }
    public void LevelDownSkill(AttackSkillNode skill)
    {
        skill.LevelDown(); // ��ų ���� �ٿ�
    }
}
