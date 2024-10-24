using System;
using System.Diagnostics.CodeAnalysis;
using graph_algorithm.Support;

namespace graph_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Veiculo<int>> movedVehicles = new List<Veiculo<int>>() {
                new Veiculo<int>(1, 3, 1, 1, Direction.Horizontal, false),
                new Veiculo<int>(2, 2, 0, 1, Direction.Horizontal, false),
                new Veiculo<int>(3, 2, 1, 1, Direction.Vertical, false),
                new Veiculo<int>(4, 3, 0, 1, Direction.Horizontal, true),
                new Veiculo<int>(5, 3, 3, 2, Direction.Vertical, false),
                new Veiculo<int>(6, 1, 4, 1, Direction.Horizontal, false),
                new Veiculo<int>(7, 5, 5, 1, Direction.Horizontal, true),
                new Veiculo<int>(8, 2, 7, 1, Direction.Vertical, false),
                new Veiculo<int>(9, 4, 6, 1, Direction.Vertical, false),
                new Veiculo<int>(10, 4, 5, 1, Direction.Horizontal, false),
                new Veiculo<int>(11, 4, 0, 2, Direction.Vertical, false),
                new Veiculo<int>(12, 3, 9, 2, Direction.Vertical, false),
                new Veiculo<int>(13, 2, 6, 2, Direction.Vertical, false),
                new Veiculo<int>(14, 1, 5, 2, Direction.Horizontal, false),
                new Veiculo<int>(15, 1, 7, 1, Direction.Vertical, false),
                new Veiculo<int>(16, 2, 9, 1, Direction.Vertical, false),
                new Veiculo<int>(17, 1, 9, 1, Direction.Horizontal, true),
                new Veiculo<int>(18, 0, 1, 3, Direction.Horizontal, false),
                new Veiculo<int>(19, 1, 0, 3, Direction.Horizontal, false),
                new Veiculo<int>(20, 5, 6, 3, Direction.Horizontal, false)
            };
            Estado<int> movedState = new Estado<int>(movedVehicles);

            Graph<int> graph = new Graph<int>(movedVehicles, movedVehicles[1], new Tuple<int, int>(2, 9));

            // Gera o grafo (opcional, dependendo da sua implementação da busca)
            // graph.GerarGrafo(18); // 18 é o ID do carro vermelho

            List<Movimento> solucao = graph.BuscarSolucao(2); // Busca a solução

            if (solucao != null)
            {
                Console.WriteLine("Solução encontrada:");
                foreach (var movimento in solucao)
                {
                    Console.WriteLine(movimento);
                }
            }
            else
            {
                Console.WriteLine("Solução não encontrada.");
            }
        }
    }


    public class Movimento
    {
        public int VeiculoId { get; set; }
        public Direction Direcao { get; set; }
        public int Quantidade { get; set; }

        public Movimento(int veiculoId, Direction direcao, int quantidade)
        {
            VeiculoId = veiculoId;
            Direcao = direcao;
            Quantidade = quantidade;
        }

        public override string ToString()
        {
            string direcaoStr = Direcao == Direction.Horizontal ? "Horizontal" : "Vertical";
            string sentidoStr = "";

            if (Direcao == Direction.Horizontal)
            {
                sentidoStr = Quantidade > 0 ? "Direita" : "Esquerda";
            }
            else
            {
                sentidoStr = Quantidade > 0 ? "Baixo" : "Cima";
            }

            return $"{{ Veiculo: {VeiculoId}, Direção: {direcaoStr}, Sentido: {sentidoStr}, Quantidade: {Math.Abs(Quantidade)} }}";
        }
    }


    class Graph<T>
    {
        private Estado<T> atual;
        private Veiculo<T> veiculo_start;
        private Tuple<int, int> end;
        private Queue<Tuple<Estado<T>, List<Movimento>>> fila;

        public Graph(List<Veiculo<T>> veiculos, Veiculo<T> start, Tuple<int, int> end_position)
        {
            fila = new Queue<Tuple<Estado<T>, List<Movimento>>>();
            this.atual = new Estado<T>(veiculos);
            this.veiculo_start = start;
            this.end = end_position;
        }

        private void MoverBloqueadoresRecursivamente(Estado<T> estadoAtual, Veiculo<T> veiculoAtual, Veiculo<T> veiculoAnterior, List<Movimento> movimentosAteAqui, HashSet<Estado<T>> visitados, Queue<Tuple<Estado<T>, List<Movimento>>> fila)
        {
            List<Veiculo<T>> bloqueadores = ObterVeiculosBloqueadores(estadoAtual, veiculoAtual);

            if (bloqueadores.Count == 0)
            {
                // Tenta mover o veículo atual APENAS se ele for o carro vermelho
                if (veiculoAtual.Id.Equals(veiculo_start.Id)) // Verifica se é o carro vermelho
                {
                    List<Tuple<Estado<T>, Movimento>> vizinhos = GerarVizinhos(estadoAtual, veiculoAtual);
                    AdicionarVizinhosAFila(vizinhos, visitados, movimentosAteAqui, fila);
                }
                return; // Sai da recursão se não houver bloqueadores ou se não for o carro vermelho
            }

            foreach (Veiculo<T> bloqueador in bloqueadores.ToList()) // Cria uma cópia da lista para iterar
            {
                List<Tuple<Estado<T>, Movimento>> vizinhosBloqueador = GerarVizinhos(estadoAtual, bloqueador);

                foreach (var (vizinho, movimento) in vizinhosBloqueador)
                {
                    if (!visitados.Contains(vizinho))
                    {
                        visitados.Add(vizinho);
                        var novosMovimentos = new List<Movimento>(movimentosAteAqui) { movimento };

                        bool algumBloqueadorRemovido = bloqueadores.Any(b => !ObterVeiculosBloqueadores(vizinho, veiculoAtual).Contains(b));

                        Veiculo<T> novoVeiculoAtual = vizinho.Veiculos.First(v => v.Id.Equals(veiculoAtual.Id));
                        List<Tuple<Estado<T>, Movimento>> vizinhosAtual = GerarVizinhos(vizinho, novoVeiculoAtual);
                        AdicionarVizinhosAFila(vizinhosAtual, visitados, novosMovimentos, fila);

                        if (algumBloqueadorRemovido)
                        {
                            // Adiciona o estado atual com os novos movimentos à fila
                            fila.Enqueue(new Tuple<Estado<T>, List<Movimento>>(vizinho, novosMovimentos));

                            return; // Sai do loop e da recursão atual
                        }

                        MoverBloqueadoresRecursivamente(vizinho, novoVeiculoAtual, bloqueador, novosMovimentos, visitados, fila);
                    }
                }
            }
        }


        public List<Movimento> BuscarSolucao(T veiculoIdAlvo)
        {
            fila.Clear();
            fila.Enqueue(new Tuple<Estado<T>, List<Movimento>>(atual, new List<Movimento>()));

            HashSet<Estado<T>> visitados = new HashSet<Estado<T>>();
            visitados.Add(atual);

            while (fila.Count > 0)
            {
                var (estadoAtual, movimentosAteAqui) = fila.Dequeue();

                if (estadoAtual.Veiculos.Any(v => v.Id.Equals(veiculoIdAlvo) && v.Position.Equals(end)))
                {
                    return movimentosAteAqui; // Solução encontrada
                }

                Veiculo<T> carroVermelho = estadoAtual.Veiculos.First(v => v.Id.Equals(veiculoIdAlvo));

                // Tenta mover o carro vermelho primeiro
                List<Tuple<Estado<T>, Movimento>> vizinhosVermelho = GerarVizinhos(estadoAtual, carroVermelho);
                AdicionarVizinhosAFila(vizinhosVermelho, visitados, movimentosAteAqui, fila);

                // Se o carro vermelho não pôde ser movido, move os bloqueadores
                List<Veiculo<T>> bloqueadores = ObterVeiculosBloqueadores(estadoAtual, carroVermelho); // consegui movimentar o carro ate 2,5 dps nao deu

                if (bloqueadores.Count > 0)
                {
                    foreach (Veiculo<T> bloqueador in bloqueadores)
                    {
                        List<Veiculo<T>> bloqueadoresDoBloqueador = ObterVeiculosBloqueadores(estadoAtual, bloqueador);

                        if (bloqueadoresDoBloqueador.Count > 0)
                        {
                            // Move os bloqueadores do bloqueador recursivamente
                            MoverBloqueadoresRecursivamente(estadoAtual, bloqueador, carroVermelho, movimentosAteAqui, visitados, fila);
                        }
                        else
                        {
                            // Bloqueador não está bloqueado, gera vizinhos normalmente
                            List<Tuple<Estado<T>, Movimento>> vizinhos = GerarVizinhos(estadoAtual, bloqueador);
                            AdicionarVizinhosAFila(vizinhos, visitados, movimentosAteAqui, fila);
                        }
                    }
                }
            }

            return null; // Sem solução encontrada
        }

        // Método auxiliar para adicionar vizinhos à fila
        private void AdicionarVizinhosAFila(List<Tuple<Estado<T>, Movimento>> vizinhos, HashSet<Estado<T>> visitados, List<Movimento> movimentosAteAqui, Queue<Tuple<Estado<T>, List<Movimento>>> fila)
        {
            foreach (var (vizinho, movimento) in vizinhos)
            {
                if (!visitados.Contains(vizinho))
                {
                    visitados.Add(vizinho);
                    var novosMovimentos = new List<Movimento>(movimentosAteAqui) { movimento };
                    fila.Enqueue(new Tuple<Estado<T>, List<Movimento>>(vizinho, novosMovimentos));
                }
            }
        }

        private List<Veiculo<T>> ObterVeiculosBloqueadores(Estado<T> estado, Veiculo<T> carroVermelho)
        {
            List<Veiculo<T>> bloqueadores = new List<Veiculo<T>>();

            if (carroVermelho.Diretion == Direction.Horizontal)
            {
                // Verifica bloqueio à direita
                Veiculo<T> movidoDireita = carroVermelho.MoveHorizontally(1, MoveHorizontal.Direita);
                bloqueadores.AddRange(estado.Veiculos.Where(v => !v.Id.Equals(carroVermelho.Id) && movidoDireita.HadColision(v)));

                // Verifica bloqueio à esquerda
                Veiculo<T> movidoEsquerda = carroVermelho.MoveHorizontally(1, MoveHorizontal.Esquerda);
                bloqueadores.AddRange(estado.Veiculos.Where(v => !v.Id.Equals(carroVermelho.Id) && movidoEsquerda.HadColision(v)));
            }
            else // Vertical
            {
                // Verifica bloqueio para baixo
                Veiculo<T> movidoBaixo = carroVermelho.MoveVertically(1, MoveVertical.Baixo);
                bloqueadores.AddRange(estado.Veiculos.Where(v => !v.Id.Equals(carroVermelho.Id) && movidoBaixo.HadColision(v)));

                // Verifica bloqueio para cima
                Veiculo<T> movidoCima = carroVermelho.MoveVertically(1, MoveVertical.Cima);
                bloqueadores.AddRange(estado.Veiculos.Where(v => !v.Id.Equals(carroVermelho.Id) && movidoCima.HadColision(v)));
            }

            return bloqueadores.Distinct().ToList(); // Remove duplicatas
        }
       
        private List<Tuple<Estado<T>, Movimento>> GerarVizinhos(Estado<T> estadoAtual, Veiculo<T> veiculo)
        {
            List<Tuple<Estado<T>, Movimento>> vizinhos = new List<Tuple<Estado<T>, Movimento>>();

            // Calcula os movimentos em bloco para todas as direções
            if (veiculo.Diretion == Direction.Horizontal)
            {
                int maxEsquerda = CalcularMaxMovimentoHorizontal(veiculo, estadoAtual.Veiculos, MoveHorizontal.Esquerda);
                if (maxEsquerda != 0)
                {
                    AdicionarVizinho(estadoAtual, veiculo, vizinhos, Direction.Horizontal, -maxEsquerda); // Esquerda
                }
                 
                int maxDireita = CalcularMaxMovimentoHorizontal(veiculo, estadoAtual.Veiculos, MoveHorizontal.Direita);
                if (maxDireita != 0)
                {
                    AdicionarVizinho(estadoAtual, veiculo, vizinhos, Direction.Horizontal, maxDireita); // Direita
                }
            }
            else // Vertical
            {
                int maxCima = CalcularMaxMovimentoVertical(veiculo, estadoAtual.Veiculos, MoveVertical.Cima);
                if (maxCima != 0)
                {
                    AdicionarVizinho(estadoAtual, veiculo, vizinhos, Direction.Vertical, -maxCima); // Cima
                }

                int maxBaixo = CalcularMaxMovimentoVertical(veiculo, estadoAtual.Veiculos, MoveVertical.Baixo);
                if (maxBaixo != 0)
                {
                    AdicionarVizinho(estadoAtual, veiculo, vizinhos, Direction.Vertical, maxBaixo); // Baixo
                }
            }

            return vizinhos;
        }

        private void AdicionarVizinho(Estado<T> estadoAtual, Veiculo<T> veiculo, List<Tuple<Estado<T>, Movimento>> vizinhos, Direction direcao, int quantidade)
        {
            Veiculo<T> novoVeiculo = direcao == Direction.Horizontal
                ? veiculo.MoveHorizontally(Math.Abs(quantidade), quantidade > 0 ? MoveHorizontal.Direita : MoveHorizontal.Esquerda)
                : veiculo.MoveVertically(Math.Abs(quantidade), quantidade > 0 ? MoveVertical.Baixo : MoveVertical.Cima);

            if (IsMovimentoValido(novoVeiculo, estadoAtual.Veiculos))
            {
                Estado<T> novoEstado = AtualizarEstado(estadoAtual, novoVeiculo);
                Movimento movimento = new Movimento((int)(object)veiculo.Id, direcao, quantidade); // Crie o objeto Movimento
                vizinhos.Add(new Tuple<Estado<T>, Movimento>(novoEstado, movimento));
            }
        }


        private int CalcularMaxMovimentoHorizontal(Veiculo<T> veiculo, List<Veiculo<T>> outrosVeiculos, MoveHorizontal direcao)
        {
            int maxMovimento = 0;
            int incremento = direcao == MoveHorizontal.Direita ? 1 : -1;

            for (int i = 1; ; i++)
            {
                Veiculo<T> novoVeiculo = veiculo.MoveHorizontally(i, direcao);

                if (!IsMovimentoValido(novoVeiculo, outrosVeiculos))
                {
                    break;
                }

                maxMovimento = i;
            }

            return maxMovimento * incremento; // Retorna a quantidade de movimento com o sinal correto
        }

        private int CalcularMaxMovimentoVertical(Veiculo<T> veiculo, List<Veiculo<T>> outrosVeiculos, MoveVertical direcao)
        {
            int maxMovimento = 0;
            int incremento = direcao == MoveVertical.Baixo ? 1 : -1;

            for (int i = 1; ; i++)
            {
                Veiculo<T> novoVeiculo = veiculo.MoveVertically(i, direcao);

                if (!IsMovimentoValido(novoVeiculo, outrosVeiculos))
                {
                    break;
                }

                maxMovimento = i;
            }

            return maxMovimento * incremento; // Retorna a quantidade de movimento com o sinal correto
        }

        private bool IsMovimentoValido(Veiculo<T> veiculo, List<Veiculo<T>> outrosVeiculos)
        {
            if (veiculo.LeftMatrix(veiculo.Position, 10, 6)) // Substitua 10 e 6 pelas dimensões do seu tabuleiro
            {
                return false; // Saiu do tabuleiro
            }

            foreach (var outro in outrosVeiculos)
            {
                if (!veiculo.Id.Equals(outro.Id) && veiculo.HadColision(outro))
                {
                    return false; // Colisão com outro veículo
                }
            }

            return true; // Movimento válido
        }

        private Estado<T> AtualizarEstado(Estado<T> estadoAtual, Veiculo<T> novoVeiculo)
        {
            List<Veiculo<T>> novosVeiculos = new List<Veiculo<T>>();
            foreach (var veiculo in estadoAtual.Veiculos)
            {
                novosVeiculos.Add(veiculo.Clone());
            }

            for (int i = 0; i < novosVeiculos.Count; i++)
            {
                if (novosVeiculos[i].Id.Equals(novoVeiculo.Id))
                {
                    novosVeiculos[i] = novoVeiculo;
                    break;
                }
            }

            return new Estado<T>(novosVeiculos);
        }
    }
}