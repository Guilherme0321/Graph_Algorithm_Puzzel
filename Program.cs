using System;
using graph_algorithm.Support;

namespace graph_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Veiculo<int> veiculo = new Veiculo<int>(1, 2, 3, 2, Direction.Horizontal, false);
            List<Veiculo<int>> list = new List<Veiculo<int>>();
            list.Add(veiculo);
            Estado<int> estado = new Estado<int>(list);

            List<Veiculo<int>> list2 = new List<Veiculo<int>>();
            Veiculo<int> tmp = veiculo.Clone();
            tmp.Position = Tuple.Create(2,4);
            list2.Add(tmp);
            Estado<int> estado2 = new Estado<int>(list2);
            Console.WriteLine(estado == estado2);
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