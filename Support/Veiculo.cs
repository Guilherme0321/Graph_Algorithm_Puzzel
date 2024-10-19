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
        private Direction direction;
        private bool isObstacle;

        public Veiculo(T id,int x, int y, int length, Direction direction, bool isObstacle)
        {
            this.pos = Tuple.Create(x, y);
            this.length = length;
            this.id = id;
            this.direction = direction;
            this.isObstacle = isObstacle;
        }

        public Veiculo()
        {
            this.pos = Tuple.Create(0, 0);
            this.length = 0;
        }


        public int Length { get { return this.length; } set { length = value; } }
        public Tuple<int, int> Position { get {  return this.pos; } set { pos = value; } }
        public T Id { get { return this.id; } }

        public Direction Diretion { get { return this.direction; } set { this.direction = value; } }

        public bool IsObstacle { get {  return this.isObstacle; } set { this.isObstacle = value; } }

        public bool EqualsPosition(Veiculo<T> obj)
        {
            return this.pos.Item1 == obj.pos.Item1 && this.pos.Item2 == obj.pos.Item2;
        }

        public bool HadColision(Veiculo<T> v2)
        {
            if (this.Diretion == Direction.Vertical)
            {
                for (int i = 0; i < this.Length; i++)
                {
                    for (int j = 0; j < v2.Length; j++)
                    {
                        int Xv2 = v2.Position.Item1 + (v2.Diretion == Direction.Vertical ? j : 0);
                        int Yv2 = v2.Position.Item2 + (v2.Diretion == Direction.Horizontal ? j : 0);
                        if (this.Position.Item1 + i == Xv2 && this.Position.Item2 == Yv2)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.Length; i++)
                {
                    for (int j = 0; j < v2.Length; j++)
                    {
                        int Xv2 = v2.Position.Item1 + (v2.Diretion == Direction.Vertical ? j : 0);
                        int Yv2 = v2.Position.Item2 + (v2.Diretion == Direction.Horizontal ? j : 0);
                        if (this.Position.Item1 == Xv2 && this.Position.Item2 + i == Yv2)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public Veiculo<T> Clone()
        {
            return new Veiculo<T>(id, pos.Item1, pos.Item2, length, direction, isObstacle);
        }

        public override string ToString()
        {
            return $"{{ {id}, <{pos.Item1},{pos.Item2}>, {direction}, {length} }}";
        }

    }
}
