using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;

namespace ConversationGraph.Runtime.Foundation.Interfaces
{
    public interface ITextAnimation
    {
        public UniTask StartAnimation(TextMeshProUGUI speakerText, TextMeshProUGUI messageText,
            CancellationToken cancellationToken);
    }
}