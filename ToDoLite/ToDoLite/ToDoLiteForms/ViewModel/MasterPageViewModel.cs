//
//  MasterPageViewModel.cs
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ToDoLiteForms.Helpers;
using ToDoLiteForms.Model;
using ToDoLiteForms.Services;
using Xamarin.Forms;

namespace ToDoLiteForms.ViewModel
{
    public sealed class MasterPageViewModel : ViewModelBase
    {
        private readonly INavigator _navigator;
        private readonly MasterPageModel _model;

        public ObservableCollection<ITaskList> SavedLists { get; } = new ObservableCollection<ITaskList>();

        public ICommand AddButtonCommand { get; }

        public ICommand LoginButtonCommand { get; }

        public ITaskList SelectedItem
        {
            get {
                return null;
            }
            set {
                _navigator.PushAsync<DetailPageViewModel>(vm =>
                {
                    vm.List = value;
                }).ContinueWith(t =>
                {
                    Debug.WriteLine($"{t.Exception}");
                }, TaskContinuationOptions.OnlyOnFaulted);
                var dummy = default(ITaskList);
                SetProperty(ref dummy, value);
                SetProperty(ref dummy, null); // Disable selection
            }
        }

        public MasterPageViewModel(MasterPageModel model, INavigator navigator)
        {
            _navigator = navigator;
            _model = model;
            AddButtonCommand = new Command(() => CreateNewTask());
            LoginButtonCommand = new Command(() => navigator.PushModalAsync<LoginPageViewModel>());
            if (!model.LoginAsGuestIfNecessary())
            {
                navigator.PushModalAsync<LoginPageViewModel>();
            }
            SavedLists = model.GetExistingLists();
        }

        private async void CreateNewTask()
        {
            var result = await UserDialogs.Instance.PromptAsync("Title for new list.", "New ToDo List");
            if(result.Ok && !String.IsNullOrWhiteSpace(result.Text)) {
                var list = _model.CreateList(result.Text);
                SavedLists.Add(list);
            }
        }
    }
}
