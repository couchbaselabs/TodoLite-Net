//
//  Titled.cs
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
    public abstract class Titled
    {
        protected readonly Document _document;

        protected Titled(Document doc)
        {
            _document = doc;
            Restore();
        }

        public string Title { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        internal string Id
        {
            get {
                return _document.Id;
            }
        }

        public void Restore()
        {
            if(_document.UserProperties == null) {
                return;
            }

            RestoreFrom(_document.UserProperties);
        }

        public void Save()
        {
            _document.Update(rev =>
            {
                var props = rev.UserProperties;
                var retVal = SaveTo(props);
                rev.SetUserProperties(props);
                return retVal;
            });
        }

        protected virtual bool SaveTo(IDictionary<string, object> props)
        {
            if(!props.ContainsKey("created_at")) {
                props["created_at"] = CreatedAt;
            }

            props["title"] = Title;
            return true;
        }

        protected virtual void RestoreFrom(IDictionary<string, object> props)
        {
            CreatedAt = props.GetCast<DateTime>("created_at", DateTime.UtcNow);
            Title = props.GetCast<string>("title");
        }
    }
}
