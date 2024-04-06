using ConversationGraph.Runtime.Core.Interfaces;

namespace ConversationGraph.Runtime.Foundation.Dummies
{
    public class DummyScriptableBranch : IScriptableBranch
    {
        public string Guid { get; set; }

        int IScriptableBranch.BranchCount { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>When return 0, next node is the top one.</returns>
        public int OnArrival()
        {
            return 0;
        }
    }
}