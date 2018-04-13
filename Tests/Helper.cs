using CreateObjectAfterFieldParsing.Infrastructure;
using kCura.Relativity.Client;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Relativity.Test.Helpers;
using Relativity.Test.Helpers.ImportAPIHelper;
using Relativity.Test.Helpers.SharedTestHelpers;
using Relativity.Test.Helpers.WorkspaceHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace CreateObjectAfterFieldParsing
{
    class Helper
    {
        private TestHelper relativityHelper;
        private IWebDriver driver => TestEnvironment.Instance.Driver;
        int testWorkspaceID;

        public Helper()
        {
            PageFactory.InitElements(driver, this);
            relativityHelper = new TestHelper();
        }

        [FindsBy(How = How.LinkText, Using = "COAFP Test")]
        public IWebElement Button { get; set; }

        [FindsBy(How = How.Id, Using = "qnTextBox")]
        public IWebElement Button1 { get; set; }

        [FindsBy(How = How.LinkText, Using = "Case Admin")]
        public IWebElement TabAdmin { get; set; }

        [FindsBy(How = How.LinkText, Using = "Scripts")]
        public IWebElement TabObject { get; set; }

        [FindsBy(How = How.Id, Using = "fil_itemListFUI")]
        public IWebElement TableScripts { get; set; }

        [FindsBy(How = How.LinkText, Using = "Run Script")]
        public IWebElement RunForm { get; set; }

        [FindsBy(How = How.Id, Using = "searchId_dropDownList")]
        public IWebElement ComboboxSaveSearch { get; set; }

        [FindsBy(How = How.Id, Using = "field1_dropDownList")]
        public IWebElement ComboboxField1 { get; set; }

        [FindsBy(How = How.Id, Using = "field2_dropDownList")]
        public IWebElement ComboboxField2 { get; set; }

        [FindsBy(How = How.Id, Using = "field3_dropDownList")]
        public IWebElement ComboboxField3 { get; set; }

        [FindsBy(How = How.Id, Using = "field4_dropDownList")]
        public IWebElement ComboboxField4 { get; set; }

        [FindsBy(How = How.Id, Using = "delimiter_textBox_textBox")]
        public IWebElement Delimiter { get; set; }

        [FindsBy(How = How.Id, Using = "fieldToPopulate_dropDownList")]
        public IWebElement ComboboxFieldtoPopulate { get; set; }

        [FindsBy(How = How.Id, Using = "_run_button")]
        public IWebElement BtnRun { get; set; }

        [FindsBy(How = How.Id, Using = "fil_itemListFUI")]
        public IWebElement TableElemet { get; set; }

        [FindsBy(How = How.LinkText, Using = "Workspace Admin")]
        public IWebElement Workspace { get; set; }

        [FindsBy(How = How.Id, Using = "_viewTemplate_delete1_button")]
        public IWebElement Delete { get; set; }

        [FindsBy(How = How.LinkText, Using = "Ok")]
        public IWebElement BtnOkWorkspace { get; set; }


        [FindsBy(How = How.Id, Using = "qnOuterContainer")]
        public IWebElement List { get; set; }

        [FindsBy(How = How.Id, Using = "viewMenu")]
        public IWebElement ComboboxTemplate { get; set; }

        //verifica que un elemento esté presente en el DOM de una página y sea visible.Esto no significa necesariamente que el elemento sea visible.
        public IWebElement WaitForPageUntilElementExists(By locator, int maxSeconds)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(maxSeconds))
                .Until(ExpectedConditions.ElementExists((locator)));
        }

        //verifica que un elemento esté presente en el DOM de una página y sea visible.Tiene que ser visible
        public IWebElement WaitForPageUntilElementIsVisible(By locator, int maxSeconds)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(maxSeconds))
                .Until(ExpectedConditions.ElementIsVisible((locator)));
        }

        //verifica que todos los elementos presentes en la página web que coinciden con el localizador sean visibles.Tiene que ser visible
        public IReadOnlyCollection<IWebElement> VisibilityOfAllElementsLocatedBy(By locator, int maxSeconds)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(maxSeconds))
                .Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy((locator)));
        }

        //verifica un elemento ews visible y está habilitada para que pueda hacer Click en él.
        public IWebElement WaitForPageUntilElementToBeClickable(By locator, int maxSeconds)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(maxSeconds))
                .Until(ExpectedConditions.ElementToBeClickable((locator)));
        }

        public void ProvisionEnvironment()
        {
            testWorkspaceID = createTestWorkspace();
            importTestDocuments(testWorkspaceID);
            uploadScript(testWorkspaceID);
        }

        public void CleanEnvironment()
        {
            DeletedWorkspace(testWorkspaceID);
        }

        private void uploadScript(int testWorkspaceID)
        {
            using (var client = TestEnvironment.Instance.ServicesManager.CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System))
            {
                Relativity.Test.Helpers.Application.ApplicationHelpers.ImportApplication(client, testWorkspaceID, true, @"\RA_CreateObjectAfterFieldParsing_20180413171452.rap");
            }
        }

        private int createTestWorkspace()
        {
            int workspaceID;
            using (var client = TestEnvironment.Instance.ServicesManager.CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System))
            {
                workspaceID = CreateWorkspace.Create(client, ConfigurationHelper.TEST_WORKSPACE_NAME, ConfigurationHelper.TEST_WORKSPACE_TEMPLATE_NAME);
            }
            driver.Navigate().Refresh();
            return workspaceID;
        }

        private void importTestDocuments(int testWorkspaceID)
        {
            ImportAPIHelper.ImportDocumentsInFolder(testWorkspaceID, ConfigurationHelper.TEST_DATA_LOCATION, true);
        }

        public void ClickToWorkSpace()
        {
            Thread.Sleep(2000);
            WebDriverWait obj = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            obj.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("_externalPage")));
            var selectElementSaveSearch = new SelectElement(ComboboxTemplate);
            selectElementSaveSearch.SelectByValue("_s1");
            Thread.Sleep(0500);
            driver.FindElement(By.LinkText(ConfigurationHelper.TEST_WORKSPACE_NAME)).Click();
        }

        public void ClickTabAdmin()
        {
            VisibilityOfAllElementsLocatedBy(By.Id("_externalPage"), 10);
            TabAdmin.Click();
            WaitForPageUntilElementIsVisible(By.LinkText("Scripts"), 10);
            TabObject.Click();
        }

        public void RunScript()
        {
            WebDriverWait OBJ = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            OBJ.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("_externalPage")));
            VisibilityOfAllElementsLocatedBy(By.Id("fil_itemListFUI"), 10);
            IList<IWebElement> all = TableScripts.FindElements(By.TagName("tr"));
            IList<IWebElement> all1 = all[3].FindElements(By.TagName("td"));
            IList<IWebElement> all2 = all1[5].FindElements(By.TagName("a"));
            all2[0].Click();
            //return true;
        }


        public bool SelectTypeSearch()
        {
            Thread.Sleep(0800);
            RunForm.Click();
            //cambia a la ultima ventana emergente abierta
            driver.SwitchTo().Window(driver.WindowHandles.ToList().Last());
            //Cambia a la primera ventana emergente
            //driver.SwitchTo().Window(driver.WindowHandles.ToList().First());
            //waitForPageUntilElementIsVisible(By.Id("searchId_dropDownList"), 10);
            var selectElementSaveSearch = new SelectElement(ComboboxSaveSearch);
            selectElementSaveSearch.SelectByValue("1038052");
            Thread.Sleep(0800);
            RandomField1();
            RandomField2();
            RandomField3();
            RandomField4();
            Delimiter.SendKeys("-");
            var selectElementFieldtoPopulate = new SelectElement(ComboboxFieldtoPopulate);
            selectElementFieldtoPopulate.SelectByValue("1042824");
            BtnRun.Click();
            driver.SwitchTo().Alert().Accept();
            driver.Close();
            driver.SwitchTo().Window(driver.WindowHandles.ToList().First());
            return true;
        }

        public void RandomField1()
        {
            IList<IWebElement> all = ComboboxField1.FindElements(By.TagName("option"));
            Random random = new Random();
            int posicion = random.Next(0, all.Count);
            var selectElementField1 = new SelectElement(ComboboxField1);
            all[posicion].Click();
        }

        public void RandomField2()
        {
            IList<IWebElement> all = ComboboxField2.FindElements(By.TagName("option"));
            Random random = new Random();
            int posicion = random.Next(0, all.Count);
            var selectElementField1 = new SelectElement(ComboboxField2);
            all[posicion].Click();
        }

        public void RandomField3()
        {
            IList<IWebElement> all = ComboboxField3.FindElements(By.TagName("option"));
            Random random = new Random();
            int posicion = random.Next(0, all.Count);
            var selectElementField1 = new SelectElement(ComboboxField3);
            all[posicion].Click();
        }

        public void RandomField4()
        {
            IList<IWebElement> all = ComboboxField4.FindElements(By.TagName("option"));
            Random random = new Random();
            int posicion = random.Next(0, all.Count);
            var selectElementField1 = new SelectElement(ComboboxField4);
            all[posicion].Click();
        }

        public void DeletedWorkspace(int testWorkspaceID)
        {
            using (var client = TestEnvironment.Instance.ServicesManager.CreateProxy<IRSAPIClient>(Relativity.API.ExecutionIdentity.System))
            {
                DeleteWorkspace.Delete(client, testWorkspaceID);
            }
        }

    }
}
