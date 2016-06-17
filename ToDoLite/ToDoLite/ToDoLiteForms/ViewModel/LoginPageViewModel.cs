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
using System.Windows.Input;
using ToDoLiteForms.Model;
using ToDoLiteForms.Services;
using ToDoLiteForms.View;
using Xamarin.Forms;

namespace ToDoLiteForms.ViewModel
{
    public sealed class LoginPageViewModel : ViewModelBase
    {
        private ICommand _continueButtonClicked;
        private readonly LoginPageModel _model;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<ContentPage> PageChangeRequested;

        public string ContinueButtonText
        {
            get { return _model.ShouldLoginAsGuest ? "YES" : "NO"; }
        }

        public ICommand ContinueButtonClicked
        {
            get {
                return _continueButtonClicked ?? (_continueButtonClicked = new Command(() => PageChangeRequested?.Invoke(this, new MasterPage())));
            }
        }

        public LoginPageViewModel(LoginPageModel model)
        {
            _model = model;
        }
    }
}
