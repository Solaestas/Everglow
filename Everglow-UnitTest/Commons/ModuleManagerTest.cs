using Everglow.Sources.Commons;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Everglow.Sources.Commons.ModuleSystem;

namespace Everglow_UnitTest
{
    [TestClass]
    public class ModuleManagerTest
    {
        private readonly ModuleManager m_moduleManager;

        public ModuleManagerTest()
        {
            m_moduleManager = new ModuleManager();
        }

        [TestMethod]
        public void UT_TestIf_AddModule_IsCorrect()
        {
            Mock<IModule> mockModule = new Mock<IModule>();
            mockModule.Setup(m => m.Name).Returns("Test");

            // Add a module named "Test"
            m_moduleManager.AddModule(mockModule.Object);

            // Should be able to get the object
            var module = m_moduleManager.GetModule("Test");

            Assert.AreEqual("Test", module.Name);
        }

    }
}