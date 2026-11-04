using UnityEngine;

public class BeltSegment : BuildingBase
{
    public const int SlotCount = 4;
    public const float BeltSpeed = 1.5f;

    public class BeltSlot
    {
        public ItemStack Item;
        public float Progress;
        public bool IsEmpty => Item.IsEmpty;
        public void Clear() { Item = default; Progress = 0; }
    }

    public BeltSlot[] Slots { get; private set; }
    public BeltSegment Next { get; set; }
    public BeltSegment Prev { get; set; }
    public int Direction => Rotation;

    protected override int InventoryCapacity => 0;

    public override void Initialize(BuildingData data, Vector2Int gridPos, int rotation)
    {
        base.Initialize(data, gridPos, rotation);
        Slots = new BeltSlot[SlotCount];
        for (int i = 0; i < SlotCount; i++) Slots[i] = new BeltSlot();
    }

    public override void OnTick(float dt)
    {
        if (Slots == null) return;
        for (int i = SlotCount - 1; i >= 0; i--)
        {
            var s = Slots[i];
            if (s.IsEmpty) continue;
            s.Progress += BeltSpeed * dt;
            if (s.Progress >= 1f)
            {
                if (i < SlotCount - 1 && Slots[i + 1].IsEmpty)
                { Slots[i + 1].Item = s.Item; Slots[i + 1].Progress = 0; s.Clear(); }
                else if (i == SlotCount - 1 && Next != null && Next.Slots[0].IsEmpty)
                { Next.Slots[0].Item = s.Item; Next.Slots[0].Progress = 0; s.Clear(); }
                else s.Progress = 1f;
            }
        }
    }

    public override bool TryAddItem(ItemData item, int count = 1)
    {
        if (Slots == null || !Slots[0].IsEmpty) return false;
        Slots[0].Item = new ItemStack(item, count);
        Slots[0].Progress = 0;
        return true;
    }

    public Vector2Int GetDirectionVector() => Direction switch
    {
        0 => Vector2Int.right, 1 => Vector2Int.down, 2 => Vector2Int.left, 3 => Vector2Int.up, _ => Vector2Int.right
    };
}