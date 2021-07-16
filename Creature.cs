using System;
using System.Collections.Generic;
using System.Text;

namespace praysim
{
    public enum CreatureType
    {
        None,
        Predator,
        Pray
    }

    public class Creature
    {
        public CreatureType type;
        public uint ticks;

        public Creature() { }

        public Creature(CreatureType type, uint ticks = 0)
        {
            this.type = type;
            this.ticks = ticks;
        }
    }
}
