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
        private readonly ILoginService _loginService;
        private readonly IDatabaseService _databaseService;
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
                });
                var dummy = default(ITaskList);
                SetProperty(ref dummy, null); // Disable selection
            }
        }

        public MasterPageViewModel(MasterPageModel model, IDatabaseService databaseService, ILoginService loginService, INavigator navigator)
        {
            _loginService = loginService;
            _databaseService = databaseService;
            _navigator = navigator;
            _model = model;
            AddButtonCommand = new Command(() => CreateNewTask());
            LoginButtonCommand = new Command(() => navigator.PushModalAsync<LoginPageViewModel>());
            if(!_model.ShouldLoginAsGuest) {
                navigator.PushModalAsync<LoginPageViewModel>();
            } else {
                LoginPageViewModel.LoginAsGuest(databaseService);
            }

            foreach(var list in _databaseService.QueryLists()) {
                SavedLists.Add(list);
            }
        }

        private async void CreateNewTask()
        {
            var result = await UserDialogs.Instance.PromptAsync("Title for new list.", "New ToDo List");
            if(result.Ok && !String.IsNullOrWhiteSpace(result.Text)) {
                var list = CreateList(result.Text);
                SavedLists.Add(list);
            }
        }

        private ITaskList CreateList(string title)
        {
            var list = _databaseService.CreateList();
            list.Title = title;
            var currentUserId = Settings.CurrentUserId;
            if(currentUserId != null) {
                var owner = _databaseService.GetProfile(null, currentUserId, false);
                list.Owner = owner;
            }

            list.Save();
            return list;
        }
    }
}
