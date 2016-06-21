//
//  MasterPageModel.cs
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
using ToDoLiteForms.Helpers;
using ToDoLiteForms.Services;

namespace ToDoLiteForms.Model
{
    public sealed class MasterPageModel
    {
        private readonly ILoginService _loginService;
        private readonly IDatabaseService _databaseService;

        private bool ShouldLoginAsGuest
        {
            get {
                return _loginService.IsFirstTimeUsed || Settings.IsGuestLoggedIn;
            }
        }

        public MasterPageModel(ILoginService loginService, IDatabaseService databaseService)
        {
            _loginService = loginService;
            _databaseService = databaseService;
        }

        public ObservableCollection<ITaskList> GetExistingLists()
        {
            var retVal = new ObservableCollection<ITaskList>();
            foreach (var list in _databaseService.QueryLists())
            {
                retVal.Add(list);
            }

            return retVal;
        }

        public ITaskList CreateList(string title)
        {
            var list = _databaseService.CreateList();
            list.Title = title;
            var currentUserId = Settings.CurrentUserId;
            if (currentUserId != null)
            {
                var owner = _databaseService.GetProfile(null, currentUserId, false);
                list.Owner = owner;
            }

            list.Save();
            return list;
        }

        public bool LoginAsGuestIfNecessary()
        {
            if (ShouldLoginAsGuest)
            {
                LoginPageModel.LoginAsGuest(_databaseService);
            }

            return ShouldLoginAsGuest;
        }
    }
}
