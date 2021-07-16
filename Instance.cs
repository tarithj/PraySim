using System;
using System.Collections.Generic;
using System.Text;

namespace praysim
{
    class Instance
    {
        public Creature[,] world;
        public uint x_size;
        public uint y_size;
        public uint max_ticks_to_death;
        public uint dup_interval;

        public Instance(Creature[,] world, uint x_size, uint y_size, uint max_ticks_to_death, uint dup_interval)
        {
            this.world = world;
            this.x_size = x_size;
            this.y_size = y_size;
            this.max_ticks_to_death = max_ticks_to_death;
            this.dup_interval = dup_interval;
        }

        public uint GetPredCount()
        {
            uint pred_counter = 0;
            for (uint x = 0; x < x_size; x++)
            {
                for (uint y = 0; y < y_size; y++) 
                {
                    if (world[x, y].type == CreatureType.Predator)
                    {
                        pred_counter++;
                    }
                }
            }
            return pred_counter;
        }

        public uint GetPrayCount()
        {
            uint pray_counter = 0;
            for (uint x = 0; x < x_size; x++)
            {
                for (uint y = 0; y < y_size; y++)
                {
                    if (world[x, y].type == CreatureType.Pray)
                    {
                        pray_counter++;
                    }
                }
            }
            return pray_counter;
        }

        private void Move_Cell(uint from_x, uint from_y, uint to_x, uint to_y)
        {
            if (to_x > 0 && to_x < 800 && to_y > 0 && to_y < 800)
            {
                if (world[to_x, to_y].type != CreatureType.None)
                {
                    if (world[to_x, to_y].type == CreatureType.Pray && world[from_x, from_y].type == CreatureType.Predator)
                    {
                        // predetor catches pray and multiplies
                        world[to_x, to_y].type = CreatureType.Predator;
                        world[to_x, to_y].ticks = 0;
                    }
                    else if (world[to_x, to_y].type == CreatureType.Predator && world[from_x, from_y].type == CreatureType.Pray)
                    {
                        // pray lands on predetor and predetor multiplies
                        world[from_x, from_y].type = CreatureType.Predator;
                        world[from_x, from_y].ticks = 0;
                    }
                }
                else
                {
                    world[to_x, to_y] = world[from_x, from_y];
                    world[from_x, from_y] = new Creature(CreatureType.None);
                }
            }
        }

        public void Step()
        {
            for (uint x = 0; x < x_size; x++)
            {
                for (uint y = 0; y < y_size; y++)
                {
                    var cell = world[x, y];
                    world[x, y].ticks++;

                    if (cell.ticks > max_ticks_to_death)
                    {
                        // the cell dies
                        world[x, y] = new Creature(CreatureType.None);
                    }

                    if (cell.ticks % dup_interval == 0 && cell.type == CreatureType.Pray)
                    {
                        if (y+1 >= 800)
                        {
                            world[x, y - 1] = new Creature(CreatureType.Pray);
                        }
                        else
                        {
                            world[x, y + 1] = new Creature(CreatureType.Pray);
                        }
                    }

                    if (cell.type != CreatureType.None)
                    {
                        Random rnd = new Random();
                        int rnd_direction = rnd.Next(8);

                        switch (rnd_direction)
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
        }

        public SFML.Graphics.Image GetWorldImage(uint x_size, uint y_size)
        {
            var black = new SFML.Graphics.Color(18, 18, 18);
            var green = new SFML.Graphics.Color(0, 0xff, 0);
            var red = new SFML.Graphics.Color(0xff, 0, 0);

            var dest = new SFML.Graphics.Image(x_size, y_size);

            for (uint x = 0; x < x_size; x++)
            {
                for (uint y = 0; y < y_size; y++)
                {
                    switch (world[x, y].type)
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
