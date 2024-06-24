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

    public void LevelUpSkill(DefenceSkillNode skill)
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
    public void LevelDownSkill(DefenceSkillNode skill)
    {
        skill.LevelDown(); // ��ų ���� �ٿ�
    }
}
