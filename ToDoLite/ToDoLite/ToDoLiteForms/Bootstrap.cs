//
//  Bootstrap.cs
//
//  Author:
//  	Jim Borden  <jim.borden@couchbase.com>
//
//  Copyright (c) 2016 Couchbase, Inc All rights reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using ToDoLiteForms.Factories;
using ToDoLiteForms.View;
using ToDoLiteForms.ViewModel;
using Xamarin.Forms;

namespace ToDoLiteForms
{
    public class Bootstrapper : AutofacBootstrapper
    {
        private readonly App _application;
        private readonly IModule[] _platformModules;

        public Bootstrapper(App application, params IModule[] platformModules)
        {
            _application = application;
            _platformModules = platformModules;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterModule<ToDoLiteModule>();
            foreach(var module in _platformModules) {
                builder.RegisterModule(module);
            }
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<LoginPageViewModel, LoginPage>();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            // set main page
            var viewFactory = container.Resolve<IViewFactory>();
            var mainPage = viewFactory.Resolve<LoginPageViewModel>();
            var navigationPage = new NavigationPage(mainPage);

            _application.MainPage = navigationPage;
        }
    }

    public abstract class AutofacBootstrapper
    {
        public void Run()
        {
            var builder = new ContainerBuilder();

            ConfigureContainer(builder);

            var container = builder.Build();
            var viewFactory = container.Resolve<IViewFactory>();

            RegisterViews(viewFactory);

            ConfigureApplication(container);
        }

        protected virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacModule>();
        }

        protected abstract void RegisterViews(IViewFactory viewFactory);

        protected abstract void ConfigureApplication(IContainer container);
    }
}
