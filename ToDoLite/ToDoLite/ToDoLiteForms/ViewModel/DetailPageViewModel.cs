//
//  DetailPageViewModel.cs
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoLiteForms.Model;
using ToDoLiteForms.Services;
using Xamarin.Forms;

namespace ToDoLiteForms.ViewModel
{
    public sealed class DetailPageListViewItem
    {
        private ITask _task;

        public string Text { get; set; }

        public ImageSource ImageSource { get; set; }

        public DetailPageListViewItem(ITask task)
        {
            _task = task;
            Text = task.Title;
            var hasImage = task.GetImage() != null;
            ImageSource = hasImage ?
                Device.OnPlatform(ImageSource.FromFile("camera"), ImageSource.FromFile("ic_camera"), null) :
                Device.OnPlatform(ImageSource.FromFile("camera_light"), ImageSource.FromFile("ic_camera_light"), null);
        }
    }

    public sealed class DetailPageViewModel : ViewModelBase
    {
        public ImageSource AddTaskImageSource
        {
            get {
                return Device.OnPlatform(ImageSource.FromFile("camera"), ImageSource.FromFile("ic_camera"), null);
            }

        }

        public string AddTaskText
        {
            get {
                return _addTaskText;
            }
            set {
                SetProperty(ref _addTaskText, value);
            }
        }
        private string _addTaskText;

        public ObservableCollection<DetailPageListViewItem> TaskItemsSource { get; } = new ObservableCollection<DetailPageListViewItem>();

        internal ITaskList List { get; set; }

        public DetailPageViewModel(INavigator navigator, IDatabaseService databaseService)
        {
            
        }

        public void AddNewTask()
        {
            if(String.IsNullOrEmpty(AddTaskText)) {
                return;
            }

            var addTaskText = AddTaskText;
            AddTaskText = null;
            var task = List.AddTask(addTaskText, null, null);
            task.Save();
            TaskItemsSource.Add(new DetailPageListViewItem(task));
        }
    }
}
