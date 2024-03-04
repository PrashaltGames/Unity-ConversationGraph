using System.Collections.Generic;
using ConversationGraph.Runtime.Core.Base;
using ConversationGraph.Runtime.Core.Facilitators;
using ConversationGraph.Runtime.Foundation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConversationGraph.Runtime.Core.Components
{
    public class ConversationSystem : MonoBehaviour
    {
        [Header("▼ Conversation GUI")] 
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private TextMeshProUGUI _speakerText;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Transform _selectParent;
        [SerializeField] private Button _selectButton;

        [Header("▼ Conversation Asset")] 
        [SerializeField] private ConversationAsset _conversationAsset;
        [SerializeField] private ConversationPropertyAsset _propertyAsset;
        
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
            
            _baseFacilitator.StartConversation(
                _conversationAsset.StartId, _titleText,_speakerText, _messageText, 
                datas, _propertyAsset, _selectParent, _selectButton);
        }

        public void StartConversation(ConversationAsset asset, ConversationPropertyAsset propertyAsset)
        {
            var datas = 
                GetConversationDicFromSaveDataDic(asset.ConversationSaveData);
            
            _baseFacilitator.StartConversation(
                _conversationAsset.StartId, _titleText,_speakerText, _messageText, 
                datas, propertyAsset, _selectParent, _selectButton);
        }

        public void StartConversation(Dictionary<string, string> propertiesDictionary)
        {
            var datas = 
                GetConversationDicFromSaveDataDic(_conversationAsset.ConversationSaveData);
            
            _baseFacilitator.StartConversation(
                _conversationAsset.StartId, _titleText,_speakerText, _messageText, 
                datas, propertiesDictionary, _selectParent, _selectButton);
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
