using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public string Hit; // ����� ����Ʈ �±�
    public float effectDuration = 2.0f; // ����Ʈ�� ������ �ð�
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            
            Debug.Log("����");

            // �浹 ��ġ�� ����Ʈ Ȱ��ȭ
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            GameObject hitEffect = EffectPullingManager.Instance.EffectPull(Hit);
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
