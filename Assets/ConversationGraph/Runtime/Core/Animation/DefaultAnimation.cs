using System.Threading;
using ConversationGraph.Runtime.Core.Interfaces;
using Cysharp.Threading.Tasks;
using TMPro;

namespace ConversationGraph.Runtime.Core.Animation
{
    public class DefaultAnimation : ITextAnimation
    {
        public async UniTask StartAnimation(TextMeshProUGUI speakerText, TextMeshProUGUI messageText,
            CancellationToken cancellationToken)
        {
            messageText.maxVisibleCharacters = 0;
            //アニメーション
            for (var i = 1; i <= messageText.text.Length; i++)
            {
                messageText.maxVisibleCharacters = i;
                await UniTask.Delay(500);

                if (cancellationToken.IsCancellationRequested)
                {
                    messageText.maxVisibleCharacters = messageText.text.Length;
                    break;
                }
            }
        }
    }
}