using graph_algorithm.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph_algorithm.Support
{
    public enum Direction
    {
        Vertical,
        Horizontal
    }

    public class Estado<T>
    {
        private Veiculo<T>? movedVeiculo;
        private List<Veiculo<T>> veiculos;
        private List<Tuple<int, Estado<T>>> edges;

        public Estado(Veiculo<T> moved, List<Veiculo<T>> list)
        {
            this.movedVeiculo = moved;
            this.veiculos = new List<Veiculo<T>>(list);
            this.edges = new List<Tuple<int, Estado<T>>>();
        }

        public Estado(List<Veiculo<T>> list)
        {
            this.veiculos = new List<Veiculo<T>>(list);
            this.movedVeiculo = null;
            this.edges = new List<Tuple<int, Estado<T>>>();
        }

        public Veiculo<T> MovedVeiculo { get { return this.movedVeiculo; } set { this.movedVeiculo = value; } }
        public List<Veiculo<T>> Veiculos { get { return this.veiculos; } }
        public List<Tuple<int, Estado<T>>> Edges { get { return this.edges; } }

        public void setVeiculoAt(int index, Veiculo<T> tmp)
        {
            if(index >= 0 && index < this.veiculos.Count)
            {
                veiculos[index] = tmp;
            } 
            else
            {
                throw new ArgumentOutOfRangeException(nameof(index), "O índice está fora do intervalo do array de veículos.");
            }
        }

        private bool Equal(Estado<T> obj)
        {
            if (obj == null) return false;

            if (this.veiculos.Count != obj.veiculos.Count) return false;

            for (int i = 0; i < this.veiculos.Count; i++)
            {
                if (!veiculos[i].EqualsPosition(obj.veiculos[i])) return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equal((Estado<T>)obj);
        }

        public static bool operator ==(Estado<T> i, Estado<T> j)
        {
            if (ReferenceEquals(i, null) && ReferenceEquals(j, null)) return true;
            if (ReferenceEquals(i, null) || ReferenceEquals(j, null)) return false;
            return i.Equals(j);
        }

        public static bool operator !=(Estado<T> i, Estado<T> j)
        {
            return !(i == j);
        }

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
