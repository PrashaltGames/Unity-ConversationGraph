using System;
using ConversationGraph.Runtime.Core.Interfaces;
using Cysharp.Threading.Tasks;

namespace ConversationGraph.Tests.Foundation
{
    public class TestView : IConversationView
    {
        public void ChangeTitle(string title)
        {
            
        }

        public void ChangeSpeaker(string speaker)
        {
            
        }

        public async UniTask ChangeMessage(string message)
        {
            
        }

        public void AddSelect(string selectText, Action onSelected)
        {
            
        }

        public async UniTask WaitSelect()
        {
            
        }
    }
}
