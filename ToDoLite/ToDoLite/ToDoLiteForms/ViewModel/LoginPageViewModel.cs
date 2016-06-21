//
//  LoginPageViewModel.cs
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
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ToDoLiteForms.Helpers;
using ToDoLiteForms.Model;
using ToDoLiteForms.Services;
using ToDoLiteForms.View;
using Xamarin.Forms;

namespace ToDoLiteForms.ViewModel
{
    public sealed class LoginPageViewModel : ViewModelBase
    {
        private ICommand _continueButtonClicked;
        private INavigator _navigator;
        private LoginPageModel _model;

        public string ContinueButtonText
        {
            get { return LoginPageModel.ContinueButtonText; }
        }

        public ICommand ContinueButtonClicked
        {
            get {
                return _continueButtonClicked ?? (_continueButtonClicked = new Command(LoginAsGuest));
            }
        }

        public LoginPageViewModel(LoginPageModel model, INavigator navigator)
        {
            _model = model;
            _navigator = navigator;
        }

        private void LoginAsGuest()
        {
            _model.LoginAsGuest();
            _navigator.PopModalAsync();
        }
    }
}
