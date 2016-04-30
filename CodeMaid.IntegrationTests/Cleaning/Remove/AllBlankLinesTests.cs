using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Cleaning;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Cleaning.Remove
{
    [TestClass]
    [DeploymentItem(@"Cleaning\Remove\Data\AllBlankLines.cs", "Data")]
    [DeploymentItem(@"Cleaning\Remove\Data\AllBlankLines_Cleaned.cs", "Data")]
    public class AllBlankLinesTests
    {
        #region Setup

        private static RemoveWhitespaceLogic _removeWhitespaceLogic;
        private ProjectItem _projectItem;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            _removeWhitespaceLogic = RemoveWhitespaceLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_removeWhitespaceLogic);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(@"Data\AllBlankLines.cs");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            TestEnvironment.RemoveFromProject(_projectItem);
        }

        #endregion Setup

        #region Tests

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveAllBlankLines_CleansAsExpected()
        {
            Settings.Default.Cleaning_RemoveAllBlankLines = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunRemoveAllBlankLines, _projectItem, @"Data\AllBlankLines_Cleaned.cs");
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveAllBlankLines_DoesNothingOnSecondPass()
        {
            Settings.Default.Cleaning_RemoveAllBlankLines = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunRemoveAllBlankLines, _projectItem);
        }

        [TestMethod]
        [HostType("VS IDE")]
        public void CleaningRemoveAllBlankLines_DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Cleaning_RemoveAllBlankLines = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunRemoveAllBlankLines, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private void RunRemoveAllBlankLines(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _removeWhitespaceLogic.RemoveAllBlankLines(textDocument);
        }

        #endregion Helpers
    }
}