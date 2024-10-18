using System;
using graph_algorithm.Support;

namespace graph_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Veiculo<int> veiculo = new Veiculo<int>(1, 2, 3, 2, Direction.Horizontal);
            Console.WriteLine(veiculo);
            Console.WriteLine(veiculo.Diretion == Direction.Vertical);
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