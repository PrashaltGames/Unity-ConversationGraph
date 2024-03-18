using ConversationGraph.Runtime.Core.Components;

namespace ConversationGraph.Runtime.Core.Interfaces
{
    public interface IScriptableConversation
    {
        public string Guid { get; set; }
        public void OnArrival();
    }
}