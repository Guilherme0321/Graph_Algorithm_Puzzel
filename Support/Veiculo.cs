using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph_algorithm.Support { 

    public class Veiculo<T>
    {
        private Tuple<int, int> pos;
        private T id;
        private int length;
        private char direction;

        public Veiculo(T id,int x, int y, int length, char direction)
        {
            this.pos = Tuple.Create(x, y);
            this.length = length;
            this.id = id;
            this.direction = direction;
        }

        public Veiculo()
        {
            this.pos = Tuple.Create(0, 0);
            this.length = 0;
        }

        public int Length { get { return this.length; } set { length = value; } }
        public Tuple<int, int> Position { get {  return this.pos; } set { pos = value; } }
        public T Id { get { return this.id; } }

        public char Diretion { get { return this.direction; } set { this.direction = value; } }

        public bool EqualsPosition(Veiculo<T> obj)
        {
            return this.pos.Item1 == obj.pos.Item1 && this.pos.Item2 == obj.pos.Item2;
        }

        public override string ToString()
        {
            return $"{{ {id}, <{pos.Item1},{pos.Item2}>, {direction}, {length} }}";
        }

    }
}
