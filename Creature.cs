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
        public CreatureType Type;
        public uint Ticks;

        public Creature() { }

        public Creature(CreatureType type, uint ticks = 0)
        {
            Type = type;
            Ticks = ticks;
        }
    }
}
