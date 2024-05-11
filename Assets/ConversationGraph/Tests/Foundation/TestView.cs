using System;
using ConversationGraph.Runtime.Foundation.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

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

        public async UniTask ChangeMessage(string message, ITextAnimation textAnimation)
        {
            
        }

        public async UniTask PlayTimeline(PlayableAsset playableAsset)
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
