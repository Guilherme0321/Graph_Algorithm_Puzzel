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

            // Movendo o veículo 1 para a direita
            v1 = v1.MoveHorizontally(1, MoveHorizontal.Direita);

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

        private Dictionary<Estado<T>, bool> hash;

        public Graph(List<Veiculo<T>> veiculos, Veiculo<T> start, Tuple<int, int> end_position)
        {
            this.atual = new Estado<T>(veiculos);
            this.hight = 0;
            this.veiculo_start = start;
            this.end = end_position;

            hash = new Dictionary<Estado<T>, bool>()
            {
                { atual, true }
            };
        }

        public bool HasBeenAnalysed(Estado<T> estado)
        {
            return hash[estado];
        }

        public int CalcularHeuristica(Veiculo<T> carroVermelho, List<Veiculo<T>> outrosVeiculos, Tuple<int, int> posicaoSaida)
        {
            int heuristica = 0;

            int xAtual = carroVermelho.Position.Item1;
            int yAtual = carroVermelho.Position.Item2;

            int xFinal = posicaoSaida.Item1;
            int yFinal = posicaoSaida.Item2;

            heuristica += Math.Abs(xAtual - xFinal) + Math.Abs(yAtual - yFinal);

            // Considera os veículos que estão bloqueando o caminho do carro vermelho
            foreach (var veiculo in outrosVeiculos)
            {
                if (veiculo.Position.Item2 == carroVermelho.Position.Item2 && veiculo.Position.Item1 > carroVermelho.Position.Item1)
                {
                    heuristica += 1;
                }
            }

            return heuristica;
        }


    }
}