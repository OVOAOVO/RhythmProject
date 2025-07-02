using System;
using System.Collections;
using UnityEngine;

// 粒子管理器，单例模式（也可以用依赖注入）
public class ParticleEffectManager : MonoBehaviour
{
    public static ParticleEffectManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void PlayEnemySpawnEffect(GameObject effectPrefab, Vector3 position)
    {
        PlayEffect(effectPrefab, position);
    }

    public void PlayEnemyDieEffect(GameObject effectPrefab, Vector3 position)
    {
        PlayEffect(effectPrefab, position);
    }

    public void PlayEffect(GameObject effectPrefab, Vector3 position)
    {
        GameObject particle = ObjectPool.Instance.GetGameObject(effectPrefab);
        particle.transform.position = position;
        particle.SetActive(true);

        ParticleSystem ps = particle.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            StartCoroutine(RecycleWhenDone(particle, ps));
        }
    }

    private IEnumerator RecycleWhenDone(GameObject go, ParticleSystem ps)
    {
        yield return new WaitWhile(() => ps.IsAlive(true));
        ObjectPool.Instance.PushObject(go);
    }
}
