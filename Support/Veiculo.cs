using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace graph_algorithm.Support {
    public enum MoveVertical
    {
        Cima,
        Baixo
    }

    public enum MoveHorizontal
    {
        Esquerda,
        Direita
    }

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

        public Veiculo<T> MoveHorizontally(int movimentacao, MoveHorizontal direcao)
        {
            int y = pos.Item2 + (direcao == MoveHorizontal.Esquerda ? -movimentacao : movimentacao);
            return new Veiculo<T>(this.id, this.pos.Item1, y, this.length, this.direction, this.IsObstacle);
        }

        public Veiculo<T> MoveVertically(int movimentacao, MoveVertical direcao)
        {
            int x = pos.Item1 + (direcao == MoveVertical.Cima ? -movimentacao : movimentacao);
            return new Veiculo<T>(this.id, x, this.pos.Item2, this.length, this.direction, this.IsObstacle);
        }

        public Boolean LeftMatrix(Tuple<int, int> position, int numberOfColumns, int numberOfLines)
        {
            int x = position.Item1;
            int y = position.Item2;

            if (x < 0 || y == 0) return true;
            if (x > numberOfLines - 1|| y > numberOfColumns - 1) return true;
            return false;
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

        private bool Equal(Veiculo<T> obj)
        {
            return this.id.Equals(obj.id);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equal((Veiculo<T>)obj);
        }

        public static bool operator ==(Veiculo<T> i, Veiculo<T> j)
        {
            if (ReferenceEquals(i, null) && ReferenceEquals(j, null)) return true;
            if (ReferenceEquals(i, null) || ReferenceEquals(j, null)) return false;
            return i.Equals(j);
        }

        public static bool operator !=(Veiculo<T> i, Veiculo<T> j)
        {
            return !(i == j);
        }

        public override string ToString()
        {
            return $"{{ {id}, <{pos.Item1},{pos.Item2}>, {direction}, {length} }}";
        }

    }
}
