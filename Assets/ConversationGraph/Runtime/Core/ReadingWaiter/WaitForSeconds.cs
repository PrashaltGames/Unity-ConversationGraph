using System;
using ConversationGraph.Runtime.Foundation.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ConversationGraph.Runtime.Core.ReadingWaiter
{
    [Serializable]
    public class WaitForSeconds : IReadingWaiter
    {
        [SerializeField] private float _seconds;
        public async UniTask WaitReading()
        {
            await UniTask.WaitForSeconds(_seconds);
        }
    }
}