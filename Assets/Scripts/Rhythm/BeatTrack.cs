using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//NOTE: 这个因为是UI，所以需要放在Canvas下，所以生成的滑块虽然使用了对象池，但在BeatTrack下
public class BeatTrack : MonoBehaviour
{
    public RectTransform trackPanel;         // 滑块容器
    public GameObject notePrefab;            // 滑块预制体
    public float spawnInterval = 1.0f;       // 每隔多久生成一个滑块
    public float moveSpeed = 200f;           // 像素每秒，往左移动速度

    private float timer = 0f;
    private List<RectTransform> activeNotes = new List<RectTransform>();

    void Update()
    {
        // 生成新滑块
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnNote();
        }

        // 所有滑块左移
        for (int i = activeNotes.Count - 1; i >= 0; i--)
        {
            var note = activeNotes[i];
            note.anchoredPosition -= new Vector2(moveSpeed * Time.deltaTime, 0);

            // 超出左边就回收
            // 终点判断
            if (note.anchoredPosition.x < -trackPanel.rect.width / 2)
            {
                ObjectPool.Instance.PushObject(note.gameObject);
                activeNotes.RemoveAt(i);
            }
        }
    }

    void SpawnNote()
    {
        GameObject obj = ObjectPool.Instance.GetGameObject(notePrefab);
        obj.transform.SetParent(trackPanel, false); // 保持 UI 尺寸缩放
        RectTransform rt = obj.GetComponent<RectTransform>();
        // 起点
        rt.anchoredPosition = new Vector2(trackPanel.rect.width / 2, 0);
        activeNotes.Add(rt);
    }
}
