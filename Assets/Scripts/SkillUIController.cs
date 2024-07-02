using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SkillUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _skillpointstext;
    [SerializeField] private TextMeshProUGUI _skillInfo;

    [SerializeField] private GameObject SkillWindow;
    [SerializeField] private GameObject SelectSkillWindow;
    [SerializeField] private GameObject AtkSkillTree;
    [SerializeField] private GameObject DEFSkillTree;
    [SerializeField] private GameObject UTISkillTree;

    [SerializeField] private Button ExitButton;
    [SerializeField] private Button BackSpace;
    

    private void Update()
    {
        // _skillpointstext.text = $"스킬 포인트 : {PlayerStatManager.instance.skillpoints}";
        if(Input.GetKeyDown(KeyCode.K))
        {
            if(SkillWindow.activeSelf)
            {
                SkillWindow.SetActive(false);
            }
            else
            {
                SetSkillHome();
                SkillWindow.SetActive(true);
            }
        }
    }
    public void ExitSkillWindow()
    {
        SkillWindow.SetActive(false);
    }
    public void SetSkillHome()
    {
        _skillInfo.text = " ";
        SelectSkillWindow.SetActive(true);
        AtkSkillTree.SetActive(false);
        DEFSkillTree.SetActive(false);
        UTISkillTree.SetActive(false);
    }
    public void OnClick_AtkTree()
    {
        AtkSkillTree.SetActive(true);
    }
    public void OnClick_DefTree()
    {
        DEFSkillTree.SetActive(true);
    }
    public void OnClick_UtiTree()
    {
        UTISkillTree.SetActive(true);
    }
}
