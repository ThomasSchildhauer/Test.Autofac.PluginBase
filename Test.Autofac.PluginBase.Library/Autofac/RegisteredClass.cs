using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Autofac.PluginBase.Library.Autofac
{
    public class RegisteredClass : IRegisteredClass
    {
        public void WriteToConsole()
        {
            Console.WriteLine("Injection worked");
        }
    }
}
