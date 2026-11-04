using System.Collections.Generic;
using UnityEngine;

public class BeltManager : MonoBehaviour
{
    public static BeltManager Instance { get; private set; }
    private readonly List<BeltSegment> _segments = new List<BeltSegment>();
    public IReadOnlyList<BeltSegment> Segments => _segments;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Register(BeltSegment s) { if (s != null && !_segments.Contains(s)) _segments.Add(s); }
    public void Unregister(BeltSegment s) => _segments.Remove(s);
    public void ClearAll() => _segments.Clear();

    public void RelinkAll()
    {
        foreach (var s in _segments) { s.Next = null; s.Prev = null; }
        foreach (var s in _segments)
        {
            var nt = GridManager.Instance.GetTileAt(s.GridPosition + s.GetDirectionVector());
            if (nt?.Building == null) continue;
            var nb = nt.Building.GetComponent<BeltSegment>();
            if (nb == null) continue;
            s.Next = nb; nb.Prev = s;
        }
    }
}