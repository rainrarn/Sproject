using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillNode
{
    public string skillName; // 스킬 이름
    public int skillLevel; // 스킬 레벨 (기본값은 0)
    public List<SkillNode> prerequisites; // 선행 스킬 목록
    public bool isUnlocked; // 스킬 잠금 해제 여부

    public SkillNode(string name)
    {
        skillName = name;
        skillLevel = 0;
        prerequisites = new List<SkillNode>();
        isUnlocked = false;
    }


}


