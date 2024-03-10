using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;

namespace ConversationGraph.Runtime.Core.Interfaces
{
    public interface ITextAnimation
    {
        public UniTask StartAnimation(TextMeshProUGUI speakerText, TextMeshProUGUI messageText,
            CancellationToken cancellationToken);
    }
}