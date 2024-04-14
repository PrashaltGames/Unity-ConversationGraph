namespace ConversationGraph.Runtime.Core.Interfaces
{
    public interface IScriptableEvent
    {
        public string Guid { get; set; }
        public void OnArrival();
    }
}