//
//  List.cs
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
using System.Text;

using Couchbase.Lite;
using ToDoLiteForms.Model;

namespace ToDoLite.Documents
{
    public sealed class TaskList : Titled, ITaskList
    {
        internal const string ViewName = "lists";
        internal const string DocType = "list";
        private const string TaskViewByDate = "tasksByDate";

        private Database _database;

        public IProfile Owner { get; set; }

        public IList<string> Members { get; set; }

        internal string Id
        {
            get {
                return _document.Id;
            }
        }

        public TaskList(Database database, Document document) : base(document)
        {
            _database = database;
        }

        public IEnumerable<ITask> QueryTasks()
        {
            var view = _database.GetView(TaskViewByDate);
            if(view.Map == null) {
                view.SetMap((doc, emit) =>
                {
                    var type = doc.GetCast<string>("type");
                    if(Task.DocType.Equals(type)) {
                        var key = new List<object> { doc.Get("created_at"), doc.Get("list_id") };
                        emit(key, doc);
                    }
                }, "1");
            }

            var query = view.CreateQuery();
            foreach(var row in query.Run()) {
                yield return new Task(row.Document);
            }
        }

        public ITask AddTask(string title, IEnumerable<byte> image, string imageContentType)
        {
            var task = new Task(_database.CreateDocument());
            task.Title = title;
            task.List = this;
            task.SetImage(image, imageContentType);
            return task;
        }

        protected override bool SaveTo(IDictionary<string, object> props)
        {
            if(!base.SaveTo(props)) {
                return false;
            }


            props["type"] = DocType;
            props["owner"] = (Owner as Profile)?.Id;
            props["members"] = Members;
            return true;
        }
        protected override void RestoreFrom(IDictionary<string, object> props)
        {
            base.RestoreFrom(props);
            Members = props.Get("members") as IList<string>;
        }

        public void Delete()
        {
            foreach(var task in QueryTasks()) {
                task.Delete();
            }

            _document.Delete();
        }
    }
}
