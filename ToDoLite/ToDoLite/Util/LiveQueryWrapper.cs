//
//  LiveQueryWrapper.cs
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
using System.Text;
using Couchbase.Lite;
using ToDoLite.Core.Abstraction;
using ToDoLite.Documents;

namespace ToDoLite.Util
{
    sealed class LiveQueryWrapper<IType, CType> : ILiveQuery<IType> where CType : DocumentBase, IType
    {
        private readonly LiveQuery _query;

        public ObservableCollection<IType> Collection { get; } = new ObservableCollection<IType>();

        public LiveQueryWrapper(LiveQuery query)
        {
            _query = query;
            query.Changed += Query_Changed;
            query.Start();
        }

        private void Query_Changed(object sender, QueryChangeEventArgs e)
        {
            Collection.Clear();
            foreach(var row in e.Rows) {
                var concrete = (CType)Activator.CreateInstance(typeof(CType), row.Document);
                Collection.Add(concrete);
            }
        }

        public void Dispose()
        {
            _query.Stop();
            _query.Dispose();
        }
    }
}
