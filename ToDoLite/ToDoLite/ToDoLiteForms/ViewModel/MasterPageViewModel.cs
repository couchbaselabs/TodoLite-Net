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
using ToDoLiteForms.Model;
using ToDoLiteForms.Services;
using Xamarin.Forms;

namespace ToDoLiteForms.ViewModel
{
    public class Foo
    {
        public string Text { get; set; }
    }
    public sealed class MasterPageViewModel : ViewModelBase
    {
        private readonly ILoginService _loginService;
        private readonly IDatabaseService _databaseService;
        private readonly INavigator _navigator;
        private readonly MasterPageModel _model;

        public ObservableCollection<Foo> ListOfStuff { get; } = new ObservableCollection<Foo>();

        public ICommand AddButtonCommand { get; }

        public ICommand LoginButtonCommand { get; }

        public MasterPageViewModel(MasterPageModel model, IDatabaseService databaseService, ILoginService loginService, INavigator navigator)
        {
            _loginService = loginService;
            _databaseService = databaseService;
            _navigator = navigator;
            _model = model;
            AddButtonCommand = new Command(() => ListOfStuff.Add(new Foo { Text = "Hi" }));
            LoginButtonCommand = new Command(() => navigator.PushModalAsync<LoginPageViewModel>());
            if(!_model.ShouldLoginAsGuest) {
                navigator.PushModalAsync<LoginPageViewModel>();
            } else {
                LoginPageViewModel.LoginAsGuest(databaseService);
            }
        }
    }
}
