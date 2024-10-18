using System;
using graph_algorithm.Support;

namespace graph_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Veiculo<int> veiculo = new Veiculo<int>(1, 2, 3, 2, (char)Direction.Horizontal);
            Console.WriteLine(veiculo);
        }
    }

    class Graph<T>
    {
        private Estado<T> atual;
        private int hight;


    }
}