using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager instance { get; private set; }

    public float _hp;
    public float _maxHp;
    public float _stamina;
    public float _maxStamina;
    public float _mp;
    public float _maxMp;
    public int _mpcristal;
    public int _atk;
    public float _critical;
    public int skillpoints;
    private void Awake()
    {
        instance = this;
        _atk = 1;
        _critical = 0;
        skillpoints = 10;
    }
    public void PlayerAtkUp(int atk)
    {
        _atk += atk;
    }
    public void PlayerCritical(float critical)
    {
        _critical += critical;
    }
    public void GetSkillPoints()
    {
        skillpoints++;
    }
    public void UseSkillPoints()
    {
        skillpoints--;
    }

}
