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
        AttackSkillNode skillA = new AttackSkillNode("Skill A");
        AttackSkillNode skillB = new AttackSkillNode("Skill B");
        AttackSkillNode skillC = new AttackSkillNode("Skill C");

        skillB.prerequisites.Add(skillA); // Skill B�� Skill A�� ���� ��ų�� ����
        skillC.prerequisites.Add(skillB); // Skill C�� Skill B�� ���� ��ų�� ����

        allSkills = new List<AttackSkillNode> { skillA, skillB, skillC };
    }

    public bool CanLevelUp(AttackSkillNode skill)
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
}
