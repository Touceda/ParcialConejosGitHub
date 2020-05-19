using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
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
        private List<Rabbit> rabbit = new List<Rabbit>();

        public IEnumerable<GameObject> GameObjectsRabbit// para acceder de otros lugares
        {
            get
            {
                return rabbit.ToArray();
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

        public void AddRabit(Rabbit obj)//añado conejo
        {
            rabbit.Add(obj);
        }   

        public void Remove(Rabbit obj)//elimino conejo
        {
            rabbit.Remove(obj);
        }


        public virtual void Update()
        {
            foreach (var obj in grass)//recorro el pasto y lo actualizo
            {
                    obj.UpdateOn(this);
            }

            for (int i = 0; i < rabbit.Count; i++)
            {
                rabbit[i].UpdateOn(this);
                rabbit[i].Position = PositiveMod(rabbit[i].Position, size);
            }                                      
        }

        Pen [] MisImagenes = new Pen[256];//Creo un vector donde voy a guardar los 254 colores segun el estados del Grass(Segun el Growth) y el 255 es el color del  rabit
        public void SavePens()
        {          
            Color color;
            for (int i = 0; i < 255; i++)
            {
                color = Color.FromArgb(i, 0, 255, 0);           
                MisImagenes[i] = new Pen(color);//Guardo los colores de la Grass      
            }
            color = Color.White;
            MisImagenes[255] = new Pen(color);//GUARDO EL Color de los conejos
        }

        public virtual void DrawOn(Graphics graphics)
        {           
            foreach (Grass obj in grass)//dibujo el pasto
            {                           
                graphics.FillRectangle(MisImagenes[obj.Growth].Brush, obj.Bounds);             
            }
            

            foreach (Rabbit obj in rabbit)//Dibujo Conejos
            {
                graphics.FillRectangle(MisImagenes[255].Brush, obj.Bounds);                
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
        public Grass ObjectsAt(Point pos)
        {
            //divido la posicion del conejo en 2 para ubicarlo en la matriz de glass, para saber que grass estoy pisando
            float x = pos.X / 2;
            float y = pos.Y / 2;

            //Por ultimo Redondeo el resultado para buscar la Grass que estoy pisando en la matriz
            int pointX = (int)Math.Round(x);
            int pointY = (int)Math.Round(y);    

            return grass[pointX, pointY];//obtengo la grass que estoy pisando

        }
    }
}
