using System.Collections.Concurrent;

namespace Promise.ProductManagementSystem.Helpers
{
    public class EmailQueue
    {
        private readonly ConcurrentQueue<(string To, string Subject, string Body)> _emailQueue = new();

        public void EnqueueEmail(string to, string subject, string body)
        {
            _emailQueue.Enqueue((to, subject, body));
        }

        public bool TryDequeueEmail(out (string To, string Subject, string Body) email)
        {
            return _emailQueue.TryDequeue(out email);
        }
    }
}
