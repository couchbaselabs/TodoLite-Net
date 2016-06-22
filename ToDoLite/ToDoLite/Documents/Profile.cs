//
//  Profile.cs
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

namespace ToDoLite.Documents
{
    internal sealed class Profile : DocumentBase, IProfile
    {
        private const string ViewName = "profiles";
        private const string ByIdViewName = "profiles_by_id";
        private const string DocType = "profile";

        public string Name { get; set; }

        public string UserId { get; set;
        }
        public string Type { get; set; }

        public Profile(Document document) : base(document)
        {
        }

        public static Query GetQuery(Database database, string ignoreUserId)
        {
            var view = database.GetView(ViewName);
            if(view.Map == null) {
                view.SetMap((doc, emit) =>
                {
                    if(DocType.Equals(doc.GetCast<string>("type"))) {
                        if(ignoreUserId == null || !ignoreUserId.Equals(doc.GetCast<string>("user_id"))) {
                            emit(doc.Get("name"), doc);
                        }
                    }
                }, "1");
            }

            return view.CreateQuery();
        }

        public static Query GetQueryById(Database database, string userId)
        {
            var view = database.GetView(ByIdViewName);
            if(view.Map == null) {
                view.SetMap((doc, emit) =>
                {
                    if(DocType.Equals(doc.GetCast<string>("type"))) {
                        emit(doc.Get("user_id"), doc);
                    }
                }, "1");
            }

            var query = view.CreateQuery();
            query.Keys = new List<object> { userId };
            return query;
        }

        public static Document GetUserProfileById(Database database, string userId)
        {
            var profile = default(Document);
            try {
                var enumerator = Profile.GetQueryById(database, userId).Run();
                profile = enumerator?.ElementAtOrDefault(0)?.Document;
            } catch(CouchbaseLiteException) { }

            return profile;
        }

        public static Document CreateProfile(Database database, string userId, string name)
        {
            var properties = new Dictionary<string, object> {
                ["type"] = DocType,
                ["user_id"] = userId,
                ["name"] = name
            };

            var document = database.GetDocument($"p:{userId}");
            document.PutProperties(properties);
            return document;
        }

        protected override void RestoreFrom(IDictionary<string, object> properties)
        {
            throw new NotImplementedException();
        }

        protected override bool SaveTo(IDictionary<string, object> properties)
        {
            throw new NotImplementedException();
        }
    }
}
