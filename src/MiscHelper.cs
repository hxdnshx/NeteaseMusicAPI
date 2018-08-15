using System;
using System.Collections.Generic;
using System.Text;

namespace NeteaseMusicAPI.Internal
{
    public static class MiscHelper
    {
        /// <summary>
        /// Get default value of specfied type.
        /// Ref : https://stackoverflow.com/questions/1281161/how-to-get-the-default-value-of-a-type-if-the-type-is-only-known-as-system-type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}
