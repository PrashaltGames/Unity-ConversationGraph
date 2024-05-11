using System;
using ConversationGraph.Runtime.Foundation.Interfaces;
using Cysharp.Threading.Tasks;

namespace ConversationGraph.Runtime.Core.ReadingWaiter
{
    [Serializable]
    public class WaitForClick : IReadingWaiter
    {
        public async UniTask WaitReading()
        {
            ConversationUtility.ShouldNext = false;
            ConversationUtility.WaitForInput = true;
            
            await UniTask.WaitUntil(() => ConversationUtility.ShouldNext);
            ConversationUtility.WaitForInput = false;
        }
    }
}