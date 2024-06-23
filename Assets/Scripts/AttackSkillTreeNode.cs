using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AttackSkillNode
{
    public string skillName; // ��ų �̸�
    public int skillLevel; // ��ų ���� (�⺻���� 0)
    public List<AttackSkillNode> prerequisites; // ���� ��ų ���
    public bool isUnlocked; // ��ų ��� ���� ����

    public AttackSkillNode(string name)
    {
        skillName = name;
        skillLevel = 0;
        prerequisites = new List<AttackSkillNode>();
        isUnlocked = false;
    }

    public void Unlock()
    {
        isUnlocked = true;
    }

    public void LevelUp()
    {
        if (skillLevel < 3)
        {
            skillLevel++;
            Debug.Log(skillName + " leveled up to " + skillLevel);

            if (skillLevel == 3)
            {
                Unlock();
                Debug.Log(skillName + " is fully leveled up and unlocked!");
            }
        }
    }
    public void LevelDown()
    {
        if (skillLevel > 0)
        {
            skillLevel--;
            Debug.Log(skillName + " leveled down to " + skillLevel);
        }
    }
}


