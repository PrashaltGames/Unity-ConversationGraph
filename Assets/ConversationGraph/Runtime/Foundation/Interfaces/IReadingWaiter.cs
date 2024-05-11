using Cysharp.Threading.Tasks;

namespace ConversationGraph.Runtime.Foundation.Interfaces
{
    public interface IReadingWaiter
    {
        public UniTask WaitReading();
    }
}
