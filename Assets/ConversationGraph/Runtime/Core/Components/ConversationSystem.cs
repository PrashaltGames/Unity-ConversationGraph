using System;
using ConversationGraph.Runtime.Core.Facilitators;
using ConversationGraph.Runtime.Core.Interfaces;
using ConversationGraph.Runtime.Foundation;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConversationGraph.Runtime.Core.Components
{
    public class ConversationSystem : MonoBehaviour, IConversationView, IConversationEvents
    {
        // for user event
        public Action OnConversationStartEvent { get; set; }
        public Action OnConversationEndEvent { get; set; }
        public Action OnNarratorEvent { get; set; }
        public Action OnSpeakerEvent { get; set; }
        public Action OnShowSelectButtonsEvent { get; set; }
        public Action OnSelectedEvent { get; set; }
        
        [Header("▼ Conversation GUI")] 
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private TextMeshProUGUI _speakerText;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Transform _selectParent;
        [SerializeField] private Button _selectButtonPrefab;

        [Header("▼ Conversation Asset")] 
        [SerializeField] private ConversationAsset _conversationAsset;

        [Header("▼ Conversation Settings")] 
        [SerializeReference, SubclassSelector] private IReadingWaiter _readingWaiter;

        [SerializeField, HideInInspector] private ConversationHistory _history;
        private Facilitator _facilitator;
        private TextMeshProUGUI _prefabText;
        private int _selectIndex;
        private bool _isSelected;

        private void Start()
        {
            _prefabText = _selectButtonPrefab.GetComponentInChildren<TextMeshProUGUI>();
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
            _facilitator = new(_conversationAsset, this, this, _history);
            _facilitator.Facilitate();
        }

        public void ChangeTitle(string title)
        {
            _titleText.SetText(title);
        }

        public void ChangeSpeaker(string speaker)
        {
            _speakerText.SetText(speaker);
        }

        public async UniTask ChangeMessage(string message)
        {
            _messageText.SetText(message);

            await _readingWaiter.WaitReading();
        }

        public void AddSelect(string selectText, Action onSelected)
        {
            _prefabText.SetText(selectText);
            var obj = Instantiate(_selectButtonPrefab, _selectParent);
            
            obj.onClick.AddListener(onSelected.Invoke);
            obj.onClick.AddListener(() =>
            {
                _isSelected = true;
            });
        }

        public async UniTask WaitSelect()
        {
            _selectParent.gameObject.SetActive(true);

            await UniTask.WaitUntil(() => _isSelected);
            _isSelected = false;
            
            _selectParent.gameObject.SetActive(false);
        }

        void IConversationEvents.OnConversationStart()
        {
            OnConversationStartEvent?.Invoke();
        }

        void IConversationEvents.OnConversationEnd()
        {
            OnConversationEndEvent?.Invoke();
        }

        void IConversationEvents.OnNarrator()
        {
            OnNarratorEvent?.Invoke();
        }

        void IConversationEvents.OnSpeaker()
        {
            OnSpeakerEvent?.Invoke();
        }

        void IConversationEvents.OnShowSelectButtons()
        {
            OnShowSelectButtonsEvent?.Invoke();
        }

        void IConversationEvents.OnSelected()
        {
            OnSelectedEvent?.Invoke();
        }

#if UNITY_EDITOR
        public void Reset()
        {
            _history = GetComponent<ConversationHistory>();
        }
#endif
    }
}
