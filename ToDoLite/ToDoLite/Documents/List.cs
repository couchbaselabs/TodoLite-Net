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

namespace ToDoLite.Documents
{
    public static class List
    {
        private const string ViewName = "lists";
        private const string DocType = "list";

        public static Query GetQuery(Database database)
        {
            var view = database.GetView(ViewName);
            if(view.Map == null) {
                view.SetMap((doc, emit) =>
                {
                    var type = doc.GetCast<string>("type");
                    if(DocType.Equals(type)) {
                        emit(doc.Get("title"), doc);
                    }
                }, "1");
            }

            return view.CreateQuery();
        }

        public static Document CreateNewList(Database database, string title, string userId)
        {
            var properties = new Dictionary<string, object> {
                ["type"] = DocType,
                ["title"] = title,
                ["created_at"] = DateTime.UtcNow,
                ["members"] = new List<string>()
            };

            if(userId != null) {
                properties["owner"] = userId;
            }

            var document = database.CreateDocument();
            document.PutProperties(properties);
            return document;
        }

        public static void AssignOwnerToListsIfNeeded(Database database, Document user)
        {
            var enumerator = GetQuery(database).Run();
            foreach(var row in enumerator) {
                var doc = row.Document;
                var owner = doc.GetProperty<string>("owner");
                if(owner != null) {
                    continue;
                }

                doc.Update(rev =>
                {
                    var props = rev.UserProperties;
                    props["owner"] = owner;
                    rev.SetUserProperties(props);
                    return true;
                });
            }
        }

        public static void AddMemberToList(Document list, Document user)
        {
            list.Update(rev =>
            {
                var props = rev.UserProperties;
                var members = props.GetCast<IList<string>>("members") ?? new List<string>();
                members.Add(user.Id);
                props["members"] = members;
                rev.SetUserProperties(props);

                return true;
            });
        }

        public static void RemoveMemberFromList(Document list, Document user)
        {
            list.Update(rev =>
            {
                var props = rev.UserProperties;
                var members = props.GetCast<IList<string>>("members") ?? new List<string>();
                members.Remove(user.Id);
                props["members"] = members;
                rev.SetUserProperties(props);

                return true;
            });
        }
    }
}
