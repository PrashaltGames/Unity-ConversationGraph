namespace ConversationGraph.Runtime.Core.Interfaces
{
    public interface IScriptableBranch
    {
        public string Guid { get; set; }
        public int BranchCount { get; set; }
        public int OnArrival();
    }
}