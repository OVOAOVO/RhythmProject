using System.Collections;
using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    public float fadeSpeed;
    private LineRenderer line;
    private float alpha;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        alpha = line.endColor.a;
    }

    private void OnEnable()
    {
        line.endColor = new Color(line.endColor.r, line.endColor.g, line.endColor.b);
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while(line.endColor.a > 0)
        {
            line.endColor = new Color(line.endColor.r, line.endColor.g, line.endColor.b);
            yield return new WaitForFixedUpdate();
        }

        ObjectPool.Instance.PushObject(gameObject);
    }
}
