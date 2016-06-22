﻿//
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
    internal abstract class Titled : DocumentBase
    {
        protected Titled(Document doc) : base(doc)
        {
            Restore();
        }

        public string Title { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        protected override bool SaveTo(IDictionary<string, object> props)
        {
            if(!props.ContainsKey("created_at")) {
                props["created_at"] = CreatedAt;
            }

            props["title"] = Title;
            return true;
        }

        protected override void RestoreFrom(IDictionary<string, object> props)
        {
            CreatedAt = props.GetCast<DateTime>("created_at", DateTime.UtcNow);
            Title = props.GetCast<string>("title");
        }
    }
}
