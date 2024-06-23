using UnityEngine;
using UnityEngine.UI;

public class AttackSkillTreeUI : MonoBehaviour
{
    public AttackSkillTree skillTree;
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
    void OnSkillButtonClicked(AttackSkillNode skill, GameObject skillButton)
    {
        // ���� ���� ��ư�� ��ų ��ư �Ʒ��� �����մϴ�.
        GameObject incrementButton = Instantiate(incrementButtonPrefab, skillButton.transform);

        // ���� ��ư�� �ؽ�Ʈ�� "+"�� �����մϴ�.
        incrementButton.GetComponentInChildren<Text>().text = "+";

        // ���� ��ư Ŭ�� �̺�Ʈ�� ��ų ������ ������Ű�� �޼��带 �߰��մϴ�.
        incrementButton.GetComponent<Button>().onClick.AddListener(() => skillTree.LevelUpSkill(skill));

        // ���� ���� ��ư�� ��ų ��ư �Ʒ��� �����մϴ�.
        GameObject decrementButton = Instantiate(decrementButtonPrefab, skillButton.transform);

        // ���� ��ư�� �ؽ�Ʈ�� "-"�� �����մϴ�.
        decrementButton.GetComponentInChildren<Text>().text = "-";

        // ���� ��ư Ŭ�� �̺�Ʈ�� ��ų ������ ���ҽ�Ű�� �޼��带 �߰��մϴ�.
        decrementButton.GetComponent<Button>().onClick.AddListener(() => skillTree.LevelDownSkill(skill));
    }
}

