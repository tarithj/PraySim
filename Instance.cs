using System;
using System.Collections.Generic;
using System.Text;

namespace praysim
{
    internal class Instance
    {
        public readonly Creature[,] World;
        public readonly uint XSize;
        public readonly uint YSize;
        public uint MaxTicksToDeath;
        public uint DupInterval;

        public Instance(Creature[,] world, uint xSize, uint ySize, uint maxTicksToDeath, uint dupInterval)
        {
            World = world;
            XSize = xSize;
            YSize = ySize;
            MaxTicksToDeath = maxTicksToDeath;
            DupInterval = dupInterval;
        }

        /// <summary>
        /// Returns the number of predators in the current world
        /// </summary>
        /// <returns>Number of predators</returns>
        public uint GetPredCount()
        {
            uint predCounter = 0;
            for (uint x = 0; x < XSize; x++)
            {
                for (uint y = 0; y < YSize; y++) 
                {
                    if (World[x, y].Type == CreatureType.Predator)
                    {
                        predCounter++;
                    }
                }
            }
            return predCounter;
        }

        /// <summary>
        /// Returns the number of prays in the current world
        /// </summary>
        /// <returns>Number of prays</returns>
        public uint GetPrayCount()
        {
            uint prayCounter = 0;
            for (uint x = 0; x < XSize; x++)
            {
                for (uint y = 0; y < YSize; y++)
                {
                    if (World[x, y].Type == CreatureType.Pray)
                    {
                        prayCounter++;
                    }
                }
            }
            return prayCounter;
        }

        /// <summary>
        /// Moves cell to destination
        /// </summary>
        private void Move_Cell(uint fromX, uint fromY, uint toX, uint toY)
        {
            if (toX <= 0 || toX >= 800 || toY <= 0 || toY >= 800) return;
            if (World[toX, toY].Type != CreatureType.None)
            {
                switch (World[toX, toY].Type)
                {
                    case CreatureType.Pray when World[fromX, fromY].Type == CreatureType.Predator:
                        // predator catches pray and multiplies
                        World[toX, toY].Type = CreatureType.Predator;
                        World[toX, toY].Ticks = 0;
                        break;
                    case CreatureType.Predator when World[fromX, fromY].Type == CreatureType.Pray:
                        // pray lands on predator and predator multiplies
                        World[fromX, fromY].Type = CreatureType.Predator;
                        World[fromX, fromY].Ticks = 0;
                        break;
                }
            }
            else
            {
                World[toX, toY] = World[fromX, fromY];
                World[fromX, fromY] = new Creature(CreatureType.None);
            }
        }

        /// <summary>
        /// Preforms a step in the simulation
        /// </summary>
        public void Step()
        {
            for (uint x = 0; x < XSize; x++)
            {
                for (uint y = 0; y < YSize; y++)
                {
                    var cell = World[x, y];
                    World[x, y].Ticks++;

                    if (cell.Ticks > MaxTicksToDeath)
                    {
                        // the cell dies
                        World[x, y] = new Creature(CreatureType.None);
                    }

                    if (cell.Ticks % DupInterval == 0 && cell.Type == CreatureType.Pray)
                    {
                        // duplicates cell
                        if (y+1 >= 800)
                        {
                            World[x, y - 1] = new Creature(CreatureType.Pray);
                        }
                        else
                        {
                            World[x, y + 1] = new Creature(CreatureType.Pray);
                        }
                    }

                    if (cell.Type == CreatureType.None) continue;
                    var rnd = new Random();
                    var rndDirection = rnd.Next(8);

                    switch (rndDirection)
                    {
                        case 0:
                            Move_Cell(x, y, x - 1, y + 1);
                            break;
                        case 1:
                            Move_Cell(x, y, x, y + 1);
                            break;
                        case 2:
                            Move_Cell(x, y, x + 1, y + 1);
                            break;
                        case 3:
                            Move_Cell(x, y, x + 1, y);
                            break;
                        case 4:
                            Move_Cell(x, y, x + 1, y - 1);
                            break;
                        case 5:
                            Move_Cell(x, y, x, y - 1);
                            break;
                        case 6:
                            Move_Cell(x, y, x - 1, y - 1);
                            break;
                        default:
                            Move_Cell(x, y, x - 1, y);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Generates a Image of the world
        /// </summary>
        public SFML.Graphics.Image GetWorldImage(uint xSize, uint ySize)
        {
            var black = new SFML.Graphics.Color(18, 18, 18);
            var green = new SFML.Graphics.Color(0, 0xff, 0);
            var red = new SFML.Graphics.Color(0xff, 0, 0);

            var dest = new SFML.Graphics.Image(xSize, ySize);

            for (uint x = 0; x < xSize; x++)
            {
                for (uint y = 0; y < ySize; y++)
                {
                    switch (World[x, y].Type)
                    {
                        case CreatureType.None:
                            dest.SetPixel(x, y, black);
                            break;
                        case CreatureType.Pray:
                            dest.SetPixel(x, y, green);
                            break;
                        case CreatureType.Predator:
                            dest.SetPixel(x, y, red);
                            break;
                    }
                }
            }
            return dest;
        }
    }
}
