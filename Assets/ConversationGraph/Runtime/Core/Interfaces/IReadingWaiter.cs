using Cysharp.Threading.Tasks;

namespace ConversationGraph.Runtime.Core.Interfaces
{
    public interface IReadingWaiter
    {
        public UniTask WaitReading();
    }
}
