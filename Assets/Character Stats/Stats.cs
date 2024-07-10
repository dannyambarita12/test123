using Kryz.CharacterStats;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace SimpleCharacterStats
{
    public interface ICharacterStat
    {
        public Stats Stats { get; }
    }

    [Serializable]
    public class Stats
    {
        public event Action<StatType, CharacterStat> OnStatsChanged
        {
            add { _onStatChanged += value; } 
            remove { _onStatChanged -= value; }
        }

        private Action<StatType, CharacterStat> _onStatChanged;
        
        [ShowInInspector, HideLabel]
        private Dictionary<StatType, CharacterStat> _stats;

        public Stats()
        {
            _stats = new();
        }

        public Stats(BaseStatValue<StatType>[] initialStats)
        {
            _stats = new();

            foreach (var stat in initialStats)
            {
                _stats.Add(stat.Type, new CharacterStat(stat.Value));
            }
        }

        public CharacterStat this[StatType type]
        {
            get
            {
                if (_stats.ContainsKey(type) == false)
                {
                    _stats.Add(type, new CharacterStat());
                }

                return _stats[type];
            }
        }

        public void AddModifier(StatType type, StatModifier mod)
        {
            var stat = this[type];
            stat.AddModifier(mod);
            _onStatChanged?.Invoke(type, stat);
        }

        public void RemoveModifier(StatType type, StatModifier mod)
        {
            var stat = this[type];
            stat.RemoveModifier(mod);
            _onStatChanged?.Invoke(type, stat);
        }
    }
}