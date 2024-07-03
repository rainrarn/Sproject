using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public string Hit; // 사용할 이펙트 태그
    public float effectDuration = 2.0f; // 이펙트가 유지될 시간
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            
            Debug.Log("적중");

            // 충돌 위치에 이펙트 활성화
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            GameObject hitEffect = EffectPullingManager.Instance.EffectPull(Hit);
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
