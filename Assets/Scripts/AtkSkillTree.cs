using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AtkSkillTree : MonoBehaviour
{
    [SerializeField] private GameObject _skillA;
    [SerializeField] private GameObject _skillB;
    [SerializeField] private GameObject _skillC;
    [SerializeField] private GameObject _skillD;
    [SerializeField] private GameObject _skillE;
    [SerializeField] private GameObject _skillF;
    [SerializeField] private GameObject _skillG;
    [SerializeField] private GameObject _skillH;

    private bool isClickedA;
    private bool isClickedB;
    private bool isClickedC;
    private bool isClickedD;
    private bool isClickedE;
    private bool isClickedF;
    private bool isClickedG;
    private bool isClickedH;
    private bool isSkillAUP;
    private bool isSkillBUP;
    private bool isSkillCUP;
    private bool isSkillDUP;
    private bool isSkillEUP;
    private bool isSkillFUP;
    private bool isSkillGUP;
    private bool isSkillHUP;
    [SerializeField] private TextMeshProUGUI _skillInfo;

    public void Awake()
    {
        Color color = _skillA.GetComponent<Image>().color;
        color.a = 0.5f;
        _skillB.GetComponent<Image>().color = color;
        _skillC.GetComponent<Image>().color = color;
        _skillD.GetComponent<Image>().color = color;
        _skillE.GetComponent<Image>().color = color;
        _skillF.GetComponent<Image>().color = color;
        _skillG.GetComponent<Image>().color = color;
        _skillH.GetComponent<Image>().color = color;

    }
    public void OnClick_skillA() 
    {
        if (isClickedA)
        {
            isSkillAUP = true;
            //PlayerStatManager.instance.PlayerAtkUp(1);
            Color color = _skillA.GetComponent<Image>().color;
            _skillB.GetComponent<Image>().color= color;
            _skillC.GetComponent<Image>().color = color;
        }
        else
        {
            DisableSkills();
            isClickedA = true;
            _skillInfo.text = " skillA 설명 ";
        }
    }
    public void OnClick_skillB()
    {
        if (isClickedB && isSkillAUP ==true)
        {
            isSkillBUP = true;
            //PlayerStatManager.instance.PlayerCritical(10);
            Color color = _skillA.GetComponent<Image>().color;
            _skillD.GetComponent<Image>().color = color;
            _skillE.GetComponent<Image>().color = color;
        }
        else
        {
            DisableSkills();
            isClickedB = true;
            _skillInfo.text = " skillB 설명 ";
        }
    }
    public void OnClick_skillC()
    {
        if (isClickedC && isSkillAUP == true)
        {
            isSkillCUP = true;
            //PlayerStatManager.instance.PlayerCritical(10);
            Color color = _skillA.GetComponent<Image>().color;
            _skillF.GetComponent<Image>().color = color;
            _skillG.GetComponent<Image>().color = color;
        }
        else
        {
            DisableSkills();
            isClickedC = true;
            _skillInfo.text = " skillC 설명 ";
        }
    }
    public void OnClick_skillD()
    {
        if (isClickedD && isSkillBUP == true)
        {
            isSkillDUP = true;
            //PlayerStatManager.instance.PlayerCritical(10);
            Color color = _skillA.GetComponent<Image>().color;
            _skillH.GetComponent<Image>().color = color;
        }
        else
        {
            DisableSkills();
            isClickedD = true;
            _skillInfo.text = " skillD 설명 ";
        }
    }
    public void OnClick_skillE()
    {
        if (isClickedE && isSkillBUP == true)
        {
            isSkillEUP = true;
            //PlayerStatManager.instance.PlayerCritical(10);
            Color color = _skillA.GetComponent<Image>().color;
            _skillH.GetComponent<Image>().color = color;
        }
        else
        {
            DisableSkills();
            isClickedE = true;
            _skillInfo.text = " skillE 설명 ";
        }
    }
    public void OnClick_skillF()
    {
        if (isClickedF && isSkillCUP == true)
        {
            isSkillFUP = true;
            //PlayerStatManager.instance.PlayerCritical(10);
            Color color = _skillA.GetComponent<Image>().color;
            _skillH.GetComponent<Image>().color = color;
        }
        else
        {
            DisableSkills();
            isClickedF = true;
            _skillInfo.text = " skillF 설명 ";
        }
    }
    public void OnClick_skillG()
    {
        if (isClickedG && isSkillCUP == true)
        {
            isSkillGUP = true;
            //PlayerStatManager.instance.PlayerCritical(10);
            Color color = _skillA.GetComponent<Image>().color;
            _skillH.GetComponent<Image>().color = color;
        }
        else
        {
            DisableSkills();
            isClickedG = true;
            _skillInfo.text = " skillG 설명 ";
        }
    }
    public void OnClick_skillH()
    {
        if (isClickedH && (isSkillDUP || isSkillEUP || isSkillFUP|| isSkillGUP == true))
        {
            isSkillHUP = true;
            //PlayerStatManager.instance.PlayerCritical(10);
        }
        else
        {
            DisableSkills();
            isClickedH = true;
            _skillInfo.text = " skillH 설명 ";
        }
    }

    private void DisableSkills()
    {
        isClickedA =false;
        isClickedB =false;
        isClickedC =false;
        isClickedD =false;
        isClickedE =false;
        isClickedF =false;
        isClickedG =false;
        isClickedH =false;
        _skillInfo.text = "설명";
    }
}
