using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asdf : MonoBehaviour
{
    public string Atk; // 사용할 이펙트 태그
    public string Skill1;
    public float effectDuration = 2.0f; // 이펙트가 유지될 시간
    private ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        var trigger = ps.trigger;
        trigger.SetCollider(0, GameObject.Find("PlayerCollider").GetComponent<Collider>());
    }

    void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> enterParticles = new List<ParticleSystem.Particle>();
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enterParticles);
        Debug.Log("123");
        PlayerStatManager.instance.AtkPlayer(5.0f);
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
