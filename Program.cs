using System;
using SFML.Window;
using SFML.Graphics;

namespace praysim
{
    class Program
    {
        static void Main(string[] args)
        {



            var world = new Creature[800, 800];
            var i = new Instance(world, 800, 800, 50, 25);

            for (int x = 0; x< i.x_size; x++)
            {
                for (int y = 0; y < i.y_size; y++)
                {
                    world[x, y] = new Creature();
                }
            }

            var window = new RenderWindow(new VideoMode(i.x_size, i.y_size), "PreySim", Styles.Close);

            // events
            window.Closed += (object sender, EventArgs e) =>
            {
                window.Close();
            };
            window.MouseButtonReleased += (object sender, MouseButtonEventArgs e) =>
            {
                if (e.Button == Mouse.Button.Left)
                {
                    // set pray
                    Console.WriteLine("new pray at " + e.X + "," + e.Y);
                    if (e.X < i.x_size && e.Y < i.y_size && e.X > -1 && e.Y > -1)
                    {
                        i.world[e.X, e.Y].type = CreatureType.Pray;
                        i.world[e.X, e.Y].ticks = 0;
                    }
                }
                else if (e.Button == Mouse.Button.Right)
                {
                    // set predator
                    Console.WriteLine("new pred at " + e.X + "," + e.Y);
                    if (e.X < i.x_size && e.Y < i.y_size && e.X > -1 && e.Y > -1)
                    {
                        i.world[e.X, e.Y].type = CreatureType.Predator;
                        i.world[e.X, e.Y].ticks = 0;
                    }
                }
            };
            window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                if (e.Code == Keyboard.Key.P)
                {
                    i.max_ticks_to_death++;
                }
                else if (e.Code == Keyboard.Key.O)
                {
                    i.max_ticks_to_death--;
                }
                else if (e.Code == Keyboard.Key.Q)
                {
                    i.dup_interval--;
                }
                else if (e.Code == Keyboard.Key.W)
                {
                    i.dup_interval++;
                }
            };

            window.SetFramerateLimit(30);

            var texture_to_draw = new Texture(800, 800);
            var sprite_to_draw = new Sprite(texture_to_draw);

            var viewport = i.GetWorldImage(800, 800);
            texture_to_draw.Update(viewport);

            var fps_counter = new Text("FPS:", new Font(Resource1.Roboto_Regular), 25);
            var pray_counter = new Text("PRAY COUNT:", new Font(Resource1.Roboto_Regular), 25);
            var pred_counter = new Text("PRED COUNT:", new Font(Resource1.Roboto_Regular), 25);
            var max_life_counter = new Text("MAX LIFE:", new Font(Resource1.Roboto_Regular), 25);
            var dup_int_counter = new Text("DUP FREQ:", new Font(Resource1.Roboto_Regular), 25);
            var controls = new Text("press q to dec and w to inc dup interval\npress o to dec and p to inc max life", new Font(Resource1.Roboto_Regular), 25);


            pray_counter.Position = new SFML.System.Vector2f(0, 40);
            pred_counter.Position = new SFML.System.Vector2f(0, 80);
            max_life_counter.Position = new SFML.System.Vector2f(0, 120);
            dup_int_counter.Position = new SFML.System.Vector2f(0, 160);
            controls.Position = new SFML.System.Vector2f(350, 0);

            var clock = new SFML.System.Clock();
            float dt = 0;

            // main loop
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();

                viewport = i.GetWorldImage(800, 800);
                texture_to_draw.Update(viewport);


                // update gui components
                fps_counter.DisplayedString = "FPS: "+ Math.Round(1/dt);
                pray_counter.DisplayedString = "PRAY COUNT: " + i.GetPrayCount();
                pred_counter.DisplayedString = "PRED COUNT: " + i.GetPredCount();
                max_life_counter.DisplayedString = "MAX LIFE: " + i.max_ticks_to_death;
                dup_int_counter.DisplayedString = "DUP INTERVAL: " + i.dup_interval;

                // draw
                window.Draw(sprite_to_draw);
                window.Draw(fps_counter);
                window.Draw(pred_counter);
                window.Draw(pray_counter);
                window.Draw(max_life_counter);
                window.Draw(dup_int_counter);
                window.Draw(controls);

                window.Display();
                GC.Collect();

                dt = clock.Restart().AsSeconds();
                i.Step();
            }
           
        }
    }
}
