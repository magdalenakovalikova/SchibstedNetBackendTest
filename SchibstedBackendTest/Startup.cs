﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SchibstedBackendTest.Startup))]

namespace SchibstedBackendTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}