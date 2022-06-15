using Everglow.Sources.Commons.Core.ModuleSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Everglow_UnitTest
{
    [TestClass]
    public class ModuleManagerTest
    {
        private readonly ModuleManager m_moduleManager;

        public ModuleManagerTest( )
        {
            m_moduleManager=new ModuleManager( );
        }

        private class ModuleA : IModule
        {
            public string Name => "ModuleA";
            public string Description => "";
            public void Load( )
            {
            }
            public void Unload( )
            {
            }
        }

        private class ModuleB : IModule
        {
            public string Name => "ModuleB";
            public string Description => "";
            public void Load( )
            {
            }
            public void Unload( )
            {
            }
        }

        [ModuleDependency(typeof(ModuleA),typeof(ModuleB))]
        private class ModuleC : IModule
        {
            public string Name => "ModuleC";
            public string Description => "";
            public void Load( )
            {
            }
            public void Unload( )
            {
            }
        }

        [TestMethod]
        public void UT_ModuleManager_TestIf_AddModule_IsCorrect( )
        {
            Mock<IModule> mockModule = new Mock<IModule>( );
            mockModule.Setup(m => m.Name).Returns("Test");

            // Add a module named "Test"
            m_moduleManager.AddModule(mockModule.Object);

            // Should be able to get the object
            var module = m_moduleManager.GetModule("Test");
            var moduleA = m_moduleManager.GetModule("ModuleA");

            Assert.AreEqual("Test",module.Name);
            Assert.AreEqual("ModuleA",moduleA.Name);
            Assert.AreEqual(mockModule,module);
        }

        [TestMethod]
        public void UT_ModuleManager_TestIf_DependencyOrderIsCorrect( )
        {
            Mock<IModule> mockModule = new Mock<IModule>( );
            mockModule.Setup(m => m.Name).Returns("Test");

            // Add a module named "Test"
            m_moduleManager.AddModule(mockModule.Object);

            // Should be able to get the object
            var module = m_moduleManager.GetModule("Test");

            Assert.AreEqual("Test",module.Name);
            Assert.AreEqual(mockModule,module);
        }

    }
}