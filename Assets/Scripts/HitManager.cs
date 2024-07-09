using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    public string Atk; // 사용할 이펙트 태그
    public string Skill1;
    public float effectDuration = 2.0f; // 이펙트가 유지될 시간


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Atk1"))
        {
            Debug.Log("1");
            PlayerStatManager.instance.GetMp(20);
            PlayerStatManager.instance.AtkMonster(1.0f);

            // 충돌 위치에 이펙트 활성화
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            GameObject hitEffect = EffectPullingManager.Instance.EffectPull(Atk);
            hitEffect.transform.position = hitPoint;

            // 파티클 시스템 재생
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

            // 충돌 위치에 이펙트 활성화
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            GameObject hitEffect = EffectPullingManager.Instance.EffectPull(Atk);
            hitEffect.transform.position = hitPoint;

            // 파티클 시스템 재생
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

            // 충돌 위치에 이펙트 활성화
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            GameObject hitEffect = EffectPullingManager.Instance.EffectPull(Atk);
            hitEffect.transform.position = hitPoint;

            // 파티클 시스템 재생
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

            // 충돌 위치에 이펙트 활성화
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            GameObject hitEffect = EffectPullingManager.Instance.EffectPull(Skill1);
            hitEffect.transform.position = hitPoint;

            // 파티클 시스템 재생
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

            // 충돌 위치에 이펙트 활성화
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            GameObject hitEffect = EffectPullingManager.Instance.EffectPull(Skill1);
            hitEffect.transform.position = hitPoint;

            // 파티클 시스템 재생
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

        // 파티클 시스템 중지
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop();
        }

        EffectPullingManager.Instance.ReturnObject(effect);


    }
}
