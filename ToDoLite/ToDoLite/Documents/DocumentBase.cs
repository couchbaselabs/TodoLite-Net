//
//  DocumentBase.cs
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
using ToDoLite.Core.Abstraction;

namespace ToDoLite.Documents
{
    internal abstract class DocumentBase : IItemBase
    {
        protected readonly Document _document;

        internal string Id
        {
            get {
                return _document.Id;
            }
        }

        protected DocumentBase(Document document)
        {
            _document = document;
        }

        public void Restore()
        {
            if(_document.UserProperties == null) {
                return;
            }

            RestoreFrom(_document.UserProperties);
        }

        public void Delete()
        {
            OnDelete();
            _document.Delete();
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

        protected abstract void RestoreFrom(IDictionary<string, object> properties);

        protected abstract bool SaveTo(IDictionary<string, object> properties);

        protected virtual void OnDelete() { }

        public override bool Equals(object obj)
        {
            return Id == (obj as DocumentBase)?.Id;
        }

        public override int GetHashCode()
        {
            return Id == null ? 0 : Id.GetHashCode();
        }
    }
}
