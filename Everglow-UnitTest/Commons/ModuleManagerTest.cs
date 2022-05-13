using Everglow.Sources.Commons;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Everglow_UnitTest
{
    [TestClass]
    public class ModuleManagerTest
    {
        //private readonly ModuleManager m_moduleManager;

        public ModuleManagerTest()
        {
            ModuleManager.Instance.Load();
        }

        [TestMethod]
        public void UT_TestIf_AddModule_IsCorrect()
        {
            Mock<IModule> mockModule = new Mock<IModule>();
            mockModule.Setup(m => m.Name).Returns("Test");

            // Add a module named "Test"
            ModuleManager.AddModule(mockModule.Object);

            // Should be able to get the object
            var module = ModuleManager.GetModule("Test");

            Assert.AreEqual("Test", module.Name);
        }

    }
}