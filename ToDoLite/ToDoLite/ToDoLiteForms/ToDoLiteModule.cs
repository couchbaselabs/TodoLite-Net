//
//  ToDoLiteModule.cs
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
using ToDoLiteForms.Factories;
using ToDoLiteForms.Model;
using ToDoLiteForms.Services;
using ToDoLiteForms.View;
using ToDoLiteForms.ViewModel;
using Xamarin.Forms;

namespace ToDoLiteForms
{
    sealed class ToDoLiteModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginPageModel>();
            builder.RegisterType<MasterPageModel>();
            builder.RegisterType<LoginPageViewModel>();
            builder.RegisterType<MasterPageViewModel>();
            builder.RegisterType<LoginPage>().SingleInstance();
            builder.RegisterType<MasterPage>().SingleInstance();
        }
    }

    sealed class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // service registration
            builder.RegisterType<ViewFactory>()
                .As<IViewFactory>()
                .SingleInstance();

            builder.RegisterType<Navigator>()
                .As<INavigator>()
                .SingleInstance();

            // navigation registration
            builder.Register<INavigation>(context =>
                App.Current.MainPage.Navigation
            ).SingleInstance();
        }
    }
}
