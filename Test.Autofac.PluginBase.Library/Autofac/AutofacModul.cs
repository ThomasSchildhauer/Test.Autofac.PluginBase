using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Autofac.PluginBase.Library.Autofac
{
    public class AutofacModul : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegisteredClass>().As<IRegisteredClass>();
        }
    }
}
