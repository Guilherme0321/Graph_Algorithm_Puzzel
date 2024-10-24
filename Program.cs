using System.Collections.Generic;
using System.Linq;
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
            new Veiculo<int>(14, 1, 5, 2, Direction.Vertical, false),
            new Veiculo<int>(15, 1, 7, 1, Direction.Vertical, false),
            new Veiculo<int>(16, 2, 9, 1, Direction.Vertical, false),
            new Veiculo<int>(17, 1, 9, 1, Direction.Horizontal, true),
            new Veiculo<int>(18, 0, 1, 3, Direction.Horizontal, false),
            new Veiculo<int>(19, 1, 0, 3, Direction.Horizontal, false),
            new Veiculo<int>(20, 5, 6, 3, Direction.Horizontal, false)
            };
            Estado<int> movedState = new Estado<int>(movedVehicles);

            Graph<int> graph = new Graph<int>(movedState.Veiculos[1]);
            Estado<int> estadofinal = graph.GerarGrafo(movedState, Tuple.Create(2,9));
            foreach (Veiculo<int> item in estadofinal.Veiculos)
            {
                Console.WriteLine(item);
            }
        }
    }


    public class Graph<T>
    {
        private Dictionary<Estado<T>, Estado<T>> hash; // Armazena estados já processados
        private Veiculo<T> veiculo_start; // Veículo 2, o veículo principal
        public Graph(Veiculo<T> veiculo_start)
        {
            this.veiculo_start = veiculo_start;
            this.hash = new Dictionary<Estado<T>, Estado<T>>();
        }

        public Estado<T> GerarGrafo(Estado<T> estadoInicial, Tuple<int, int> posicaoSaida)
        {
            var filaPrioridade = new SortedSet<Tuple<int, Estado<T>>>(); // Fila de prioridade com heurística
            filaPrioridade.Add(new Tuple<int, Estado<T>>(0, estadoInicial));

            while (filaPrioridade.Any())
            {
                var estadoAtual = filaPrioridade.First().Item2; // Pega o estado com menor heurística
                filaPrioridade.Remove(filaPrioridade.First());

                // Pega o veículo 2 (ID do veículo alvo) para verificar se ele atingiu a posição final
                var veiculo2 = estadoAtual.Veiculos[1];

                // Verificação das coordenadas do veículo 2
                if (veiculo2 == null)
                {
                    Console.WriteLine("Erro: veículo 2 não encontrado no estado atual.");
                    continue; // Pula esse estado se não encontrar o veículo 2
                }

                // Verifique se o veículo 2 alcançou a posição final
                if (veiculo2.Position.Equals(posicaoSaida))
                {
                    Console.WriteLine("Solução encontrada!");
                    return estadoAtual; // Solução encontrada
                }

                // Explorar possíveis movimentações para veículos que bloqueiam o veículo 2
                foreach (var veiculo in estadoAtual.Veiculos)
                {
                    // Verificar se o veículo bloqueia o caminho do veículo 2
                    if (BloqueiaVeiculo2(veiculo, veiculo2, posicaoSaida))
                        continue; // Ignora movimentações de veículos que não bloqueiam o veículo 2

                    // Movimentar apenas veículos que estão no caminho do veículo 2
                    for (int i = 1; i <= veiculo.Length; i++)
                    {
                        // Movimentação horizontal
                        if (veiculo.Diretion == Direction.Horizontal)
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
                        // Movimentação vertical
                        else if (veiculo.Diretion == Direction.Vertical)
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
            }

            Console.WriteLine("Nenhuma solução foi encontrada.");
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

        private int CalcularHeuristica(Veiculo<T> veiculo2, List<Veiculo<T>> veiculos, Tuple<int, int> posicaoSaida)
        {
            // A heurística agora será baseada na distância de Manhattan do veículo 2 para a posição de saída
            // e no número de veículos que bloqueiam diretamente o caminho dele
            int distancia = Math.Abs(veiculo2.Position.Item1 - posicaoSaida.Item1) + Math.Abs(veiculo2.Position.Item2 - posicaoSaida.Item2);

            // Contar quantos veículos estão bloqueando o caminho do veículo 2
            int bloqueios = veiculos.Count(v => BloqueiaVeiculo2(v, veiculo2, posicaoSaida));

            // A heurística será a soma da distância e dos bloqueios (quanto menos bloqueios melhor)
            return distancia + bloqueios * 10; // Penalizar mais veículos bloqueando o caminho
        }

        private bool BloqueiaVeiculo2(Veiculo<T> veiculo, Veiculo<T> veiculo2, Tuple<int, int> posicaoSaida)
        {
            // Verifica se o veículo está diretamente bloqueando o veículo 2
            if (veiculo.Diretion == Direction.Horizontal)
            {
                // Verificar se o veículo bloqueia na linha do veículo 2
                return veiculo.Position.Item1 == veiculo2.Position.Item1 &&
                       (veiculo.Position.Item2 > veiculo2.Position.Item2 && veiculo.Position.Item2 + veiculo.Length > veiculo2.Position.Item2);
            }
            else if (veiculo.Diretion == Direction.Vertical)
            {
                // Verificar se o veículo bloqueia na coluna do veículo 2
                return veiculo.Position.Item2 == veiculo2.Position.Item2 &&
                       (veiculo.Position.Item1 > veiculo2.Position.Item1 && veiculo.Position.Item1 + veiculo.Length > veiculo2.Position.Item1);
            }
            return false;
        }

        private bool IsMovimentoValido(Veiculo<T> novoVeiculo, List<Veiculo<T>> veiculos)
        {
            // Verifica se o movimento é válido (sem colisões e dentro dos limites)
            foreach (var veiculo in veiculos)
            {
                if (!novoVeiculo.Equals(veiculo) && novoVeiculo.HadColision(veiculo))
                {
                    return false;
                }
            }
            return true;
        }
    }
}