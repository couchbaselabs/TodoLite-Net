//
//  ViewFactory.cs
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
using ToDoLiteForms.ViewModel;
using Xamarin.Forms;

namespace ToDoLiteForms.Factories
{
    public interface IViewFactory
    {
        void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Page;

        Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Page Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Page Resolve<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;
    }

    public class ViewFactory : IViewFactory
    {
        private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();
        private readonly IComponentContext _componentContext;

        public ViewFactory(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Page
        {
            _map[typeof(TViewModel)] = typeof(TView);
        }

        public Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null) where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            return Resolve<TViewModel>(out viewModel, setStateAction);
        }

        public Page Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            viewModel = _componentContext.Resolve<TViewModel>();

            var viewType = _map[typeof(TViewModel)];
            var view = _componentContext.Resolve(viewType) as Page;

            if(setStateAction != null)
                viewModel.SetState(setStateAction);

            view.BindingContext = viewModel;
            return view;
        }

        public Page Resolve<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var viewType = _map[typeof(TViewModel)];
            var view = _componentContext.Resolve(viewType) as Page;
            view.BindingContext = viewModel;
            return view;
        }
    }
}
