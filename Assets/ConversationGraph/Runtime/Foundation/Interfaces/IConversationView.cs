using System;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

namespace ConversationGraph.Runtime.Foundation.Interfaces
{
    public interface IConversationView
    {
        public void ChangeTitle(string title);
        public void ChangeSpeaker(string speaker);
        public UniTask ChangeMessage(string message, ITextAnimation textAnimation);
        public UniTask PlayTimeline(PlayableAsset playableAsset);
        public void AddSelect(string selectText, Action onSelected);

        public UniTask WaitSelect();
    }
}