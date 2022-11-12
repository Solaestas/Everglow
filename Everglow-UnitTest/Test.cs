using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Everglow_UnitTest
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class MyTestClass
    {
        private class A
        {
        }

        private class B
        {
        }

        [TestMethod]
        public void MyTestMethod()
        {
            T1((A)null);
        }

        private void T1(A a)
        { }

        private void T1(B b)
        { }
    }
}