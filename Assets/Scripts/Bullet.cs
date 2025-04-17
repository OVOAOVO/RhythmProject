
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    public GameObject explosionPrefab;

    new private Rigidbody2D rigidbody2D;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void SetSpeed(Vector2 direction)
    {
        rigidbody2D.linearVelocity = direction * speed;
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject exp = ObjectPool.Instance.GetGameObject(explosionPrefab);
        exp.transform.position = transform.position;
        ObjectPool.Instance.PushObject(gameObject);
    }
}