//
//  ViewModelBase.cs
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
using System.Runtime.CompilerServices;

namespace ToDoLiteForms.ViewModel
{
    public abstract class ViewModelBase : IViewModel
    {
        public string Title { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetState<T>(Action<T> action) where T : class, IViewModel
        {
            action(this as T);
        }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if(Equals(storage, value)) return false;

            storage = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            return true;
        }
    }
}
