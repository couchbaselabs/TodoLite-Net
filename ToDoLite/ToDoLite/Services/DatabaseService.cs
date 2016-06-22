//
//  DatabaseService.cs
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
using Couchbase.Lite;
using ToDoLite.Core.Abstraction;
using ToDoLite.Core.Services;
using ToDoLite.Documents;
using ToDoLite.Util;

namespace ToDoLite.Services
{
    class DatabaseService : IDatabaseService
    {
        private const string ProfileDocType = "profile";
        private Database _database;

        public bool IsFirstTimeRun
        {
            get {
                return !Manager.SharedInstance.AllDatabaseNames.Any();
            }
        }

        public ITaskList CreateList()
        {
            var doc = _database.CreateDocument();
            return new TaskList(doc);
        }

        public ITask CreateTask()
        {
            return new Task(_database.CreateDocument());
        }

        public IProfile GetProfile(string name, string userId, bool isNew)
        {
            var profileDocId = $"p:{userId}";
            if(!isNew) {
                var existingDoc = _database.GetExistingDocument(profileDocId);
                return existingDoc != null ? new Profile(existingDoc) : null;
            }

            var doc = _database.GetDocument(profileDocId);
            var profile = new Profile(doc);
            profile.Type = ProfileDocType;
            profile.Name = name;
            profile.UserId = userId;
            return profile;
        }

        public void LoadDatabaseFor(string userId)
        {
            _database = Manager.SharedInstance.GetDatabase(userId ?? "guest");
        }

        public ILiveQuery<ITaskList> LiveListQuery()
        {
            
            var query = ListQuery().ToLiveQuery();
            return new LiveQueryWrapper<ITaskList, TaskList>(query);
        }

        public void UpdateAllLists(IProfile owner)
        {
            foreach(var row in ListQuery().Run()) {
                var list = new TaskList(row.Document);
                list.Owner = owner;
                list.Save();
            }
        }

        private Query ListQuery()
        {
            var view = _database.GetView(TaskList.ViewName);
            if(view.Map == null) {
                view.SetMap((doc, emit) =>
                {
                    var type = doc.GetCast<string>("type");
                    if(TaskList.DocType.Equals(type)) {
                        emit(doc.Get("title"), doc);
                    }
                }, "1");
            }

            return view.CreateQuery();
        }
    }
}
