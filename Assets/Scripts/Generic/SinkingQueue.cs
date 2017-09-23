using Entitas;
using System.Collections.Generic;

public class SinkingQueue<T>
{
    private const int SIZE = 10;
    private Queue<T> queue;
    private T latest;

    public SinkingQueue()
    {
        queue = new Queue<T>();
    }

    public T First()
    {
        return latest;
    }

    public T Last()
    {
        return queue.Peek();
    }

    public void Add(T element)
    {
        queue.Enqueue(element);
        latest = element; 

        if(queue.Count>SIZE)
        {
            queue.Dequeue();
        }
    }
}

