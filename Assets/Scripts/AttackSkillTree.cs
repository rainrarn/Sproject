using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public List<SkillNode> allSkills;

    void Start()
    {
        InitializeSkillTree();
    }

    void InitializeSkillTree()
    {
        SkillNode skillA = new SkillNode("Skill A");
        SkillNode skillB = new SkillNode("Skill B");
        SkillNode skillC = new SkillNode("Skill C");

        skillB.prerequisites.Add(skillA); // Skill B�� Skill A�� ���� ��ų�� ����
        skillC.prerequisites.Add(skillB); // Skill C�� Skill B�� ���� ��ų�� ����

        allSkills = new List<SkillNode> { skillA, skillB, skillC };
    }

    public bool CanLevelUp(SkillNode skill)
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

    public void LevelUpSkill(SkillNode skill)
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
