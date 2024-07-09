using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MonsterStatManager : MonoBehaviour
{
    public static MonsterStatManager M_instance {  get; private set; }

    public string _name;

    public float _hp;
    public float _maxhp;

    public int _atk;

    [SerializeField] Slider  _hpbar;
    [SerializeField] TextMeshProUGUI monsterName;


    private void Awake()
    {
        M_instance = this;

        _maxhp = 1000;
        _hp = 1000;
        _hpbar.maxValue = _maxhp;
       
        monsterName.text = _name;
    }
    private void Update()
    {
       _hpbar.value = _hp;
    }
}
