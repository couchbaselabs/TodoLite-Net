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
using ToDoLiteForms.Model;

namespace ToDoLite.Documents
{
    public class Task : Titled, ITask
    {
        internal const string DocType = "task";
        private const string ViewName = "tasks";
        private const string TaskImageName = "image";

        public bool IsChecked
        {
            get; set;
        }

        public ITaskList List
        {
            get; set;
        }

        public Task(Document document) : base(document)
        {
        }

        public void Delete()
        {
            _document.Delete();
        }

        protected override bool SaveTo(IDictionary<string, object> props)
        {
            if(!base.SaveTo(props)) {
                return false;
            }

            var list = List as TaskList;
            props["type"] = DocType;
            props["checked"] = IsChecked;
            props["list_id"] = list?.Id;
            return true;
        }

        protected override void RestoreFrom(IDictionary<string, object> props)
        {
            base.RestoreFrom(props);
            IsChecked = props.GetCast<bool>("checked");

        }

        public void SetImage(IEnumerable<byte> image, string contentType)
        {
            _document.Update(rev =>
            {
                rev.SetAttachment(TaskImageName, contentType, image);
                return true;
            });
        }
    }
}
