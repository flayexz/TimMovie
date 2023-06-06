using System.Collections.Concurrent;
using TimMovie.SharedKernel.Validators;

namespace TimMovie.SharedKernel.Extensions;

public static class QueueExtensions
{
    public static bool Remove<T>(this Queue<T> queue, T item)
    {
        ArgumentValidator.ThrowExceptionIfNull(queue, nameof(queue));

        var isDeleted = false;
        
        for (var i = 0; i < queue.Count; i++)
        {
            var itemFromQueue = queue.Dequeue();
            if (!isDeleted && Equals(item, itemFromQueue))
            {
                isDeleted = true;
            }
            else
            {
                queue.Enqueue(itemFromQueue);
            }
        }

        return isDeleted;
    }
    
    public static bool Remove<T>(this ConcurrentQueue<T> queue, T item)
    {
        ArgumentValidator.ThrowExceptionIfNull(queue, nameof(queue));

        var isDeleted = false;
        
        for (var i = 0; i < queue.Count; i++)
        {
            queue.TryDequeue(out var itemFromQueue);
            if (!isDeleted && Equals(item, itemFromQueue))
            {
                isDeleted = true;
            }
            else
            {
                queue.Enqueue(itemFromQueue);
            }
        }

        return isDeleted;
    }
}