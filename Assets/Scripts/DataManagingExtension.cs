using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManigingExtensions
{
    public static AttackSkill GetAttackSkillData(this SkillDataManager manager, string dataName)
    {
        var loadedCharacterList = manager.LoadedAttackSkillList;
        if (loadedCharacterList.Count == 0
            || loadedCharacterList.ContainsKey(dataName) == false)
        {
            return null;
        }

        return loadedCharacterList[dataName];
    }

    public static DefenceSkill GetDefenceSkillData(this SkillDataManager manager, string dataName)
    {
        var loadedSkillList = manager.LoadedDefenceSkillList;
        if (loadedSkillList.Count == 0
            || loadedSkillList.ContainsKey(dataName) == false)
        {
            return null;
        }

        return loadedSkillList[dataName];
    }
    public static UtillSkill GetUtillSkillData(this SkillDataManager manager, string dataName)
    {
        var loadedSkillList = manager.LoadedUtillSkillList;
        if (loadedSkillList.Count == 0
            || loadedSkillList.ContainsKey(dataName) == false)
        {
            return null;
        }

        return loadedSkillList[dataName];
    }


    public static string GetAttackSkillName(this SkillDataManager manager, string dataClassName)
    {
        var skillData = manager.GetAttackSkillData(dataClassName);
        return (skillData != null) ? skillData.Name : string.Empty;
    }
    public static string GetDefenceSkillName(this SkillDataManager manager, string dataClassName)
    {
        var skillData = manager.GetDefenceSkillData(dataClassName);
        return (skillData != null) ? skillData.Name : string.Empty;
    }
    public static string GetUtillSkillName(this SkillDataManager manager, string dataClassName)
    {
        var skillData = manager.GetUtillSkillData(dataClassName);
        return (skillData != null) ? skillData.Name : string.Empty;
    }


    public static string GetAttackSkillDescription(this SkillDataManager manager, string dataClassName)
    {
        var buffData = manager.GetAttackSkillData(dataClassName);
        string desc = string.Empty;
        if (buffData != null)
        {
            desc = string.Format(buffData.Description);
        }
        return desc;
    }
    public static string GetDefenceSkillDescription(this SkillDataManager manager, string dataClassName)
    {
        var buffData = manager.GetDefenceSkillData(dataClassName);
        string desc = string.Empty;
        if (buffData != null)
        {
            desc = string.Format(buffData.Description);
        }
        return desc;
    }
    public static string GetUtillSkillDescription(this SkillDataManager manager, string dataClassName)
    {
        var buffData = manager.GetUtillSkillData(dataClassName);
        string desc = string.Empty;
        if (buffData != null)
        {
            desc = string.Format(buffData.Description);
        }
        return desc;
    }
}
