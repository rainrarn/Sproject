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

    // ��ų ��ư���� �����ϴ� �޼����Դϴ�.
    void CreateSkillButtons()
    {
        // ��� ��ų�� ��ȸ�ϸ鼭 ��ư�� �����մϴ�.
        foreach (var skill in skillTree.allSkills)
        {
            // ��ų ��ư �ν��Ͻ��� �����մϴ�.
            GameObject skillButton = Instantiate(skillButtonPrefab, skillButtonParent);

            // ��ư�� �ؽ�Ʈ�� ��ų �̸����� �����մϴ�.
            skillButton.GetComponentInChildren<Text>().text = skill.skillName;

            // ��ư Ŭ�� �̺�Ʈ�� OnSkillButtonClicked �޼��带 �߰��մϴ�.
            skillButton.GetComponent<Button>().onClick.AddListener(() => OnSkillButtonClicked(skill, skillButton));
        }
    }

    // ��ų ��ư�� Ŭ������ �� ȣ��Ǵ� �޼����Դϴ�.
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
