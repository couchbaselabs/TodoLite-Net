//
//  Extensions.cs
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

namespace ToDoLite
{
    public static class Extensions
    {
        public static U Get<T, U>(this IDictionary<T, U> d, T key)
        {
            U val = default(U);
            d.TryGetValue(key, out val);
            return val;
        }

        public static bool TryGetValue<T>(this IDictionary<string, object> dic, string key, out T value)
        {
            value = default(T);
            object obj;
            if(!dic.TryGetValue(key, out obj)) {
                return false;
            }

            //If the types already match then things are easy
            if((obj is T)) {
                value = (T)obj;
                return true;
            }

            try {
                //Take the slow route for things like boxed value types
                value = (T)Convert.ChangeType(value, typeof(T));
                return true;
            } catch(Exception) {
                return false;
            }
        }

        public static bool TryCast<T>(object obj, out T castVal)
        {
            //If the types already match then things are easy
            if(obj is T) {
                castVal = (T)obj;
                return true;
            }

            try {
                //Take the slow route for things like boxed value types
                castVal = (T)Convert.ChangeType(obj, typeof(T));
            } catch(Exception) {
                castVal = default(T);
                return false;
            }

            return true;
        }

        public static T GetCast<T>(this IDictionary<string, object> collection, string key)
        {
            return collection.GetCast(key, default(T));
        }

        public static T GetCast<T>(this IDictionary<string, object> collection, string key, T defaultVal)
        {
            object value = collection.Get(key);
            return CastOrDefault<T>(value, defaultVal);
        }

        public static T CastOrDefault<T>(object obj, T defaultVal)
        {
            T retVal;
            if(obj != null && TryCast<T>(obj, out retVal)) {
                return retVal;
            }

            return defaultVal;
        }

        public static T CastOrDefault<T>(object obj)
        {
            return CastOrDefault<T>(obj, default(T));
        }
    }
}
