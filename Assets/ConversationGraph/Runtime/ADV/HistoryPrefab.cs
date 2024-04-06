using TMPro;
using UnityEngine;

namespace ConversationGraph.Runtime.ADV
{
    public class HistoryPrefab : MonoBehaviour
    {
        public TextMeshProUGUI SpeakerText => _speakerText;
        public TextMeshProUGUI MessageText => _messageText;
        
        [SerializeField] private TextMeshProUGUI _speakerText;
        [SerializeField] private TextMeshProUGUI _messageText;
    }
}