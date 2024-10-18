using graph_algorithm.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph_algorithm.Support
{
    enum Direction
    {
        Vertical = 'v',
        Horizontal = 'h'
    }

    class Estado<T>
    {
        public Veiculo<T>? moved_veiculo;
        public List<Veiculo<T>> veiculos;

        public List<Tuple<int, Estado<T>>>? edges;

        public Estado()
        {
            veiculos = null;
            moved_veiculo = null;
            edges = null;
        }
        public Estado(Veiculo<T> moved)
        {
            this.moved_veiculo = moved;
            this.veiculos = new List<Veiculo<T>>();
            this.edges = new List<Tuple<int, Estado<T>>>();
        }

        public bool Equals(Estado<T> obj)
        {
            if(obj == null) return false;
            for(int i = 0; i < this.veiculos.Count; i++) 
            {
                if (this.veiculos[i].EqualsPosition(obj.veiculos[i])) return false;
            }
            return true;
        }

        public static bool operator ==(Estado<T> i, Estado<T> j) { return i.Equals(j); }

        public static bool operator !=(Estado<T> i, Estado<T> j) { return !(i == j); }

        public override int GetHashCode()
        {
            int hash = 17;

            foreach (Veiculo<T> i in veiculos)
            {
                hash = hash * 31 + i.Position.Item1.GetHashCode();
                hash = hash * 31 + i.Position.Item2.GetHashCode();
            }

            return hash;
        }
    }
}
