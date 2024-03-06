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
        public TextMeshProUGUI MessageText => _messageText;
        public TextMeshProUGUI SpeakerText => _speakerText;
        public TextMeshProUGUI TitleText => _titleText;
        public Transform SelectParent => _selectParent;
        public Button SelectButton => _selectButton;
        public ConversationAsset ConversationAsset => _conversationAsset;
        public ConversationPropertyAsset ConversationPropertyAsset => _propertyAsset;
        public IReadingWaiter ReadingWaiter => _readingWaiter;
        
        [Header("▼ Conversation GUI")] 
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private TextMeshProUGUI _speakerText;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Transform _selectParent;
        [SerializeField] private Button _selectButton;

        [Header("▼ Conversation Asset")] 
        [SerializeField] private ConversationAsset _conversationAsset;
        [SerializeField] private ConversationPropertyAsset _propertyAsset;

        [Header("▼ Conversation Settings")] 
        [SerializeReference, SubclassSelector] private IReadingWaiter _readingWaiter;
        
        private BaseFacilitator _baseFacilitator;

        public void Start()
        {
            _baseFacilitator = new BasicFacilitator();
            StartConversation();
        }
        #if ENABLE_LEGACY_INPUT_MANAGER
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && ConversationUtility.WaitForInput)
            {
                ConversationUtility.ShouldNext = true;
            }
        }
        #elif ENABLE_INPUT_SYSTEM
        
        #endif

        public void StartConversation()
        {
            _baseFacilitator.StartConversation(this, _conversationAsset);
        }

        public void StartConversation(ConversationAsset asset, ConversationPropertyAsset propertyAsset)
        {
            var datas = 
                GetConversationDicFromSaveDataDic(asset.ConversationSaveData);
            
            _baseFacilitator.StartConversation(this, _conversationAsset);
        }

        public void StartConversation(Dictionary<string, string> propertiesDictionary)
        {
            var datas = 
                GetConversationDicFromSaveDataDic(_conversationAsset.ConversationSaveData);
            
            _baseFacilitator.StartConversation(this, _conversationAsset);
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
