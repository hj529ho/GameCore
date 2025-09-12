using System;
using System.Collections.Generic;

public interface IGachaReward
{
    void SaveInventory(); // 뽑기 성공 시 인벤토리에 저장
}

public class GachaEntry<T>: UnityEngine.ScriptableObject where T : IGachaReward
{
    public readonly T Reward;
    public readonly float Weight;
    public GachaEntry(T reward, float weight)
    {
        if (weight <= 0f) throw new ArgumentOutOfRangeException(nameof(weight), "Weight must be > 0");
        Reward = reward;
        Weight = weight;
    }
}

public class GachaBox
{
    public List<GachaEntry<IGachaReward>> entries = new();
}