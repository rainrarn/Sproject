using System.Collections;
using System.Collections.Generic;

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



}
