using System.Collections.Generic;

namespace Game.Stats
{
    public interface IModifier
    {
        public IEnumerable<float> GetModifier(StatsType statsType);
        public IEnumerable<float> GetPercentage(StatsType statsType);
    }
}