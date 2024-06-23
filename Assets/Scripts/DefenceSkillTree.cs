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

        skillB.prerequisites.Add(skillA); // Skill B�� Skill A�� ���� ��ų�� ����
        skillC.prerequisites.Add(skillB); // Skill C�� Skill B�� ���� ��ų�� ����

        allSkills = new List<DefenceSkillNode> { skillA, skillB, skillC };
    }

    public bool CanLevelUp(DefenceSkillNode skill)
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
}
