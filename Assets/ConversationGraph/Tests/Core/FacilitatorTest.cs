using ConversationGraph.Editor.Core;
using ConversationGraph.Editor.Foundation;
using ConversationGraph.Runtime.Core.Facilitators;
using ConversationGraph.Tests.Foundation;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace ConversationGraph.Tests.Core
{
    public class FacilitatorTest
    {
        [TestCase("abd4ae1595ea3464d95de69dedfb832e")]
        [TestCase("4d7ee7be3dfb4ae4bb405e6d81c64fc5")]
        public void FacilitateToOnNarrator(string guid)
        {
            var asset = ConversationGraphEditorUtility.GetAssetByGuid<ConversationGraphAsset>(guid);
            var isStart = false;
            var isEnd = false;
            var isNarrator = false;
            
            var testEvents = new TestEvent();

            testEvents.Narrator += () => isNarrator = true;
            testEvents.ConversationStart += () => isStart = true;
            testEvents.ConversationEnd += () => isEnd = true;
            
            new Facilitator(asset.SubAsset, new TestView(), testEvents).Facilitate().Forget();
            
            Assert.That(isNarrator, Is.True);
            Assert.That(isStart, Is.True);
            Assert.That(isEnd, Is.True);
        }
        [TestCase("742648f66ff4c68408329158cb5c1a72")]
        [TestCase("4d7ee7be3dfb4ae4bb405e6d81c64fc5")]
        public void FacilitateToOnSpeaker(string guid)
        {
            var asset = ConversationGraphEditorUtility.GetAssetByGuid<ConversationGraphAsset>(guid);
            var isStart = false;
            var isEnd = false;
            var isSpeaker = false;
            
            var testEvents = new TestEvent();
            testEvents.Narrator += () => isSpeaker = true;
            testEvents.ConversationStart += () => isStart = true;
            testEvents.ConversationEnd += () => isEnd = true;
            
            new Facilitator(asset.SubAsset, new TestView(), testEvents).Facilitate().Forget();
            
            Assert.That(isSpeaker, Is.True);
            Assert.That(isStart, Is.True);
            Assert.That(isEnd, Is.True);
        }
    }
}