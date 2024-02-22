using System.Collections.Generic;
using ConversationGraph.Runtime.Core.Facilitators;
using ConversationGraph.Runtime.Core.Interfaces;
using ConversationGraph.Runtime.Foundation;
using TMPro;
using UnityEngine;

namespace ConversationGraph.Runtime.Core.Components
{
    public class ConversationSystem : MonoBehaviour
    {
        [Header("▼ Conversation GUI")] 
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private TextMeshProUGUI _speakerText;
        [SerializeField] private TextMeshProUGUI _titleText;

        [Header("▼ Conversation Asset")] 
        [SerializeField] private ConversationAsset _conversationAsset;
        
        
        private BaseFacilitator _baseFacilitator;

        public void Start()
        {
            _baseFacilitator = new BasicFacilitator();
            StartConversation();
        }

        public void StartConversation()
        {
            var datas = 
                GetConversationDicFromSaveDataDic(_conversationAsset.ConversationSaveData);
            
            _baseFacilitator.StartConversation(_conversationAsset.StartId, _titleText,_speakerText, _messageText, datas);
        }

        private Dictionary<string, ConversationData> GetConversationDicFromSaveDataDic(Dictionary<string, ConversationSaveData> saveDataDic)
        {
            var resultDic = new Dictionary<string, ConversationData>();
            foreach (var saveData in 
                     _conversationAsset.ConversationSaveData)
            {
                resultDic.Add(saveData.Key, ConversationUtility.JsonToConversationData(saveData.Value));
            }

            return resultDic;
        }
    }
}
