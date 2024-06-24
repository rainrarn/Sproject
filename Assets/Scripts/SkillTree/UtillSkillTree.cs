using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtillSkillTree : MonoBehaviour
{
    public List<UtillSkillTreeNode> allSkills;

    void Start()
    {
        InitializeSkillTree();
    }

    void InitializeSkillTree()
    {
        UtillSkillTreeNode skillA = new UtillSkillTreeNode("Nimble");
        UtillSkillTreeNode skillB = new UtillSkillTreeNode("Restore");
        UtillSkillTreeNode skillC = new UtillSkillTreeNode("Prepare");
        UtillSkillTreeNode skillD = new UtillSkillTreeNode("Accel");
        UtillSkillTreeNode skillE = new UtillSkillTreeNode("Chance");
        UtillSkillTreeNode skillF = new UtillSkillTreeNode("Dejavu");
        UtillSkillTreeNode skillG = new UtillSkillTreeNode("Pathfinder");
        UtillSkillTreeNode skillH = new UtillSkillTreeNode("IronWill");

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

        allSkills = new List<UtillSkillTreeNode> { skillA, skillB, skillC, skillD, skillE, skillF, skillG, skillH };
    }

    public bool CanLevelUp(UtillSkillTreeNode skill)
    {
        if (skill.skillName == "IronWill")
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

    public void LevelUpSkill(UtillSkillTreeNode skill)
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
    public void LevelDownSkill(UtillSkillTreeNode skill)
    {
        skill.LevelDown(); // ��ų ���� �ٿ�
    }
}
