//
//  Task.cs
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
using System.IO;
using System.Text;

using Couchbase.Lite;

namespace ToDoLite.Documents
{
    public static class Task
    {
        private const string ViewName = "tasks";
        private const string DocType = "task";

        public static Query GetQuery(Database database, string listDocId)
        {
            var view = database.GetView(ViewName);
            if(view.Map == null) {
                view.SetMap((doc, emit) =>
                {
                    if(DocType.Equals(doc.GetCast<string>("type"))) {
                        var key = new List<object> { doc.Get("list_id"), doc.Get("created_at") };
                        emit(key, doc);
                    }
                }, "1");
            }

            var query = view.CreateQuery();
            query.Descending = true;
            query.StartKey = new List<object> { listDocId, new Dictionary<string, object>() };
            query.EndKey = new List<object> { listDocId };

            return query;
        }

        public static Document CreateTask(Database database, string title, Stream image, string listId)
        {
            var properties = new Dictionary<string, object> {
                ["type"] = DocType,
                ["title"] = title,
                ["checked"] = false,
                ["created_at"] = DateTime.UtcNow,
                ["list_id"] = listId
            };

            var document = database.CreateDocument();
            var revision = document.CreateRevision();
            revision.SetUserProperties(properties);

            if(image != null) { 
                revision.SetAttachment("image", "image/jpg", image);
            }

            revision.Save();
            return document;
        }

        public static void AttachImage(Document task, Stream image)
        {
            if(task == null || image == null) {
                return;
            }

            var revision = task.CreateRevision();
            revision.SetAttachment("image", "image/jpg", image);
            revision.Save();
        }

        public static void UpdateCheckedStatus(Document task, bool isChecked)
        {
            task.Update(rev =>
            {
                var props = rev.UserProperties;
                props["checked"] = isChecked;
                rev.SetUserProperties(props);
                return true;
            });
        }

        public static void DeleteTask(Document task)
        {
            task.Delete();
        }
    }
}
