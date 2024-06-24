using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UtillSkillTreeUI : MonoBehaviour
{
    public UtillSkillTree skillTree;
    public GameObject skillButtonPrefab;
    public GameObject incrementButtonPrefab;
    public GameObject decrementButtonPrefab;
    public Transform skillButtonParent;

    void Start()
    {
        CreateSkillButtons();
    }

    // 스킬 버튼들을 생성하는 메서드입니다.
    void CreateSkillButtons()
    {
        // 모든 스킬을 순회하면서 버튼을 생성합니다.
        foreach (var skill in skillTree.allSkills)
        {
            // 스킬 버튼 인스턴스를 생성합니다.
            GameObject skillButton = Instantiate(skillButtonPrefab, skillButtonParent);

            // 버튼의 텍스트를 스킬 이름으로 설정합니다.
            skillButton.GetComponentInChildren<Text>().text = skill.skillName;

            // 버튼 클릭 이벤트에 OnSkillButtonClicked 메서드를 추가합니다.
            skillButton.GetComponent<Button>().onClick.AddListener(() => OnSkillButtonClicked(skill, skillButton));
        }
    }

    // 스킬 버튼을 클릭했을 때 호출되는 메서드입니다.
    void OnSkillButtonClicked(UtillSkillTreeNode skill, GameObject skillButton)
    {
        GameObject incrementButton = Instantiate(incrementButtonPrefab, skillButton.transform);
        incrementButton.GetComponentInChildren<Text>().text = "+";
        incrementButton.GetComponent<Button>().onClick.AddListener(() => skillTree.LevelUpSkill(skill));
        GameObject decrementButton = Instantiate(decrementButtonPrefab, skillButton.transform);
        decrementButton.GetComponentInChildren<Text>().text = "-";
        decrementButton.GetComponent<Button>().onClick.AddListener(() => skillTree.LevelDownSkill(skill));
    }
}
