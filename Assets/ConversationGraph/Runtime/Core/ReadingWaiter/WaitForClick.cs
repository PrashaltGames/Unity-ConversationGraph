using System;
using Cysharp.Threading.Tasks;

namespace ConversationGraph.Runtime.Core.ReadingWaiter
{
    [Serializable]
    public class WaitForClick : IReadingWaiter
    {
        public async UniTask WaitReading()
        {
            await UniTask.Delay(10);
        }
    }
}