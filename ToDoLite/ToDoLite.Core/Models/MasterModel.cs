//
//  MasterModel.cs
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
using ToDoLite.Core.Abstraction;
using ToDoLite.Core.Services;

namespace ToDoLite.Core.Models
{
    public sealed class MasterModel : IDisposable
    {
        private IDatabaseService _databaseService;
        private ISettingsService _settings;
        private ILiveQuery<ITaskList> _liveQuery;

        public MasterModel(IDatabaseService databaseService, ISettingsService settings)
        {
            _databaseService = databaseService;
            _settings = settings;
            if(_databaseService.IsFirstTimeRun || _settings.IsGuestLoggedIn) {
                _settings.IsGuestLoggedIn = true;
                _settings.CurrentUserId = null;
                _databaseService.LoadDatabaseFor(null);
            }
        }

        public ObservableCollection<ITaskList> GetExistingLists()
        {
            if(_liveQuery == null) {
                _liveQuery = _databaseService.LiveListQuery();
            }

            return _liveQuery.Collection;
        }

        public ITaskList CreateList(string title)
        {
            var list = _databaseService.CreateList();
            list.Title = title;
            var currentUserId = _settings.CurrentUserId;
            if(currentUserId != null) {
                var owner = _databaseService.GetProfile(null, currentUserId, false);
                list.Owner = owner;
            }

            list.Save();
            return list;
        }

        public void Dispose()
        {
            _liveQuery?.Dispose();
        }
    }
}
