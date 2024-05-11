namespace ConversationGraph.Runtime.Foundation.Interfaces
{
    public interface IScriptableEvent
    {
        public string Guid { get; set; }
        public void OnArrival();
    }
}