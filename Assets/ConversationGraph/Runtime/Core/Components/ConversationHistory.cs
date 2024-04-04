using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ConversationGraph.Runtime.Core.Interfaces;
using ConversationGraph.Runtime.Foundation;
using UnityEngine;

namespace ConversationGraph.Runtime.Core.Components
{
    [RequireComponent(typeof(ConversationSystem))]
    public class ConversationHistory : MonoBehaviour
    {
        public List<MessageData> HistoryList { get; } = new();

        [SerializeField] private HistoryPrefab _historyPrefab;
        [SerializeField] private Transform _historyParent;

        public void ShowHistory()
        {
            foreach (var history in HistoryList)
            {
                _historyPrefab.SpeakerText.SetText(history.Speaker);
                foreach (var message in history.MessageList)
                {
                    var historyObj = Instantiate(_historyPrefab, _historyParent);
                    historyObj.MessageText.SetText(message);
                }
            }
        }
#if UNITY_EDITOR
        private void Reset()
        {
            var system = GetComponent<ConversationSystem>();
            system.Reset();
        }
#endif
        
    }
}