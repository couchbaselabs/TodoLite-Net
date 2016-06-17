using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Core;
using ToDoLiteForms.Services;
using ToDoLiteForms.View;
using Xamarin.Forms;

namespace ToDoLiteForms
{
    public class App : Application
    {
        public App (params IModule[] modules)
        {
            // The root page of your application
            var bootstrapper = new Bootstrapper(this, modules);
            bootstrapper.Run();
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }
    }
}
