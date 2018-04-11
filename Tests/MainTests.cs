using NUnit.Framework;
using SeleniumUnitTest;

namespace CreateObjectAfterFieldParsing
{
    [TestFixture]
    public class MainTests
    {
        Helper helper = new Helper();

        [SetUp]
        public void Setup()
        {
            PageInitial home = new PageInitial();
            home.GoToPage();
            home.LoginToApplication();
        }

        [Test]
        [Category("UC1 - ObjectFieldCreated")]
        public void RunScriptTest()
        {
            // -- arrange: dedicated to collect input data
            helper.ProvisionEnvironment();
            helper.ClickToWorkSpace();
            helper.ClickTabAdmin();


            ////--- act: run process
            helper.RunScript();
            var act = helper.SelectTypeSearch();


            ////--- assert: validate results
            Assert.IsTrue(act);
        }

        [TearDown]
        public void TearDown()
        {
            helper.CleanEnvironment();
        }
    }
}
