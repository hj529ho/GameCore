using System;
using System.Collections.Generic;
using System.Linq;

namespace GGDOk.Scripts._Core
{
    /// <summary>
    /// 화폐/인벤토리 의존성은 델리게이트로 주입
    /// </summary>
    public class BaseGacha<TReward> where TReward : IGachaReward
    {
        private readonly int _cost;
        GachaBox _box;
        private readonly Func<int> _getCurrency;
        private readonly Func<int, bool> _trySpend;
        protected BaseGacha(int cost, GachaBox box, Func<int> getCurrency, Func<int, bool> trySpend)
        {
            _cost = cost;
            _box = box;
            _getCurrency = getCurrency;
            _trySpend = trySpend;
        }
        public bool TryRoll(out TReward reward)
        {
            OnBeforePick();
            if (_getCurrency.Invoke() < _cost)
            {
                reward = default;
                return false;
            }
            _trySpend.Invoke(_cost);
            reward = PickWeighted();
            OnAfterPick(reward);
            return true;
        }
        public bool TryRollMany(int count, out List<TReward> rewards)
        {
            OnBeforePickMany(count);
            if (_getCurrency.Invoke() < _cost * count)
            {
                rewards = null;
                return false;
            }
            _trySpend.Invoke(_cost * count);
            rewards = new List<TReward>();
            for (int i = 0; i < count; i++)
            {
                rewards.Add(PickWeighted());
            }
            OnAfterPickMany(rewards);
            return true;
        }
       
        protected virtual void OnBeforePick() { }
        protected virtual void OnAfterPick(TReward reward) { }
        protected virtual void OnBeforePickMany(int count) { }
        protected virtual void OnAfterPickMany(IReadOnlyList<TReward> rewards) { }
        
        protected TReward PickWeighted()
        {
            //TODO 가중치픽
            return default(TReward);
        }
    }
}