using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transfer.Common.Attributes;

namespace Transfer.Common.Extensions
{
    public static class EnumExtension
    {
        public static Guid GetEnumGuid(this Enum e)
        {
            var enumType = e.GetType();
            var name = Enum.GetName(enumType, e);
            var res = enumType.GetField(name).GetCustomAttributes(typeof(EnumGuidAttribute), true).Cast<EnumGuidAttribute>().Select(s => s.Guid).FirstOrDefault();
            return res;
        }

        public static string GetEnumDescription(this Enum e)
        {
            var enumType = e.GetType();
            var name = Enum.GetName(enumType, e);
            var res = enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>().Select(s => s.Description).FirstOrDefault();
            return res;
        }

        /// <summary>
        ///     Возвращает все элементы перечисления в виде IEnumerable&lt;T&gt;
        /// </summary>
        public static IEnumerable<T> Values<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}
