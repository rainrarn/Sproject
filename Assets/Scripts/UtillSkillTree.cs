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
        UtillSkillTreeNode skillA = new UtillSkillTreeNode("Skill A");
        UtillSkillTreeNode skillB = new UtillSkillTreeNode("Skill B");
        UtillSkillTreeNode skillC = new UtillSkillTreeNode("Skill C");
        UtillSkillTreeNode skillD = new UtillSkillTreeNode("Skill D");
        UtillSkillTreeNode skillE = new UtillSkillTreeNode("Skill E");
        UtillSkillTreeNode skillF = new UtillSkillTreeNode("Skill F");
        UtillSkillTreeNode skillG = new UtillSkillTreeNode("Skill G");
        UtillSkillTreeNode skillH = new UtillSkillTreeNode("Skill H");

        skillB.prerequisites.Add(skillA); // Skill B�� Skill A�� ���� ��ų�� ����
        skillC.prerequisites.Add(skillA);
        skillD.prerequisites.Add(skillB);
        skillE.prerequisites.Add(skillB);
        skillF.prerequisites.Add(skillC);
        skillG.prerequisites.Add(skillC);

        allSkills = new List<UtillSkillTreeNode> { skillA, skillB, skillC, skillD, skillE, skillF, skillG, skillH };
    }

    public bool CanLevelUp(UtillSkillTreeNode skill)
    {
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
