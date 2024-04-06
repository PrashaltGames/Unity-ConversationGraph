using System.Threading;
using ConversationGraph.Runtime.Core.Interfaces;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ConversationGraph.Runtime.Core.Animation
{
    public class DefaultAnimation : ITextAnimation
    {
        private const int DelayMilliseconds = 100;
        public async UniTask StartAnimation(TextMeshProUGUI speakerText, TextMeshProUGUI messageText,
            CancellationToken cancellationToken)
        {
            messageText.maxVisibleCharacters = 0;
            //アニメーション
            for (var i = 1; i <= messageText.text.Length; i++)
            {
                messageText.maxVisibleCharacters = i;
                await UniTask.Delay(DelayMilliseconds, cancellationToken: cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    messageText.maxVisibleCharacters = messageText.text.Length;
                    break;
                }
            }
        }
    }
}