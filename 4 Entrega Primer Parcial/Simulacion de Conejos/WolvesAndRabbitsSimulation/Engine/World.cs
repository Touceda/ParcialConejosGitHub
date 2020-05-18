using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
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

        private Grass[,] grass = new Grass[128, 128];
        private GameObject[] rabit = new GameObject[0];

        public IEnumerable<GameObject> GameObjectsRabit
        {
            get
            {
                return rabit.ToArray();
            }
        }

        public int Width { get { return width; } }
        public int Height { get { return height; } }

        public float Random()//random
        {
            return (float)rnd.NextDouble();
        }

        public Point RandomPoint()//random
        {
            return new Point(rnd.Next(width), rnd.Next(height));
        }

        public int Random(int min, int max)//random
        {
            return rnd.Next(min, max);
        }



        public void AddGrass(Grass obj, int x, int y)//Añado la grass
        {
            grass[x, y] = obj;
        }

        public void AddRabit(GameObject obj)//añado conejo
        {
            rabit = rabit.Concat(new GameObject[] { obj }).ToArray();
        }   

        public void Remove(GameObject obj)//elimino conejo
        {
            rabit = rabit.Where(o => o != obj).ToArray();
        }


        public virtual void Update()
        {
            foreach (var obj in grass)//recorro el pasto y lo actualizo
            {
                    obj.UpdateOn(this);
            }

            foreach (GameObject obj in GameObjectsRabit)//recorro los conejos y los actualizo
            {
                    obj.UpdateOn(this);
                    obj.Position = PositiveMod(obj.Position, size);
            }
        }

        public virtual void DrawOn(Graphics graphics)
        {    
            foreach (GameObject obj in grass)//dibujo el pasto
            {                    
                    graphics.FillRectangle(new Pen(obj.Color) .Brush, obj.Bounds);             
            }

            foreach (var obj in GameObjectsRabit)//Dibujo Conejos
            {
                graphics.FillRectangle(new Pen(obj.Color).Brush, obj.Bounds);
            }
        }

        // http://stackoverflow.com/a/10065670/4357302
        private static int PositiveMod(int a, int n)//calculo para que el conejo no salga de pantalla
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

        //public double Dist(PointF a, PointF b)
        //{
        //    return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        //}

        public Grass ObjectsAt(Point pos)
        {
            //divido la posicion del conejo en 2 para ubicarlo en la matriz de glass, para saber que grass estoy pisando
            float x = pos.X / 2;
            float y = pos.Y / 2;

            //Por ultimo Redondeo el resultado para buscar la Grass que estoy pisando en la matriz
            int pointX = (int)Math.Round(x);
            int pointY = (int)Math.Round(y);    

            return grass[pointX, pointY];//obtengo la grass que estoy pisando

            //if (MIX % 2 != 0)
            //{
            //    MIX++;
            //}

            //if (MIY % 2 != 0)
            //{
            //    MIY++;
            //}


            
            //Grass grass = null;
            //grass = Grass[pos.X, pos.Y];
            //return grass;

            //return GameObjects.Where(each =>
            //{
            //    Rectangle bounds = each.Bounds;
            //    PointF center = new PointF((bounds.Left + bounds.Right - 1) / 2.0f,
            //                               (bounds.Top + bounds.Bottom - 1) / 2.0f);
            //    return Dist(pos, center) <= bounds.Width / 2.0f
            //        && Dist(pos, center) <= bounds.Height / 2.0f;
            //});

        }
    }
}
