using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTester.Interfaces
{
    interface IZTester
    {
        void StartTest();
        void SetTheEnvironment();
        void FinishTest();
    }
}
