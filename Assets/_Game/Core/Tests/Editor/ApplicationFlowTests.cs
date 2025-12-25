using NUnit.Framework;
using ProjectCore.UI;

namespace ProjectCore.Tests
{
    public class ApplicationFlowTests
    {
        private ApplicationFlowLogic _logic;

        [SetUp]
        public void SetUp()
        {
            _logic = new ApplicationFlowLogic();
        }

        // ===== MAIN MENU CONTEXT =====
        
        [Test]
        public void MainMenu_Game_Should_GoToGame()
        {
            var result = _logic.GetDecision(FlowContext.MainMenu, UICloseReasons.Game);
            Assert.AreEqual(FlowIntent.GoToGame, result);
        }
        

        [Test]
        public void MainMenu_UnmappedReason_Should_DefaultToMainMenu()
        {
            var result = _logic.GetDecision(FlowContext.MainMenu, UICloseReasons.Home);
            Assert.AreEqual(FlowIntent.DefaultToMainMenu, result);
        }

        // ===== LEVEL FAIL CONTEXT =====

        [Test]
        public void LevelFail_Home_Should_GoToMainMenu()
        {
            var result = _logic.GetDecision(FlowContext.LevelFail, UICloseReasons.Home);
            Assert.AreEqual(FlowIntent.GoToMainMenu, result);
        }

        [Test]
        public void LevelFail_Game_Should_GoToGame()
        {
            var result = _logic.GetDecision(FlowContext.LevelFail, UICloseReasons.Game);
            Assert.AreEqual(FlowIntent.GoToGame, result);
        }

        [Test]
        public void LevelFail_Revive_Should_ResumePrevious()
        {
            var result = _logic.GetDecision(FlowContext.LevelFail, UICloseReasons.Revive);
            Assert.AreEqual(FlowIntent.ResumePrevious, result);
        }
        

        // ===== EDGE CASES =====

        [Test]
        public void InvalidContext_Should_DefaultToMainMenu()
        {
            var result = _logic.GetDecision((FlowContext)999, UICloseReasons.Game);
            Assert.AreEqual(FlowIntent.DefaultToMainMenu, result);
        }

        [Test]
        public void InvalidReason_Should_DefaultToMainMenu()
        {
            var result = _logic.GetDecision(FlowContext.MainMenu, (UICloseReasons)999);
            Assert.AreEqual(FlowIntent.DefaultToMainMenu, result);
        }

        [Test]
        public void InvalidContextAndReason_Should_DefaultToMainMenu()
        {
            var result = _logic.GetDecision((FlowContext)999, (UICloseReasons)999);
            Assert.AreEqual(FlowIntent.DefaultToMainMenu, result);
        }
    }
}