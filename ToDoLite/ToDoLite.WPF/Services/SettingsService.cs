//
//  SettingsService.cs
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
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoLite.Core.Services;

namespace ToDoLite.WPF.Services
{
    internal sealed class SettingsService : ISettingsService
    {
        private static readonly Configuration _Manager;

        static SettingsService()
        {
            _Manager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public string CurrentUserId
        {
            get {
                return _Manager.AppSettings.Settings[SettingsKeys.UserId].Value;
            }

            set {
                _Manager.AppSettings.Settings[SettingsKeys.UserId].Value = value;
                Refresh();
            }
        }

        public bool IsGuestLoggedIn
        {
            get {
                return Boolean.Parse(_Manager.AppSettings.Settings[SettingsKeys.Guest].Value);
            }

            set {
                _Manager.AppSettings.Settings[SettingsKeys.Guest].Value = value.ToString();
                Refresh();
            }
        }

        private void Refresh()
        {
            _Manager.Save(ConfigurationSaveMode.Full, true);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
