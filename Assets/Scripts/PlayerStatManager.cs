using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager instance { get; private set; }

    public float _hp;
    public float _maxHp;
    
    public float _stamina;
    public float _maxStamina;
    public float _stamonaRegenPerSecond = 5;

    public float _mp;
    public float _maxMp;
    public int _manacristal;
    public int _cristalcount;

    public GameObject ManaCristal1;
    public GameObject ManaCristal2;
    public GameObject ManaCristal3;
    public GameObject ManaCristal4;
    public GameObject ManaCristal5;

    public int _atk;
    public float _critical;
    public int skillpoints;

    [SerializeField] Slider _hpBar;
    [SerializeField] Slider _staminaBar;
    [SerializeField] Slider _mpBar;


    private void Awake()
    {
        _maxHp = 100;
        _hp = 100;
        _maxStamina = 100;
        _stamina = 100;
        _maxMp = 100;
        _mp = 10;

        instance = this;
        _atk = 1;
        _critical = 0;
        skillpoints = 10;

        _hpBar.maxValue = _maxHp;
        _hpBar.value = _hp;

        _staminaBar.maxValue = _maxStamina;
        _staminaBar.value = _stamina;

        _mpBar.maxValue = _maxMp;
        _mpBar.value = _mp;

        ManaCristal1.SetActive(false);
        ManaCristal2.SetActive(false);
        ManaCristal3.SetActive(false);
        ManaCristal4.SetActive(false);
        ManaCristal5.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            _hp -= 10;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if(_hp < _maxHp)
            {
                _hp += 10;
                if(_hp > _maxHp)
                {
                    _hp = _maxHp;
                }
            }  
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            _mp -= 10;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_mp < _maxMp)
            {
                _mp += 10;
                if (_mp > _maxMp)
                {
                    _mp = _maxMp;
                }
            }
            GetManaCristal();
        }

        _hpBar.value = _hp;
        _mpBar.value = _mp;
        _staminaBar.value = _stamina;
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

    public void GetMp(int mp)
    {
        _mp += mp;
    }

    public void GetManaCristal()
    {
        if(_mp>=_maxMp && _cristalcount < 5)
        {
            _mp -= _maxMp;
            PlusCristal();
        }
    }
    
    private void PlusCristal()
    {
        _cristalcount++;
        CristalActive();
    }

    private void MinusCristal()
    {
        _cristalcount--;
        CristalActive();
    }
    private void CristalActive()
    {
        if (_cristalcount ==1)
        {
            ManaCristal1.SetActive(true);
            ManaCristal2.SetActive(false);
            ManaCristal3.SetActive(false);
            ManaCristal4.SetActive(false);
            ManaCristal5.SetActive(false);
        }
        else if(_cristalcount ==2)
        {
            ManaCristal1.SetActive(true);
            ManaCristal2.SetActive(true);
            ManaCristal3.SetActive(false);
            ManaCristal4.SetActive(false);
            ManaCristal5.SetActive(false);
        }
        else if(_cristalcount ==3)
        {
            ManaCristal1.SetActive(true);
            ManaCristal2.SetActive(true);
            ManaCristal3.SetActive(true);
            ManaCristal4.SetActive(false);
            ManaCristal5.SetActive(false);
        }
        else if(_cristalcount ==4)
        {
            ManaCristal1.SetActive(true);
            ManaCristal2.SetActive(true);
            ManaCristal3.SetActive(true);
            ManaCristal4.SetActive(true);
            ManaCristal5.SetActive(false);
        }
        else if(_cristalcount ==5)
        {
            ManaCristal1.SetActive(true);
            ManaCristal2.SetActive(true);
            ManaCristal3.SetActive(true);
            ManaCristal4.SetActive(true);
            ManaCristal5.SetActive(true);
        }
    }
}