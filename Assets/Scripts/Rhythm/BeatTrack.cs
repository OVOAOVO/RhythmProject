using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

            // 超出左边就销毁
            if (note.anchoredPosition.x < -trackPanel.rect.width / 2 - 100)
            {
                Destroy(note.gameObject);
                activeNotes.RemoveAt(i);
            }
        }
    }

    void SpawnNote()
    {
        GameObject obj = Instantiate(notePrefab, trackPanel);
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(trackPanel.rect.width / 2 + 50, 0); // 从最右边开始
        activeNotes.Add(rt);
    }
}
