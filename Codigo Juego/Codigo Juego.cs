using System;
using System.Collections.Generic;
using System.Threading;

namespace MotosDeLuz
{
    // Clase Moto
    public class Moto
    {
        // Atributos de la moto
        public int Velocidad { get; private set; }
        public int TamañoEstela { get; private set; } = 3;
        public int Combustible { get; private set; } = 100;
        public Queue<Item> Items = new Queue<Item>();
        public Stack<Poder> Poderes = new Stack<Poder>();

        public List<(int, int)> Estela = new List<(int, int)>(); // La lista de posiciones (x, y)
        private int x, y; // Posición actual de la moto

        public Moto(int startX, int startY)
        {
            Velocidad = new Random().Next(1, 11); // Velocidad entre 1 y 10
            x = startX;
            y = startY;
            // Establecemos las primeras 3 posiciones de la estela
            for (int i = 0; i < 3; i++)
            {
                Estela.Add((x - i, y));
            }
        }

        // Método para mover la moto
        public void Mover(char direccion)
        {
            // Actualizar posición según la dirección (W, A, S, D)
            switch (direccion)
            {
                case 'W': y--; break; // Arriba
                case 'A': x--; break; // Izquierda
                case 'S': y++; break; // Abajo
                case 'D': x++; break; // Derecha
            }

            // Añadir la nueva posición a la estela
            Estela.Insert(0, (x, y));

            // Si la estela supera el tamaño permitido, eliminamos el último
            if (Estela.Count > TamañoEstela)
            {
                Estela.RemoveAt(Estela.Count - 1);
            }

            // Consumo de combustible
            if (Combustible > 0)
            {
                Combustible -= Velocidad / 5;
                if (Combustible < 0)
                    Combustible = 0;
            }

            // Verificación de choque o sin combustible
            if (Combustible == 0)
            {
                DestruirMoto();
            }
        }

        // Método para destruir la moto
        public void DestruirMoto()
        {
            Console.WriteLine("La moto ha sido destruida.");
            // Colocar ítems y poderes en posiciones aleatorias del mapa
        }

        // Método para aplicar el poder del tope
        public void AplicarPoder()
        {
            if (Poderes.Count > 0)
            {
                Poder poder = Poderes.Pop();
                Console.WriteLine($"Aplicando poder: {poder.Nombre}");
                // Aplicar lógica del poder
            }
        }

        // Método para aplicar ítems con delay
        public void AplicarItems()
        {
            while (Items.Count > 0)
            {
                Item item = Items.Dequeue();
                item.Aplicar(this);
                Thread.Sleep(1000); // Delay de 1 segundo
            }
        }
    }

    // Clase Item
    public abstract class Item
    {
        public abstract void Aplicar(Moto moto);
    }

    // Clase Celda de Combustible
    public class CeldaCombustible : Item
    {
        public int Capacidad { get; private set; }

        public CeldaCombustible()
        {
            Capacidad = new Random().Next(5, 21); // Capacidad entre 5 y 20
        }

        public override void Aplicar(Moto moto)
        {
            if (moto.Combustible < 100)
            {
                moto.Combustible += Capacidad;
                if (moto.Combustible > 100)
                    moto.Combustible = 100;
                Console.WriteLine($"Celda de combustible aplicada. Combustible actual: {moto.Combustible}");
            }
            else
            {
                Console.WriteLine("Combustible lleno, celda guardada.");
                moto.Items.Enqueue(this); // Reinsertar si está lleno
            }
        }
    }

    // Clase Poder
    public abstract class Poder
    {
        public string Nombre { get; protected set; }

        public abstract void Aplicar(Moto moto);
    }

    // Clase Poder de Escudo
    public class Escudo : Poder
    {
        public Escudo()
        {
            Nombre = "Escudo";
        }

        public override void Aplicar(Moto moto)
        {
            Console.WriteLine("Moto invencible por tiempo limitado.");
            // Lógica de invulnerabilidad temporal
        }
    }

    // Clase Poder de Hiper Velocidad
    public class HiperVelocidad : Poder
    {
        public HiperVelocidad()
        {
            Nombre = "Hiper Velocidad";
        }

        public override void Aplicar(Moto moto)
        {
            int incremento = new Random().Next(1, 6); // Aumenta velocidad entre 1 y 5
            Console.WriteLine($"Velocidad aumentada en {incremento}.");
            moto.Velocidad += incremento;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Moto moto = new Moto(0, 0);

            // Agregar items y poderes de prueba
            moto.Items.Enqueue(new CeldaCombustible());
            moto.Poderes.Push(new HiperVelocidad());
            moto.Poderes.Push(new Escudo());

            // Simulación básica de movimientos
            char[] movimientos = { 'W', 'D', 'S', 'A' }; // Movimientos en el mapa
            foreach (char movimiento in movimientos)
            {
                moto.Mover(movimiento);
                Console.WriteLine($"Moto en posición: ({moto.Estela[0].Item1}, {moto.Estela[0].Item2})");
            }

            // Aplicar ítems y poderes
            moto.AplicarItems();
            moto.AplicarPoder();
        }
    }
}

