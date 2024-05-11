namespace ConversationGraph.Runtime.Foundation.Interfaces
{
    public interface IScriptableBranch
    {
        public string Guid { get; set; }
        // to source generator?
        public int BranchCount { get; protected set; }
        public int OnArrival();
    }
}