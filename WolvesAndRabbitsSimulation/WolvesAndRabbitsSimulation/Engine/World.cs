using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WolvesAndRabbitsSimulation.Simulation;

namespace WolvesAndRabbitsSimulation.Engine
{
    class World
    {
        private Random rnd = new Random();

        private const int width = 255;
        private const int height = 255;
        private Size size = new Size(width, height);
        List<Rabbit> Conejera = new List<Rabbit>();
        List<Grass> Pasto = new List<Grass>();
        Grass[] Garden = new Grass[0];
        public IEnumerable<GameObject> GameObjects
        {
            get
            {
                List<GameObject> a = new List<GameObject>();
                return a.Concat(Garden).Concat(Conejera).ToArray();
            }
        }

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public float Random()
        {
            return (float)rnd.NextDouble();
        }

        public Point RandomPoint()
        {
            return new Point(rnd.Next(width), rnd.Next(height));
        }

        public int Random(int min, int max)
        {
            return rnd.Next(min, max);
        }

        public void AddRabit(Rabbit obj)
        {
            Conejera.Add(obj);
        }
        public void AddGrass(Grass grass)
        {
            Pasto.Add(grass);
        }

        public void RemoveRabit(Rabbit obj)
        {
            Conejera.Remove(obj);
        }

        public void UnPocoDeOrden()
        {
            Garden = Pasto.ToArray();
        }

        public virtual void Update()
        {
            foreach (GameObject obj in GameObjects)
            {
                obj.UpdateOn(this);
                obj.Position = PositiveMod(obj.Position, size);
            }
        }

        public virtual void DrawOn(Graphics graphics)
        {
            foreach (GameObject obj in GameObjects)
            {
                graphics.FillRectangle(new Pen(obj.Color).Brush, obj.Bounds);
            }
        }

        // http://stackoverflow.com/a/10065670/4357302
        private static int PositiveMod(int a, int n)
        {
            int result = a % n;
            if ((a < 0 && n > 0) || (a > 0 && n < 0))
                result += n;
            return result;
        }
        private static Point PositiveMod(Point p, Size s)
        {
            return new Point(PositiveMod(p.X, s.Width), PositiveMod(p.Y, s.Height));
        }

        public double Dist(PointF a, PointF b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public IEnumerable<GameObject> ObjectsAt(Point pos)
        {
            return GameObjects.Where(each =>
            {
                Rectangle bounds = each.Bounds;
                PointF center = new PointF((bounds.Left + bounds.Right - 1) / 2.0f,
                                           (bounds.Top + bounds.Bottom - 1) / 2.0f);
                return Dist(pos, center) <= bounds.Width / 2.0f
                    && Dist(pos, center) <= bounds.Height / 2.0f;
            });
        }

        /*internal Grass PastoQuePiso(Rabbit rabbit)
        {
            int lugarX = rabbit.Position.X;
            int lugarY = rabbit.Position.Y;
            int aproximacion = lugarX + (width * lugarY);

            for (int x = lugarX; x < Grass.PATCH_SIZE + lugarX; x += Grass.PATCH_SIZE)
            {
                for (int y = aproximacion; y < aproximacion + (width * Grass.PATCH_SIZE); y += (width * Grass.PATCH_SIZE))
                {

                }
            }
        }*/
    }
}
