using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject monsterPrefab; // 怪物预设体
    public GameObject target; // 目标物体
    public float radius = 5f; // 圆弧半径
    public float startAngle = 0f; // 起始角度
    public float endAngle = 180f; // 结束角度
    public int numMonsters = 10; // 怪物数量
    public float moveSpeed = 20f; // 怪物的移动速度
    void Start()
    {
        SpawnMonstersOnArc();
    }

    void SpawnMonstersOnArc()
    {
        // 计算每个怪物的角度增量
        float angleIncrement = (endAngle - startAngle) / (numMonsters - 1);

        for (int i = 0; i < numMonsters; i++)
        {
            GameObject monster = ObjectPool.Instance.GetGameObject(monsterPrefab);
            // 计算当前怪物的位置角度
            float angle = startAngle + i * angleIncrement;

            // 将角度转换为弧度
            float radians = angle * Mathf.Deg2Rad;

            // 计算怪物位置（极坐标转笛卡尔坐标）
            float x = radius * Mathf.Cos(radians);
            float y = radius * Mathf.Sin(radians);

            // 顺时针旋转90度
            float rotatedX = y;  // 顺时针旋转90度：x' = y
            float rotatedY = -x; // 顺时针旋转90度：y' = -x

            // 创建怪物实例，位置是经过旋转的坐标
            Vector3 spawnPosition = new Vector3(rotatedX, rotatedY, 0f); // 假设圆弧平面是在XZ平面上
            monster.transform.position = spawnPosition; // 设置怪物位置
            
            // 使怪物朝向目标
            if (target != null)
            {
                monster.transform.LookAt(target.transform);
                StartCoroutine(MoveMonster(monster, spawnPosition, target.transform.position));
            }
        }
    }

      // 协程来实现怪物的平滑移动
    IEnumerator MoveMonster(GameObject monster, Vector3 startPosition, Vector3 targetPosition)
    {
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        float distanceCovered = 0f;
        
        while (distanceCovered < journeyLength)
        {
            // 每个节拍按设定的移动步伐进行移动
            float step = moveSpeed * Time.deltaTime;

            distanceCovered += step;

            // 平滑移动
            monster.transform.position = Vector3.MoveTowards(monster.transform.position, targetPosition, step);

            // 每当hit变化时更新目标位置
            if (Conductor.Instance.hit > 0 && distanceCovered >= journeyLength)
            {
                // 这里可以触发怪物到达目标后的行为，或者根据hit重置目标
                break;
            }

            yield return null; // 等待下一帧
        }

    }

//   // 协程来实现怪物的按照节拍移动一小段
//     IEnumerator MoveMonster(GameObject monster, Vector3 startPosition, Vector3 targetPosition)
//     {
//         float journeyLength = Vector3.Distance(startPosition, targetPosition);
//         float distanceCovered = 0f;

//         monster.transform.position = startPosition;

//         float previousHit = Conductor.Instance.hit;

//         while (distanceCovered < journeyLength)
//         {

//             float currentHit = Conductor.Instance.hit;

//             if (currentHit > previousHit)
//             {

//                 float step = moveSpeed * Time.deltaTime;

//                 distanceCovered += step;

//                 monster.transform.position = startPosition + (targetPosition - startPosition).normalized * distanceCovered;
//                // monster.transform.position = Vector3.MoveTowards(monster.transform.position, targetPosition, step);
//             }

//             previousHit = currentHit;

//             //yield return new WaitForSeconds(0.1f);
//             yield return null;
//         }

//         // 到达目标后，执行怪物到达目标后的逻辑（如果有）
//         // 这里可以加入触发事件的代码
//     }


}