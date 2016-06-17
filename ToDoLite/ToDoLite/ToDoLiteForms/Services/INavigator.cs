//
//  INavigator.cs
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
using System.Threading.Tasks;
using ToDoLiteForms.Factories;
using ToDoLiteForms.ViewModel;
using Xamarin.Forms;

namespace ToDoLiteForms.Services
{
    public interface INavigator
    {
        Task<IViewModel> PopAsync();

        Task<IViewModel> PopModalAsync();

        Task PopToRootAsync();

        Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;
    }

    public class Navigator : INavigator
    {
        private readonly Lazy<INavigation> _navigation;
        private readonly IViewFactory _viewFactory;

        public Navigator(Lazy<INavigation> navigation, IViewFactory viewFactory)
        {
            _navigation = navigation;
            _viewFactory = viewFactory;
        }

        private INavigation Navigation
        {
            get { return _navigation.Value; }
        }

        public async Task<IViewModel> PopAsync()
        {
            Page view = await Navigation.PopAsync();
            return view.BindingContext as IViewModel;
        }

        public async Task<IViewModel> PopModalAsync()
        {
            Page view = await Navigation.PopAsync();
            return view.BindingContext as IViewModel;
        }

        public async Task PopToRootAsync()
        {
            await Navigation.PopToRootAsync();
        }

        public async Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = _viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
            await Navigation.PushAsync(view);
            return viewModel;
        }

        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            await Navigation.PushAsync(view);
            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = _viewFactory.Resolve<TViewModel>(out viewModel, setStateAction);
            await Navigation.PushModalAsync(view);
            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            await Navigation.PushModalAsync(view);
            return viewModel;
        }
    }
}
