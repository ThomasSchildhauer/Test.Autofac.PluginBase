﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Autofac.PluginBase.Interfaces;

namespace Test.Autofac.PluginBase.Library
{
    public class ClassB : IGamesPlugin
    {
        public void OnStartup()
        {
            Console.WriteLine("Class B");
        }
    }
}
