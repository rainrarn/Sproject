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

}
