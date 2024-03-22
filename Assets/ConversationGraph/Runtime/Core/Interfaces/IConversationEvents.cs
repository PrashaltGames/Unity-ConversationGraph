namespace ConversationGraph.Runtime.Core.Interfaces
{
    public interface IConversationEvents
    {
        public void OnConversationStart();

        public void OnConversationEnd();

        public void OnNarrator();

        public void OnSpeaker();

        public void OnShowSelectButtons();

        public void OnSelected();
    }
}