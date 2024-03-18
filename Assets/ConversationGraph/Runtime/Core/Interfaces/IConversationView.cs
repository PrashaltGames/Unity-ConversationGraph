using System;
using Cysharp.Threading.Tasks;

namespace ConversationGraph.Runtime.Core.Interfaces
{
    public interface IConversationView
    {
        public void ChangeTitle(string title);
        public void ChangeSpeaker(string speaker);
        public UniTask ChangeMessage(string message);

        public void AddSelect(string selectText, Action onSelected);

        public UniTask WaitSelect();
    }
}