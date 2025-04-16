using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public ParticleSystem particleEffect;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (particleEffect != null)
        {
            if (!particleEffect.isPlaying) // 检查粒子系统是否未在播放
            {
                particleEffect.Play(); // 播放粒子效果
            }
            else
            {
                particleEffect.Stop(); // 停止当前粒子效果
                particleEffect.Clear(); // 清除现有粒子
                particleEffect.Play(); // 重新播放粒子效果
            }
        }
    }
}
