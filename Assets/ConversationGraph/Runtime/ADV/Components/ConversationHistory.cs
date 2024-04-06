using System.Collections.Generic;
using ConversationGraph.Runtime.Core;
using ConversationGraph.Runtime.Core.Components;
using ConversationGraph.Runtime.Foundation;
using UnityEngine;

namespace ConversationGraph.Runtime.ADV.Components
{
    [RequireComponent(typeof(ConversationSystem))]
    public class ConversationHistory : MonoBehaviour
    {
        public List<MessageData> HistoryList { get; } = new();

        [SerializeField] private HistoryPrefab _historyPrefab;
        [SerializeField] private Transform _historyParent;

        [SerializeField] private CanvasGroup _historyCanvas;

        public void SetHistoryActive(bool isActive)
        {
            _historyCanvas.alpha = isActive ? 1 : 0;
            _historyCanvas.blocksRaycasts = isActive;
            
            if (isActive)
            {
                ShowHistory();
            }
            else
            {
                HideHistory();
            }
        }
        private void ShowHistory()
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

        private void HideHistory()
        {
            
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