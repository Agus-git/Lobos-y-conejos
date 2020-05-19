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

        internal bool BuscarConejos(Point position)
        {
            byte contador = 0;
            foreach (Rabbit item in Conejera)
            {
                if (item.Position == position)
                    contador++;
            }
            return contador > 1;
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

        internal Grass PastoQuePiso(Point rabbit)
        {
            int lugarX = rabbit.X / 2;
            int lugarY = rabbit.Y / 2;
            int aproximacion = (lugarX) + (width / 2 * (lugarY));
            int ancho = 128;
            List<Grass> area = new List<Grass>();

            if (lugarY == 127)
                lugarY--;
            if (lugarX == 127)
                lugarX--;

            for (int x = lugarX; x < Grass.PATCH_SIZE + lugarX; x++)
            {
                for (int y = lugarY * ancho; y < (lugarY + Grass.PATCH_SIZE) * ancho; y += ancho)
                {
                    Grass a = Garden[y + x];
                    if (a.Position == rabbit)
                        return a;

                    area.Add(a);
                }
            }
            return area[0];
        }
    }
}
