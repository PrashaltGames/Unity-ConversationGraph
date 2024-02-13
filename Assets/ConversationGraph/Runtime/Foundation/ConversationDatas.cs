using System.Collections.Generic;

namespace ConversationGraph.Runtime.Foundation
{
    public struct ConversationData
    {
        public ConversationData(string speakerName, IEnumerable<string> textList)
        {
            SpeakerName = speakerName;
            TextList = textList;
        }
        public string SpeakerName { get; private set; }
        public IEnumerable<string> TextList { get; private set; }
    }
}
