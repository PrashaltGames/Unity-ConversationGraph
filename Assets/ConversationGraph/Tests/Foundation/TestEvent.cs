
using System;
using ConversationGraph.Runtime.Core.Interfaces;

namespace ConversationGraph.Tests.Foundation
{
    public class TestEvent : IConversationEvents
    {
        public Action ConversationStart { get; set; }
        public Action ConversationEnd { get; set; }
        public Action Narrator { get; set; }
        public Action Speaker { get; set; }
        public Action Select { get; set; }
        public void OnConversationStart()
        {
            ConversationStart?.Invoke();
        }

        public void OnConversationEnd()
        {
            ConversationEnd?.Invoke();
        }

        public void OnNarrator()
        {
            Narrator?.Invoke();
        }

        public void OnSpeaker()
        {
            Speaker?.Invoke();
        }

        public void OnShowSelectButtons()
        {
            
        }

        public void OnSelected()
        {
            Select?.Invoke();
        }
    }
}
