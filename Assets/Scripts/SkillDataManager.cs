using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class SkillDataManager : MonoBehaviour
{
    public static SkillDataManager Inst {  get; private set; }

    public Dictionary<string, AttackSkill> LoadedAttackSkillList {  get; private set; }
    public Dictionary<string, DefenceSkill> LoadedDefenceSkillList {  get; private set; }
    public Dictionary<string, UtillSkill> LoadedUtillSkillList { get; private set; }

    private readonly string _dataRootPath = " ";

    private void Awake()
    {
        Inst = this;
        ReadAllDataOnAwake();
    }

    private void ReadAllDataOnAwake()
    {
        ReadData(nameof(AttackSkill));
        ReadData(nameof(DefenceSkill));
        ReadData(nameof(UtillSkill));
    }

    private void ReadData(string tableName)
    {
        switch (tableName)
        {
            case "AttackSkill":
                ReadAttackSkillTable(tableName);
                break;
            case "DefenceSkill":
                ReadDefenceSkillTable(tableName);
                break;
            case "UtillSkill":
                ReadUtillSkillTable(tableName);
                break;
        }
    }

    private void ReadAttackSkillTable(string tableName)
    {
        LoadedAttackSkillList = new Dictionary<string, AttackSkill>();

        XDocument doc = XDocument.Load($"{_dataRootPath}/{tableName}.xml");
        var dataElements = doc.Descendants("data");

        foreach ( var data in dataElements )
        {
            var tempAttack = new AttackSkill();
            tempAttack.SkillName = data.Attribute("DataName").Value;
            tempAttack.Name = data.Attribute(nameof(tempAttack.Name)).Value;
            tempAttack.Description = data.Attribute(nameof(tempAttack.Description)).Value;
            tempAttack.BaseDamaged = float.Parse(data.Attribute(nameof(tempAttack.BaseDamaged)).Value);
            tempAttack.DamageMultiSkillLevelName =float.Parse(data.Attribute(nameof(tempAttack.DamageMultiSkillLevelName)).Value);

            string attackSkillNameListStr = data.Attribute(nameof(tempAttack.AttackSkillNameList)).Value;
            if(!string.IsNullOrEmpty(attackSkillNameListStr) )
            {
                attackSkillNameListStr = attackSkillNameListStr.Replace("{", string.Empty);
                attackSkillNameListStr = attackSkillNameListStr.Replace("}", string.Empty);

                var names = attackSkillNameListStr.Split(',');

                var list = new List<string>();
                if(names.Length > 0)
                {
                    foreach( var name in names )
                    {
                        list.Add(name);
                    }
                    tempAttack.AttackSkillNameList = list;
                }
                LoadedAttackSkillList.Add(tempAttack.SkillName, tempAttack);
            }

        }

        
    }

    private void ReadDefenceSkillTable(string tableName)
    {
        LoadedDefenceSkillList = new Dictionary<string, DefenceSkill>();

        XDocument doc = XDocument.Load($"{_dataRootPath}/{tableName}.xml");
        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            var tempDefence = new DefenceSkill();
            tempDefence.SkillName = data.Attribute("DataName").Value;
            tempDefence.Name = data.Attribute(nameof(tempDefence.Name)).Value;
            tempDefence.Description = data.Attribute(nameof(tempDefence.Description)).Value;
            tempDefence.BaseDamaged = float.Parse(data.Attribute(nameof(tempDefence.BaseDamaged)).Value);
            tempDefence.DamageMultiSkillLevelName = float.Parse(data.Attribute(nameof(tempDefence.DamageMultiSkillLevelName)).Value);

            string defenceSkillNameListStr = data.Attribute(nameof(tempDefence.DefenceSkillNameList)).Value;
            if (!string.IsNullOrEmpty(defenceSkillNameListStr))
            {
                defenceSkillNameListStr = defenceSkillNameListStr.Replace("{", string.Empty);
                defenceSkillNameListStr = defenceSkillNameListStr.Replace("}", string.Empty);

                var names = defenceSkillNameListStr.Split(',');

                var list = new List<string>();
                if (names.Length > 0)
                {
                    foreach (var name in names)
                    {
                        list.Add(name);
                    }
                    tempDefence.DefenceSkillNameList = list;
                }
                LoadedDefenceSkillList.Add(tempDefence.SkillName, tempDefence);
            }

        }


    }

    private void ReadUtillSkillTable(string tableName)
    {
        LoadedUtillSkillList = new Dictionary<string, UtillSkill>();

        XDocument doc = XDocument.Load($"{_dataRootPath}/{tableName}.xml");
        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            var tempUtill = new UtillSkill();
            tempUtill.SkillName = data.Attribute("DataName").Value;
            tempUtill.Name = data.Attribute(nameof(tempUtill.Name)).Value;
            tempUtill.Description = data.Attribute(nameof(tempUtill.Description)).Value;
            tempUtill.BaseDamaged = float.Parse(data.Attribute(nameof(tempUtill.BaseDamaged)).Value);
            tempUtill.DamageMultiSkillLevelName = float.Parse(data.Attribute(nameof(tempUtill.DamageMultiSkillLevelName)).Value);

            string utillSkillNameListStr = data.Attribute(nameof(tempUtill.UtillSkillNameList)).Value;
            if (!string.IsNullOrEmpty(utillSkillNameListStr))
            {
                utillSkillNameListStr = utillSkillNameListStr.Replace("{", string.Empty);
                utillSkillNameListStr = utillSkillNameListStr.Replace("}", string.Empty);

                var names = utillSkillNameListStr.Split(',');

                var list = new List<string>();
                if (names.Length > 0)
                {
                    foreach (var name in names)
                    {
                        list.Add(name);
                    }
                    tempUtill.UtillSkillNameList = list;
                }
                LoadedUtillSkillList.Add(tempUtill.SkillName, tempUtill);
            }

        }


    }
}
