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

            for (int x = 0; x< i.XSize; x++)
            {
                for (int y = 0; y < i.YSize; y++)
                {
                    world[x, y] = new Creature();
                }
            }

            var window = new RenderWindow(new VideoMode(i.XSize, i.YSize), "PreySim", Styles.Close);

            // events
            window.Closed += (sender, e) =>
            {
                window.Close();
            };
            window.MouseButtonReleased += (sender, e) =>
            {
                if (e.Button == Mouse.Button.Left)
                {
                    // set pray
                    Console.WriteLine("new pray at " + e.X + "," + e.Y);
                    if (e.X < i.XSize && e.Y < i.YSize && e.X > -1 && e.Y > -1)
                    {
                        i.World[e.X, e.Y].Type = CreatureType.Pray;
                        i.World[e.X, e.Y].Ticks = 0;
                    }
                }
                else if (e.Button == Mouse.Button.Right)
                {
                    // set predator
                    Console.WriteLine("new pred at " + e.X + "," + e.Y);
                    if (e.X < i.XSize && e.Y < i.YSize && e.X > -1 && e.Y > -1)
                    {
                        i.World[e.X, e.Y].Type = CreatureType.Predator;
                        i.World[e.X, e.Y].Ticks = 0;
                    }
                }
            };
            window.KeyPressed += (object sender, KeyEventArgs e) =>
            {
                switch (e.Code)
                {
                    case Keyboard.Key.P:
                        i.MaxTicksToDeath++;
                        break;
                    case Keyboard.Key.O:
                        i.MaxTicksToDeath--;
                        break;
                    case Keyboard.Key.Q:
                        i.DupInterval--;
                        break;
                    case Keyboard.Key.W:
                        i.DupInterval++;
                        break;
                }
            };

            window.SetFramerateLimit(60);

            var textureToDraw = new Texture(800, 800);
            var spriteToDraw = new Sprite(textureToDraw);

            var viewport = i.GetWorldImage(800, 800);
            textureToDraw.Update(viewport);

            var fpsCounter = new Text("FPS:", new Font(Resource1.Roboto_Regular), 25);
            var prayCounter = new Text("PRAY COUNT:", new Font(Resource1.Roboto_Regular), 25);
            var predCounter = new Text("PRED COUNT:", new Font(Resource1.Roboto_Regular), 25);
            var maxLifeCounter = new Text("MAX LIFE:", new Font(Resource1.Roboto_Regular), 25);
            var dupIntCounter = new Text("DUP FREQ:", new Font(Resource1.Roboto_Regular), 25);
            var controls = new Text("press q to dec and w to inc dup interval\npress o to dec and p to inc max life", new Font(Resource1.Roboto_Regular), 25);


            prayCounter.Position = new SFML.System.Vector2f(0, 40);
            predCounter.Position = new SFML.System.Vector2f(0, 80);
            maxLifeCounter.Position = new SFML.System.Vector2f(0, 120);
            dupIntCounter.Position = new SFML.System.Vector2f(0, 160);
            controls.Position = new SFML.System.Vector2f(350, 0);

            var clock = new SFML.System.Clock();
            float dt = 0;

            // main loop
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();

                viewport = i.GetWorldImage(800, 800);
                textureToDraw.Update(viewport);


                // update gui components
                fpsCounter.DisplayedString = "FPS: "+ Math.Round(1/dt);
                prayCounter.DisplayedString = "PRAY COUNT: " + i.GetPrayCount();
                predCounter.DisplayedString = "PRED COUNT: " + i.GetPredCount();
                maxLifeCounter.DisplayedString = "MAX LIFE: " + i.MaxTicksToDeath;
                dupIntCounter.DisplayedString = "DUP INTERVAL: " + i.DupInterval;

                // draw
                window.Draw(spriteToDraw);
                window.Draw(fpsCounter);
                window.Draw(predCounter);
                window.Draw(prayCounter);
                window.Draw(maxLifeCounter);
                window.Draw(dupIntCounter);
                window.Draw(controls);

                window.Display();
                GC.Collect();

                dt = clock.Restart().AsSeconds();
                i.Step();
            }
           
        }
    }
}
