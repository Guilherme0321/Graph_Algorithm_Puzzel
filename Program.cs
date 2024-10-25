using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static RushHourSolver;

public class RushHourSolver
{
    public enum VehicleType
    {
        RedCar,    // Carro principal vermelho
        CarA,
        CarB,
        CarC,
        CarD,
        CarE,
        CarF,
        CarG,
        CarH,
        CarI,
        CarJ,
        CarK,
        CarL,
        CarM,
        CarN,
        CarO,
        CarP,
        CarQ,
        CarR,
        CarS,
        CarT,
        CarU,
        CarV,
        CarW,
        CarX,
        CarY,
        CarZ,
    }

    public class Vehicle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsHorizontal { get; set; }
        public int Length { get; set; }
        public bool IsMain { get; set; }
        public VehicleType Type { get; set; }

        public Vehicle(int x, int y, bool isHorizontal, int length, VehicleType type, bool isMain = false)
        {
            X = x;
            Y = y;
            IsHorizontal = isHorizontal;
            Length = length;
            Type = type;
            IsMain = isMain;
        }

        public Vehicle Clone()
        {
            return new Vehicle(X, Y, IsHorizontal, Length, Type, IsMain);
        }

        public char GetDisplaySymbol()
        {
            return Type == VehicleType.RedCar ? 'R' : Type.ToString().Last();
        }
    }

    public class GameState : IEquatable<GameState>
    {
        public List<Vehicle> Vehicles { get; }
        public int MovesCount { get; }
        public GameState Parent { get; }
        public string Move { get; set; }

        private int? _hashCode;

        public GameState(List<Vehicle> vehicles, int movesCount, GameState parent = null, string move = null)
        {
            Vehicles = vehicles ?? throw new ArgumentNullException(nameof(vehicles));
            MovesCount = movesCount;
            Parent = parent;
            Move = move;
        }

        public string GetBoardState()
        {
            var boardBuilder = new StringBuilder();
            char[,] board = new char[BOARD_ROWS, BOARD_COLS];

            for (int i = 0; i < BOARD_ROWS; i++)
                for (int j = 0; j < BOARD_COLS; j++)
                    board[i, j] = '.';

            foreach (var vehicle in Vehicles)
            {
                char symbol = vehicle.GetDisplaySymbol();

                for (int i = 0; i < vehicle.Length; i++)
                {
                    if (vehicle.IsHorizontal)
                        board[vehicle.Y, vehicle.X + i] = symbol;
                    else
                        board[vehicle.Y + i, vehicle.X] = symbol;
                }
            }

            boardBuilder.AppendLine("  0 1 2 3 4 5 6 7 8 9");
            boardBuilder.AppendLine("  -------------------");

            for (int i = 0; i < BOARD_ROWS; i++)
            {
                boardBuilder.Append($"{i}|");
                for (int j = 0; j < BOARD_COLS; j++)
                {
                    boardBuilder.Append($"{board[i, j]} ");
                }
                if (i == EXIT_ROW)
                    boardBuilder.Append("EXIT");
                boardBuilder.AppendLine();
            }

            return boardBuilder.ToString();
        }

        public bool Equals(GameState other)
        {
            if (other == null) return false;
            if (Vehicles.Count != other.Vehicles.Count) return false;

            for (int i = 0; i < Vehicles.Count; i++)
            {
                if (Vehicles[i].X != other.Vehicles[i].X ||
                    Vehicles[i].Y != other.Vehicles[i].Y)
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            if (_hashCode.HasValue) return _hashCode.Value;

            unchecked
            {
                int hash = 17;
                foreach (var vehicle in Vehicles)
                {
                    hash = hash * 31 + vehicle.X;
                    hash = hash * 31 + vehicle.Y;
                }
                _hashCode = hash;
                return hash;
            }
        }

        public GameState Clone()
        {
            return new GameState(
                Vehicles.Select(v => v.Clone()).ToList(),
                MovesCount,
                this,
                Move
            );
        }
    }

    private class PriorityQueue<T>
    {
        private SortedDictionary<int, Queue<T>> _dict = new SortedDictionary<int, Queue<T>>();

        public void Enqueue(T item, int priority)
        {
            if (!_dict.ContainsKey(priority))
                _dict[priority] = new Queue<T>();
            _dict[priority].Enqueue(item);
        }

        public T Dequeue()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Queue is empty");

            var first = _dict.First();
            var item = first.Value.Dequeue();
            if (first.Value.Count == 0)
                _dict.Remove(first.Key);
            return item;
        }

        public bool IsEmpty()
        {
            return !_dict.Any();
        }
    }

    public class SolutionPath
    {
        public List<string> Moves { get; set; }
        public List<string> BoardStates { get; set; }
        public int TotalMoves { get; set; }

        public SolutionPath()
        {
            Moves = new List<string>();
            BoardStates = new List<string>();
            TotalMoves = 0;
        }
    }

    private const int BOARD_ROWS = 6;
    private const int BOARD_COLS = 10;
    private const int EXIT_ROW = 2;

    public static GameState CreateInitialState()
    {
        var vehicles = new List<Vehicle>
        {
            // Red car (main vehicle)
            new Vehicle(0, 2, true, 1, VehicleType.CarR, true),
            new Vehicle(1, 3, true, 1, VehicleType.CarA),
            new Vehicle(1, 2, false, 1, VehicleType.CarB),
            new Vehicle(0, 3, true, 1, VehicleType.CarC),
            new Vehicle(3, 3, false, 2, VehicleType.CarD),
            new Vehicle(4, 1, true, 1, VehicleType.CarE),
            new Vehicle(5, 5, true, 1, VehicleType.CarF),
            new Vehicle(7, 2, false, 1, VehicleType.CarG),
            new Vehicle(6, 4, false, 1, VehicleType.CarH),
            new Vehicle(5, 4, true, 1, VehicleType.CarI),
            new Vehicle(0, 4, false, 2, VehicleType.CarJ),
            new Vehicle(9, 3, false, 2, VehicleType.CarK),
            new Vehicle(6, 2, false, 2, VehicleType.CarL),
            new Vehicle(5, 1, false, 2, VehicleType.CarM),
            new Vehicle(7, 1, false, 1, VehicleType.CarN),
            new Vehicle(9, 2, false, 1, VehicleType.CarO),
            new Vehicle(9, 1, true, 1, VehicleType.CarP),
            new Vehicle(1, 0, true, 3, VehicleType.CarQ),
            new Vehicle(0, 1, true, 3, VehicleType.CarS),
            new Vehicle(6, 5, true, 3, VehicleType.CarT)
        };
        /*
            new Vehicle(0, 2, true, 2, VehicleType.RedCar, true),
            new Vehicle(1, 3, true, 2, VehicleType.CarA),
            new Vehicle(1, 2, false, 2, VehicleType.CarB),
            new Vehicle(0, 3, true, 2, VehicleType.CarC),
            new Vehicle(3, 3, false, 2, VehicleType.CarD),
            new Vehicle(4, 1, true, 2, VehicleType.CarE),
            new Vehicle(5, 5, true, 2, VehicleType.CarF),
            new Vehicle(7, 2, false, 2, VehicleType.CarG),
            new Vehicle(6, 4, false, 2, VehicleType.CarH),
            new Vehicle(5, 4, true, 2, VehicleType.CarI),
            new Vehicle(0, 4, false, 2, VehicleType.CarJ),
            new Vehicle(9, 3, false, 2, VehicleType.CarK),
            new Vehicle(6, 2, false, 2, VehicleType.CarL),
            new Vehicle(5, 1, false, 2, VehicleType.CarM)
         */

        return new GameState(vehicles, 0);
    }

    public SolutionPath SolveGame(GameState initialState)
    {
        if (initialState == null)
            throw new ArgumentNullException(nameof(initialState));

        if (!initialState.Vehicles.Any(v => v.IsMain))
            throw new InvalidOperationException("Initial state must contain a main vehicle");

        foreach (var vehicle in initialState.Vehicles)
        {
            if (!IsVehicleWithinBounds(vehicle))
                throw new InvalidOperationException($"Vehicle at position ({vehicle.X}, {vehicle.Y}) is outside board boundaries");
        }

        var openSet = new PriorityQueue<GameState>();
        var closedSet = new HashSet<GameState>();
        var costs = new Dictionary<GameState, int>();

        openSet.Enqueue(initialState, Heuristic(initialState));
        costs[initialState] = 0;

        while (!openSet.IsEmpty())
        {
            var current = openSet.Dequeue();

            if (IsGoalState(current))
            {
                return CreateSolutionPath(current);
            }

            closedSet.Add(current);

            foreach (var nextState in GetPossibleMoves(current))
            {
                if (closedSet.Contains(nextState))
                    continue;

                int tentativeCost = costs[current] + 1;

                if (!costs.ContainsKey(nextState) || tentativeCost < costs[nextState])
                {
                    costs[nextState] = tentativeCost;
                    int priority = tentativeCost + Heuristic(nextState);
                    openSet.Enqueue(nextState, priority);
                }
            }
        }

        return null;
    }

    private bool IsVehicleWithinBounds(Vehicle vehicle)
    {
        if (vehicle.IsHorizontal)
        {
            return vehicle.X >= 0 &&
                   vehicle.X + vehicle.Length <= BOARD_COLS &&
                   vehicle.Y >= 0 &&
                   vehicle.Y < BOARD_ROWS;
        }
        else
        {
            return vehicle.X >= 0 &&
                   vehicle.X < BOARD_COLS &&
                   vehicle.Y >= 0 &&
                   vehicle.Y + vehicle.Length <= BOARD_ROWS;
        }
    }

    private int Heuristic(GameState state)
    {
        var mainVehicle = state.Vehicles.FirstOrDefault(v => v.IsMain);
        if (mainVehicle == null)
        {
            throw new InvalidOperationException("No main vehicle found in the current game state.");
        }

        int distanceToExit = BOARD_COLS - (mainVehicle.X + mainVehicle.Length);
        int blockingVehicles = CountBlockingVehicles(state, mainVehicle);

        return distanceToExit + (blockingVehicles * 2);
    }

    private int CountBlockingVehicles(GameState state, Vehicle mainCar)
    {
        int count = 0;
        int pathStart = mainCar.X + mainCar.Length;

        foreach (var vehicle in state.Vehicles)
        {
            if (!vehicle.IsMain && !vehicle.IsHorizontal &&
                vehicle.X > pathStart && vehicle.Y <= EXIT_ROW &&
                vehicle.Y + vehicle.Length > EXIT_ROW)
            {
                count++;
            }
        }

        return count;
    }

    private bool IsGoalState(GameState state)
    {
        var mainCar = state.Vehicles.FirstOrDefault(v => v.IsMain);
        return mainCar != null && mainCar.X + mainCar.Length >= BOARD_COLS;
    }

    private List<GameState> GetPossibleMoves(GameState state)
    {
        var possibleMoves = new List<GameState>();
        var board = CreateBoard(state);

        for (int i = 0; i < state.Vehicles.Count; i++)
        {
            var vehicle = state.Vehicles[i];
            char vehicleSymbol = vehicle.GetDisplaySymbol();

            if (CanMove(vehicle, board, 1))
            {
                var newState = state.Clone();
                newState.Vehicles[i].X += vehicle.IsHorizontal ? 1 : 0;
                newState.Vehicles[i].Y += vehicle.IsHorizontal ? 0 : 1;
                string direction = vehicle.IsHorizontal ? "right" : "down";
                newState.Move = $"Move {vehicleSymbol} {direction}";
                possibleMoves.Add(newState);
            }

            if (CanMove(vehicle, board, -1))
            {
                var newState = state.Clone();
                newState.Vehicles[i].X += vehicle.IsHorizontal ? -1 : 0;
                newState.Vehicles[i].Y += vehicle.IsHorizontal ? 0 : -1;
                string direction = vehicle.IsHorizontal ? "left" : "up";
                newState.Move = $"Move {vehicleSymbol} {direction}";
                possibleMoves.Add(newState);
            }
        }

        return possibleMoves;
    }

    private bool CanMove(Vehicle vehicle, bool[,] board, int direction)
    {
        if (vehicle.IsHorizontal)
        {
            int newX = direction > 0 ? vehicle.X + vehicle.Length : vehicle.X - 1;

            if (newX < 0 || newX >= BOARD_COLS)
                return false;

            return !board[vehicle.Y, newX];
        }
        else
        {
            int newY = direction > 0 ? vehicle.Y + vehicle.Length : vehicle.Y - 1;

            if (newY < 0 || newY >= BOARD_ROWS)
                return false;

            return !board[newY, vehicle.X];
        }
    }

    private bool[,] CreateBoard(GameState state)
    {
        var board = new bool[BOARD_ROWS, BOARD_COLS];

        foreach (var vehicle in state.Vehicles)
        {
            for (int i = 0; i < vehicle.Length; i++)
            {
                if (vehicle.IsHorizontal)
                    board[vehicle.Y, vehicle.X + i] = true;
                else
                    board[vehicle.Y + i, vehicle.X] = true;
            }
        }

        return board;
    }

    private SolutionPath CreateSolutionPath(GameState finalState)
    {
        var solution = new SolutionPath();
        var current = finalState;

        while (current != null)
        {
            if (current.Move != null)
                solution.Moves.Add(current.Move);

            solution.BoardStates.Add(current.GetBoardState());
            current = current.Parent;
        }

        solution.Moves.Reverse();
        solution.BoardStates.Reverse();
        solution.TotalMoves = solution.Moves.Count;

        return solution;
    }
}

public class Program
{
    public static void Main()
    {
        try
        {
            var solver = new RushHourSolver();
            var initialState = RushHourSolver.CreateInitialState();

            //Console.WriteLine("Initial state:");
            Console.WriteLine(initialState.GetBoardState());

            //Console.WriteLine("Solving puzzle...");
            SolutionPath solution = solver.SolveGame(initialState);

            foreach (string item in solution.Moves)
            {
                Console.WriteLine(item);
            }

            //if (solution != null)
            //{
            //    Console.WriteLine($"Solution found in {solution.TotalMoves} moves:");
            //    for (int i = 0; i < solution.BoardStates.Count; i++)
            //    {
            //        Console.WriteLine($"\nStep {i}:");
            //        if (i > 0)
            //            Console.WriteLine(solution.Moves[i - 1]);
            //        Console.WriteLine(solution.BoardStates[i]);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No solution found!");
            //}
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}