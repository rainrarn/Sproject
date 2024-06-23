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

        skillB.prerequisites.Add(skillA); // Skill B�� Skill A�� ���� ��ų�� ����
        skillC.prerequisites.Add(skillB); // Skill C�� Skill B�� ���� ��ų�� ����

        allSkills = new List<UtillSkillTreeNode> { skillA, skillB, skillC };
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
}
