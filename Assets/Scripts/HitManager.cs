using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    public string Atk; // ����� ����Ʈ �±�
    public string Skill1;
    public float effectDuration = 2.0f; // ����Ʈ�� ������ �ð�


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Atk1"))
        {
            Debug.Log("1");
            PlayerStatManager.instance.GetMp(20);
            PlayerStatManager.instance.AtkMonster(1.0f);

            // �浹 ��ġ�� ����Ʈ Ȱ��ȭ
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            GameObject hitEffect = EffectPullingManager.Instance.EffectPull(Atk);
            hitEffect.transform.position = hitPoint;

            // ��ƼŬ �ý��� ���
            ParticleSystem ps = hitEffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            StartCoroutine(ReturnEffectToPoolAfterDelay(hitEffect, effectDuration));
        }
        else if (other.CompareTag("Atk2"))
        {
            Debug.Log("2");
            PlayerStatManager.instance.GetMp(20);
            PlayerStatManager.instance.AtkMonster(1.5f);

            // �浹 ��ġ�� ����Ʈ Ȱ��ȭ
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            GameObject hitEffect = EffectPullingManager.Instance.EffectPull(Atk);
            hitEffect.transform.position = hitPoint;

            // ��ƼŬ �ý��� ���
            ParticleSystem ps = hitEffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            StartCoroutine(ReturnEffectToPoolAfterDelay(hitEffect, effectDuration));
        }
        else if (other.CompareTag("Atk3"))
        {
            Debug.Log("3");
            PlayerStatManager.instance.GetMp(30);
            PlayerStatManager.instance.AtkMonster(2.0f);

            // �浹 ��ġ�� ����Ʈ Ȱ��ȭ
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            GameObject hitEffect = EffectPullingManager.Instance.EffectPull(Atk);
            hitEffect.transform.position = hitPoint;

            // ��ƼŬ �ý��� ���
            ParticleSystem ps = hitEffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            StartCoroutine(ReturnEffectToPoolAfterDelay(hitEffect, effectDuration));
        }
        else if (other.CompareTag("Skill1_1"))
        {
            Debug.Log("1");
            PlayerStatManager.instance.AtkMonster(2.0f);

            // �浹 ��ġ�� ����Ʈ Ȱ��ȭ
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            GameObject hitEffect = EffectPullingManager.Instance.EffectPull(Skill1);
            hitEffect.transform.position = hitPoint;

            // ��ƼŬ �ý��� ���
            ParticleSystem ps = hitEffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            StartCoroutine(ReturnEffectToPoolAfterDelay(hitEffect, effectDuration));
        }
        else if (other.CompareTag("Skill1_2"))
        {
            Debug.Log("2");
            PlayerStatManager.instance.AtkMonster(5.0f);

            // �浹 ��ġ�� ����Ʈ Ȱ��ȭ
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            GameObject hitEffect = EffectPullingManager.Instance.EffectPull(Skill1);
            hitEffect.transform.position = hitPoint;

            // ��ƼŬ �ý��� ���
            ParticleSystem ps = hitEffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            StartCoroutine(ReturnEffectToPoolAfterDelay(hitEffect, effectDuration));
        }
    }

    private IEnumerator ReturnEffectToPoolAfterDelay(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);

        // ��ƼŬ �ý��� ����
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop();
        }

        EffectPullingManager.Instance.ReturnObject(effect);


    }
}
