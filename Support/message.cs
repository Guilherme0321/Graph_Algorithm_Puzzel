using graph_algorithm.Support;
using System;

public class MinHeap
{
    private List<Tuple<int, Estado>> heap;
    private int size;
    
    public MinHeap()
    {
        heap = new List<Tuple<int, Estado>>();
        size = 0;
    }

    public bool IsEmpty()
    {
        return size == 0;
    }

    public int Count()
    {
        return size;
    }

    private int Parent(int index)
    {
        return (index - 1) / 2;
    }

    private int LeftChild(int index)
    {
        return 2 * index + 1;
    }

    private int RightChild(int index)
    {
        return 2 * index + 2;
    }

    private void Swap(int i, int j)
    {
        Tuple<int, Estado> temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }

    public void Insert(Tuple<int, Estado> value)
    {
        heap.Add(value);
        size++;
        HeapifyUp(size - 1);
    }

    private void HeapifyUp(int index)
    {
        while (index > 0 && heap[Parent(index)].Item1 > heap[index].Item1)
        {
            Swap(index, Parent(index));
            index = Parent(index);
        }
    }

    public Tuple<int, Estado> Peek()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Heap está vazio");
        return heap[0];
    }

    public Tuple<int, Estado> ExtractMin()
    {
        if (IsEmpty())
            throw new InvalidOperationException("Heap está vazio");

        Tuple<int, Estado> min = heap[0];
        heap[0] = heap[size - 1];
        heap.RemoveAt(size - 1);
        size--;

        if (!IsEmpty())
        {
            HeapifyDown(0);
        }

        return min;
    }

    private void HeapifyDown(int index)
    {
        int minIndex = index;
        int left = LeftChild(index);
        int right = RightChild(index);

        if (left < size && heap[left].Item1 < heap[minIndex].Item1)
            minIndex = left;

        if (right < size && heap[right].Item1 < heap[minIndex].Item1)
            minIndex = right;

        if (minIndex != index)
        {
            Swap(index, minIndex);
            HeapifyDown(minIndex);
        }
    }

    public void Clear()
    {
        heap.Clear();
        size = 0;
    }

    public List<Tuple<int, Estado>> GetHeapArray()
    {
        return new List<Tuple<int, Estado>>(heap);
    }

    // Método para verificar se a estrutura mantém a propriedade de heap
    public bool IsValidHeap()
    {
        return IsValidHeapHelper(0);
    }

    private bool IsValidHeapHelper(int index)
    {
        if (index >= size) return true;

        int left = LeftChild(index);
        int right = RightChild(index);

        if (left < size && heap[left].Item1 < heap[index].Item1)
            return false;

        if (right < size && heap[right].Item1 < heap[index].Item1)
            return false;

        return IsValidHeapHelper(left) && IsValidHeapHelper(right);
    }
}