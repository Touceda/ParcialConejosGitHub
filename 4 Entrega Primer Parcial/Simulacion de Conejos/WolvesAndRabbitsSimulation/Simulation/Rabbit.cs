using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using WolvesAndRabbitsSimulation.Engine;

namespace WolvesAndRabbitsSimulation.Simulation
{
    class Rabbit : GameObject
    {
        private int DEATH_AGE = 1500; //Año De Muerte
        private int ADULTHOOD = 100; // Es Adulto
        private int FOOD_TO_BREED = 100; //Comida para tener un Hijo
        private int FOOD_CONSUMPTION = 25; // Comida Consumida
        private int MAX_CHILDREN = 30; // Max Hijos
        private double BREED_PROBABILITY = 0.3; //Probabilidad de tener hijos
        private double DEATH_PROBABILITY = 0.5; //Probabilidad de morir 

        private int age = 0;// Edad
        private int food = 500; //Comida

        public Rabbit()
        {
            Color = Color.White;
        }

        public override void UpdateOn(World forest)// Se actualiza el estado del conejo
        {
            EatSomeGrass(forest);
            Breed(forest);
            GrowOld(forest);
            ConsumeFood(forest);
            Wander(forest);
        }

        private void EatSomeGrass(World forest)//Come Grass
        {

            Grass grass = forest.ObjectsAt(Position);
            //Grass grass = forest.ObjectsAt(Position).Select(o => o as Grass).First(o => o != null);//creo una copia de la grass que va a comer
            int amount = FOOD_CONSUMPTION * 2;//lo que llena la barra de comida
            if (grass.Growth < amount)
            {
                amount = grass.Growth;
            }
            grass.Growth -= amount;
            food += amount;
        }

        private void Breed(World forest)//Tiene Hijos 
        {
            if (age < ADULTHOOD || food < FOOD_TO_BREED) return;
            //if (forest.ObjectsAt(Position).Any(o => o is Rabbit && o != this))//no hace falta
                for (int i = 0; i < MAX_CHILDREN; i++)
                {
                    if (forest.Random(1, 10) <= 10 * BREED_PROBABILITY)
                    {
                        Rabbit bunny = new Rabbit();
                        bunny.Position = Position;
                        forest.AddRabit(bunny);
                    }
                }
            
        }

        private void GrowOld(World forest)// Mira la edad y muere
        {
            age++;
            if (age >= DEATH_AGE) { Die(forest); }
        }

        private void ConsumeFood(World forest) //Consume comida
        {
            food -= FOOD_CONSUMPTION;
            if (food <= 0) { Die(forest); }
        }

        private void Die(World forest) // Probabilidad de morir
        {
            if (forest.Random(1, 10) <= 10 * DEATH_PROBABILITY)
            {
                forest.Remove(this);
            }
        }

        private void Wander(World forest) //Actualiza position 
        {
            Forward(forest.Random(1, 5));
            Turn(forest.Random(-100, 100));
        }
    }
}
