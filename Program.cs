using System;
using graph_algorithm.Support;

namespace graph_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            // Criação de veículos para o estado inicial
            Veiculo<int> v1 = new Veiculo<int>(1, 0, 0, 2, Direction.Horizontal, false); 
            Veiculo<int> v2 = new Veiculo<int>(2, 1, 0, 3, Direction.Vertical, false); 
            Veiculo<int> v3 = new Veiculo<int>(3, 0, 1, 2, Direction.Horizontal, true); // obstáculo

            // Criação do estado inicial
            List<Veiculo<int>> initialVehicles = new List<Veiculo<int>>() { v1, v2, v3 };
            Estado<int> initialState = new Estado<int>(initialVehicles);

            // Movendo o veículo 1 para a direita
            v1.MoveHorizontally(1, Veiculo<int>.MoveHorizontal.Direita);

            // Criação do novo estado após mover o veículo 1
            List<Veiculo<int>> movedVehicles = new List<Veiculo<int>>() { v1, v2, v3 };
            Estado<int> movedState = new Estado<int>(v1, movedVehicles);

            // Verificando se houve colisão após mover o veículo 1
            foreach (Veiculo<int> vehicle in movedState.Veiculos)
            {
                if (vehicle != v1 && v1.HadColision(vehicle))
                {
                    Console.WriteLine($"O veículo {v1.Id} colidiu com o veículo {vehicle.Id}");
                }
            }


        }
    }

    class Graph<T>
    {
        private Estado<T> atual;
        private int hight;
        
        private Veiculo<T> veiculo_start;
        private Tuple<int, int> end;

        public Graph(List<Veiculo<T>> veiculos, Veiculo<T> start, Tuple<int, int> end_position)
        {
            this.atual = new Estado<T>(veiculos);
            this.hight = 0;
            this.veiculo_start = start;
            this.end = end_position;
        }

        

    }
}