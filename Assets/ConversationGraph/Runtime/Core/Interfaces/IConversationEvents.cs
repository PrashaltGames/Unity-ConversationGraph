namespace ConversationGraph.Runtime.Core.Interfaces
{
    public interface IConversationEvents
    {
        internal void OnConversationStart();

        internal void OnConversationEnd();

        internal void OnNarrator();

        internal void OnSpeaker();

        internal void OnShowSelectButtons();

        internal void OnSelected();
    }
}