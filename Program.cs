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

            List<Veiculo> movedVehicles = new List<Veiculo>() {
            new Veiculo("1", 3, 1, 1, Direction.Horizontal, false),
            new Veiculo("2", 2, 0, 1, Direction.Horizontal, false),
            new Veiculo("3", 2, 1, 1, Direction.Vertical, false),
            new Veiculo("4", 3, 0, 1, Direction.Horizontal, true),
            new Veiculo("5", 3, 3, 2, Direction.Vertical, false),
            new Veiculo("6", 1, 4, 1, Direction.Horizontal, false),
            new Veiculo("7", 5, 5, 1, Direction.Horizontal, true),
            new Veiculo("8", 2, 7, 1, Direction.Vertical, false),
            new Veiculo("9", 4, 6, 1, Direction.Vertical, false),
            new Veiculo("10", 4, 5, 1, Direction.Horizontal, false),
            new Veiculo("11", 4, 0, 2, Direction.Vertical, false),
            new Veiculo("12", 3, 9, 2, Direction.Vertical, false),
            new Veiculo("13", 2, 6, 2, Direction.Vertical, false),
            new Veiculo("14", 1, 5, 2, Direction.Horizontal, false),
            new Veiculo("15", 1, 7, 1, Direction.Vertical, false),
            new Veiculo("16", 2, 9, 1, Direction.Vertical, false),
            new Veiculo("17", 1, 9, 1, Direction.Horizontal, true),
            new Veiculo("18", 0, 1, 3, Direction.Horizontal, false),
            new Veiculo("19", 1, 0, 3, Direction.Horizontal, false),
            new Veiculo("20", 5, 6, 3, Direction.Horizontal, false)
            };
            Estado movedState = new Estado(movedVehicles);

            Graph graph = new Graph(movedState.Veiculos[1]);
            Estado estadofinal = graph.GerarGrafo(movedState, Tuple.Create(2,9));
            foreach (Veiculo item in estadofinal.Veiculos)
            {
                Console.WriteLine(item);
            }
        }
    }


    public class Graph
    {
        private Dictionary<Estado, Estado> hash; // Armazena estados já processados
        private Veiculo veiculo_start; // Veículo 2, o veículo principal
        public Graph(Veiculo veiculo_start)
        {
            this.veiculo_start = veiculo_start;
            this.hash = new Dictionary<Estado, Estado>();
        }

        public Estado GerarGrafo(Estado estadoInicial, Tuple<int, int> posicaoSaida)
        {
            var filaPrioridade = new MinHeap(); // Fila de prioridade com heurística
            filaPrioridade.Insert(new Tuple<int, Estado>(0, estadoInicial));

            while (!filaPrioridade.IsEmpty())
            {
                var estadoAtual = filaPrioridade.Peek().Item2; // Pega o estado com menor heurística
                var heuristica = filaPrioridade.ExtractMin().Item1;
                // Pega o veículo 2 (ID do veículo alvo) para verificar se ele atingiu a posição final
                var veiculo2 = estadoAtual.Veiculos[1];

                Console.WriteLine($"{estadoAtual.Veiculos[1]} {heuristica}");

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
                            Veiculo novoVeiculoEsquerda = veiculo.MoveHorizontally(i, MoveHorizontal.Esquerda);
                            if (IsMovimentoValido(novoVeiculoEsquerda, estadoAtual.Veiculos))
                            {
                                AtualizarEstado(estadoAtual, novoVeiculoEsquerda, posicaoSaida, filaPrioridade);
                            }

                            // Mover para a direita
                            Veiculo novoVeiculoDireita = veiculo.MoveHorizontally(i, MoveHorizontal.Direita);
                            if (IsMovimentoValido(novoVeiculoDireita, estadoAtual.Veiculos))
                            {
                                AtualizarEstado(estadoAtual, novoVeiculoDireita, posicaoSaida, filaPrioridade);
                            }
                        }
                        // Movimentação vertical
                        else if (veiculo.Diretion == Direction.Vertical)
                        {
                            // Mover para cima
                            Veiculo novoVeiculoCima = veiculo.MoveVertically(i, MoveVertical.Cima);
                            if (IsMovimentoValido(novoVeiculoCima, estadoAtual.Veiculos))
                            {
                                AtualizarEstado(estadoAtual, novoVeiculoCima, posicaoSaida, filaPrioridade);
                            }

                            // Mover para baixo
                            Veiculo novoVeiculoBaixo = veiculo.MoveVertically(i, MoveVertical.Baixo);
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


        private void AtualizarEstado(Estado estadoAtual, Veiculo novoVeiculo, Tuple<int, int> posicaoSaida, MinHeap filaPrioridade)
        {
            // Atualiza o estado com o novo veículo
            List<Veiculo> novosVeiculos = estadoAtual.Veiculos.Select(v => v.Clone()).ToList();

            // Substitua o veículo na lista clonada
            int index = novosVeiculos.FindIndex(v => v.Id.Equals(novoVeiculo.Id));
            if (index != -1)
            {
                novosVeiculos[index] = novoVeiculo;
            }

            Estado novoEstado = new Estado(novosVeiculos);

            if (!hash.ContainsKey(novoEstado))
            {
                hash[novoEstado] = novoEstado; // Marca como analisado
                int heuristica = CalcularHeuristica(veiculo_start, novosVeiculos, posicaoSaida);
                filaPrioridade.Insert(new Tuple<int, Estado>(heuristica, novoEstado)); // Adiciona o novo estado com sua heurística
            }
        }

        private int CalcularHeuristica(Veiculo veiculo2, List<Veiculo> veiculos, Tuple<int, int> posicaoSaida)
        {
            // A heurística agora será baseada na distância de Manhattan do veículo 2 para a posição de saída
            // e no número de veículos que bloqueiam diretamente o caminho dele
            int distancia = Math.Abs(veiculo2.Position.Item1 - posicaoSaida.Item1) + Math.Abs(veiculo2.Position.Item2 - posicaoSaida.Item2);

            // Contar quantos veículos estão bloqueando o caminho do veículo 2
            int bloqueios = veiculos.Count(v => BloqueiaVeiculo2(v, veiculo2, posicaoSaida));

            // A heurística será a soma da distância e dos bloqueios (quanto menos bloqueios melhor)
            return distancia + bloqueios * 10; // Penalizar mais veículos bloqueando o caminho
        }

        private bool BloqueiaVeiculo2(Veiculo veiculo, Veiculo veiculo2, Tuple<int, int> posicaoSaida)
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

        private bool IsMovimentoValido(Veiculo novoVeiculo, List<Veiculo> veiculos)
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