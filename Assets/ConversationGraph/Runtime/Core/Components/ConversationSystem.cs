using System.Collections.Generic;
using ConversationGraph.Runtime.Core.Interfaces;
using ConversationGraph.Runtime.Foundation;
using TMPro;
using UnityEngine;

namespace ConversationGraph.Runtime.Core.Components
{
    public class ConversationSystem : MonoBehaviour
    {
        [Header("-- Conversation GUI --")] 
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _speaker;
        [SerializeField] private TextMeshProUGUI _title;

        [Header("▼ Conversation Asset")] 
        [SerializeField] private ConversationAsset _asset;
        
        
        private IFacilitator _facilitator;
        private List<ConversationData> _datas = new();

        public void Start()
        {
            StartConversation();
        }

        public void StartConversation()
        {
            foreach (var saveData in _asset.ConversationSaveData)
            {
                _datas.Add(ConversationUtility.JsonToConversationData(saveData));
            }
        }
    }
}
