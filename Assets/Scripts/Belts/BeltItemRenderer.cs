using System.Collections.Generic;
using UnityEngine;

public class BeltItemRenderer : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Sprite fallbackSprite;

    private readonly List<SpriteRenderer> _pool = new List<SpriteRenderer>();

    private void LateUpdate()
    {
        if (BeltManager.Instance == null) return;
        int used = 0;
        foreach (var seg in BeltManager.Instance.Segments)
        {
            if (seg.Slots == null) continue;
            for (int i = 0; i < BeltSegment.SlotCount; i++)
            {
                var slot = seg.Slots[i];
                if (slot.IsEmpty) continue;
                var sr = Get(used++);
                sr.gameObject.SetActive(true);
                sr.sprite = slot.Item.Item.Icon != null ? slot.Item.Item.Icon : fallbackSprite;
                sr.color = slot.Item.Item.Tint;
                Vector3 dir = DirToVec(seg.Direction);
                float t = (i + slot.Progress) / BeltSegment.SlotCount;
                sr.transform.position = seg.transform.position + dir * (t - 0.5f);
            }
        }
        for (int i = used; i < _pool.Count; i++) _pool[i].gameObject.SetActive(false);
    }

    private SpriteRenderer Get(int i)
    {
        while (_pool.Count <= i)
        {
            var o = Instantiate(itemPrefab, transform);
            _pool.Add(o.GetComponent<SpriteRenderer>());
        }
        return _pool[i];
    }

    private Vector3 DirToVec(int d) => d switch
    {
        0 => Vector3.right, 1 => Vector3.down, 2 => Vector3.left, 3 => Vector3.up, _ => Vector3.right
    };
}