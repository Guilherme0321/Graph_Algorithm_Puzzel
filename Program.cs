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
                new Veiculo<int>(1, 3, 1, 1, Direction.Vertical, false),
                new Veiculo<int>(2, 2, 0, 1, Direction.Vertical, false),
                new Veiculo<int>(3, 2, 1, 1, Direction.Horizontal, false),
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
            graph.GerarGrafo(2);
            Estado<int> estadofinal = graph.BuscaGulosa();
            foreach (Veiculo<int> item in estadofinal.Veiculos)
            {
                Console.WriteLine(item);
            }
        }
    }

    class Graph<T>
    {
        private Estado<T> atual;
        private int hight;
        
        private Veiculo<T> veiculo_start;
        private Tuple<int, int> end;

        private Dictionary<Estado<T>, Estado<T>> hash;

        public Graph(List<Veiculo<T>> veiculos, Veiculo<T> start, Tuple<int, int> end_position)
        {
            this.atual = new Estado<T>(veiculos);
            this.hight = 0;
            this.veiculo_start = start;
            this.end = end_position;

            hash = new Dictionary<Estado<T>, Estado<T>>()
            {
                { atual, atual }
            };
        }

        public void GerarGrafo(T veiculoIdAlvo)
        {
            Queue<Estado<T>> fila = new Queue<Estado<T>>();
            fila.Enqueue(atual);

            while (fila.Count > 0)
            {
                Estado<T> estadoAtual = fila.Dequeue();

                // Verifique se o veículo com o ID específico atingiu a posição final
                if (estadoAtual.Veiculos.Any(v => v.Id.Equals(veiculoIdAlvo) && v.Position.Equals(end)))
                {
                    Console.WriteLine("Objetivo atingido pelo veículo de ID: " + veiculoIdAlvo);
                    return;
                }

                // Gere todos os movimentos possíveis para cada veículo
                foreach (Veiculo<T> veiculo in estadoAtual.Veiculos)
                {
                    if(veiculo.Diretion == Direction.Horizontal)
                    {
                        // Movimentos horizontais
                        for (int i = 1; i <= veiculo.Length; i++)
                        {
                            // Mover para a esquerda
                            Veiculo<T> novoVeiculoEsquerda = veiculo.MoveHorizontally(i, MoveHorizontal.Esquerda);
                            if (IsMovimentoValido(novoVeiculoEsquerda, estadoAtual.Veiculos))
                            {
                                var novoEstado = AtualizarEstado(estadoAtual, novoVeiculoEsquerda);
                                if (!hash.ContainsKey(novoEstado))
                                {
                                    hash[novoEstado] = novoEstado; // Marca como analisado
                                    fila.Enqueue(novoEstado);
                                    //Console.WriteLine(novoEstado.Veiculos[2]);
                                }
                            }

                            // Mover para a direita
                            Veiculo<T> novoVeiculoDireita = veiculo.MoveHorizontally(i, MoveHorizontal.Direita);
                            if (IsMovimentoValido(novoVeiculoDireita, estadoAtual.Veiculos))
                            {
                                var novoEstado = AtualizarEstado(estadoAtual, novoVeiculoDireita);
                                if (!hash.ContainsKey(novoEstado))
                                {
                                    hash[novoEstado] = novoEstado; // Marca como analisado
                                    fila.Enqueue(novoEstado);
                                    //Console.WriteLine(novoEstado.Veiculos[2]);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= veiculo.Length; i++)
                        {
                            // Mover para cima
                            Veiculo<T> novoVeiculoCima = veiculo.MoveVertically(i, MoveVertical.Cima);
                            if (IsMovimentoValido(novoVeiculoCima, estadoAtual.Veiculos))
                            {
                                var novoEstado = AtualizarEstado(estadoAtual, novoVeiculoCima);
                                if (!hash.ContainsKey(novoEstado))
                                {
                                    hash[novoEstado] = novoEstado; // Marca como analisado
                                    fila.Enqueue(novoEstado);
                                }
                            }

                            // Mover para baixo
                            Veiculo<T> novoVeiculoBaixo = veiculo.MoveVertically(i, MoveVertical.Baixo);
                            if (IsMovimentoValido(novoVeiculoBaixo, estadoAtual.Veiculos) && !novoVeiculoBaixo.Id.Equals(2))
                            {
                                var novoEstado = AtualizarEstado(estadoAtual, novoVeiculoBaixo);
                                if (!hash.ContainsKey(novoEstado))
                                {
                                    hash[novoEstado] = novoEstado; // Marca como analisado
                                    fila.Enqueue(novoEstado);
                                }
                            }
                        }
                    }
                    // Movimentos verticais
                }
            }
        }
        private bool IsMovimentoValido(Veiculo<T> veiculo, List<Veiculo<T>> outrosVeiculos)
        {
            // Verifica se a nova posição do veículo é válida (não sai da matriz, não colide com outros veículos)
            if (veiculo.LeftMatrix(veiculo.Position, 10, 6))
            {
                return false;
            }

            foreach (var outro in outrosVeiculos)
            {
                if (veiculo.HadColision(outro))
                {
                    return false;
                }
            }

            return true;
        }

        private Estado<T> AtualizarEstado(Estado<T> estadoAtual, Veiculo<T> novoVeiculo)
        {
            // Clone a lista de veículos do estado atual
            List<Veiculo<T>> novosVeiculos = estadoAtual.Veiculos.Select(v => v.Clone()).ToList();

            // Encontre o veículo que precisa ser atualizado
            for (int i = 0; i < novosVeiculos.Count; i++)
            {
                if (novosVeiculos[i].Id.Equals(novoVeiculo.Id))
                {
                    // Atualize a posição do veículo
                    novosVeiculos[i] = novoVeiculo;
                    if(novoVeiculo.Id.Equals(18)) 
                        Console.WriteLine(novoVeiculo);
                    break;
                }
            }

            // Crie um novo estado com a lista atualizada de veículos
            Estado<T> novoEstado = new Estado<T>(novoVeiculo, novosVeiculos);

            // Adicione as arestas ao novo estado, se necessário
            // (Dependendo da lógica do seu algoritmo, talvez você queira adicionar as arestas aqui)

            return novoEstado;
        }


        public Estado<T> HasBeenAnalysed(Estado<T> estado)
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

        public Estado<T> BuscaGulosa()
        {
            Tuple<int, int> posicaoSaida = this.end;
            // Fila de prioridade para armazenar os estados a serem explorados
            SortedSet<Tuple<int, Estado<T>>> filaPrioridade = new SortedSet<Tuple<int, Estado<T>>>(Comparer<Tuple<int, Estado<T>>>.Create((x, y) =>
            {
                // Ordena pela heurística, se forem iguais, usa o hashcode do estado para desempate
                int comparacao = x.Item1.CompareTo(y.Item1);
                return comparacao != 0 ? comparacao : x.Item2.GetHashCode().CompareTo(y.Item2.GetHashCode());
            }));

            // Adiciona o estado inicial à fila de prioridade
            int heuristicaInicial = CalcularHeuristica(veiculo_start, atual.Veiculos, posicaoSaida);
            filaPrioridade.Add(new Tuple<int, Estado<T>>(heuristicaInicial, atual));

            while (filaPrioridade.Count > 0)
            {
                // Pega o estado com menor valor heurístico
                var estadoAtual = filaPrioridade.Min.Item2;
                filaPrioridade.Remove(filaPrioridade.Min);

                foreach(Veiculo<T> i in estadoAtual.Veiculos)
                {
                    if (i.Id.Equals(veiculo_start.Id) && i.Position.Item2 == end.Item2 && i.Position.Item1 == end.Item1)
                    {
                        Console.WriteLine("Encontrado");
                        return estadoAtual;
                    }
                }

                // Verifica se o estado atual é o estado final

                // Gera os estados vizinhos e adiciona à fila de prioridade
                foreach (Veiculo<T> veiculo in estadoAtual.Veiculos)
                {
                    // Movimentos horizontais
                    for (int i = 1; i <= veiculo.Length; i++)
                    {
                        // Mover para a esquerda
                        Veiculo<T> novoVeiculoEsquerda = veiculo.MoveHorizontally(i, MoveHorizontal.Esquerda);
                        if (IsMovimentoValido(novoVeiculoEsquerda, estadoAtual.Veiculos))
                        {
                            AtualizarEstado(estadoAtual, novoVeiculoEsquerda, posicaoSaida, filaPrioridade);
                        }

                        // Mover para a direita
                        Veiculo<T> novoVeiculoDireita = veiculo.MoveHorizontally(i, MoveHorizontal.Direita);
                        if (IsMovimentoValido(novoVeiculoDireita, estadoAtual.Veiculos))
                        {
                            AtualizarEstado(estadoAtual, novoVeiculoDireita, posicaoSaida, filaPrioridade);
                        }
                    }

                    // Movimentos verticais
                    for (int i = 1; i <= veiculo.Length; i++)
                    {
                        // Mover para cima
                        Veiculo<T> novoVeiculoCima = veiculo.MoveVertically(i, MoveVertical.Cima);
                        if (IsMovimentoValido(novoVeiculoCima, estadoAtual.Veiculos))
                        {
                            AtualizarEstado(estadoAtual, novoVeiculoCima, posicaoSaida, filaPrioridade);
                        }

                        // Mover para baixo
                        Veiculo<T> novoVeiculoBaixo = veiculo.MoveVertically(i, MoveVertical.Baixo);
                        if (IsMovimentoValido(novoVeiculoBaixo, estadoAtual.Veiculos))
                        {
                            AtualizarEstado(estadoAtual, novoVeiculoBaixo, posicaoSaida, filaPrioridade);
                        }
                    }
                }
            }

            return null; // Retorna nulo se não encontrar o estado final
        }

        private void AtualizarEstado(Estado<T> estadoAtual, Veiculo<T> novoVeiculo, Tuple<int, int> posicaoSaida, SortedSet<Tuple<int, Estado<T>>> filaPrioridade)
        {
            // Atualiza o estado com o novo veículo
            List<Veiculo<T>> novosVeiculos = estadoAtual.Veiculos.Select(v => v.Clone()).ToList();

            // Substitua o veículo na lista clonada
            int index = novosVeiculos.FindIndex(v => v.Id.Equals(novoVeiculo.Id));
            if (index != -1)
            {
                novosVeiculos[index] = novoVeiculo;
            }

            Estado<T> novoEstado = new Estado<T>(novosVeiculos);

            if (!hash.ContainsKey(novoEstado))
            {
                hash[novoEstado] = novoEstado; // Marca como analisado
                int heuristica = CalcularHeuristica(veiculo_start, novosVeiculos, posicaoSaida);
                filaPrioridade.Add(new Tuple<int, Estado<T>>(heuristica, novoEstado)); // Adiciona o novo estado com sua heurística
            }
        }

    }
}