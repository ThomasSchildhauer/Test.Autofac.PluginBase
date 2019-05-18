using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Autofac.PluginBase.Interfaces;

namespace Test.Autofac.PluginBase.Library.Autofac
{
    public class PluginStartClass : IGamesPlugin
    {
        private IRegisteredClass _registeredClass;

        public PluginStartClass(IRegisteredClass registeredClass)
        {
            _registeredClass = registeredClass;
        }

        public void OnStartup()
        {
            _registeredClass.WriteToConsole();
        }
    }
}
