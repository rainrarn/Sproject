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
    void OnSkillButtonClicked(AttackSkillNode skill, GameObject skillButton)
    {
        // 레벨 증가 버튼을 스킬 버튼 아래에 생성합니다.
        GameObject incrementButton = Instantiate(incrementButtonPrefab, skillButton.transform);

        // 증가 버튼의 텍스트를 "+"로 설정합니다.
        incrementButton.GetComponentInChildren<Text>().text = "+";

        // 증가 버튼 클릭 이벤트에 스킬 레벨을 증가시키는 메서드를 추가합니다.
        incrementButton.GetComponent<Button>().onClick.AddListener(() => skillTree.LevelUpSkill(skill));

        // 레벨 감소 버튼을 스킬 버튼 아래에 생성합니다.
        GameObject decrementButton = Instantiate(decrementButtonPrefab, skillButton.transform);

        // 감소 버튼의 텍스트를 "-"로 설정합니다.
        decrementButton.GetComponentInChildren<Text>().text = "-";

        // 감소 버튼 클릭 이벤트에 스킬 레벨을 감소시키는 메서드를 추가합니다.
        decrementButton.GetComponent<Button>().onClick.AddListener(() => skillTree.LevelDownSkill(skill));
    }
}

