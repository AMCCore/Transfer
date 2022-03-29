using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Linq;
using System.Linq.Expressions;
using Transfer.Common.Attributes;
using Transfer.Common.Extensions;

namespace Transfer.Dal.Helpers
{
    public class GuidEnumConverter<TEnum> : ValueConverter<TEnum, Guid> where TEnum : Enum
    {
        public GuidEnumConverter(ConverterMappingHints mappingHints = null) : base(ToGuid(), ToEnum(), mappingHints)
        {
        }

        protected static Expression<Func<TEnum, Guid>> ToGuid() => v => v.GetEnumGuid();

        protected static Expression<Func<Guid, TEnum>> ToEnum() => v => ConvertToEnum(v);

        //public static TEnum ConvertToEnum(Guid value)
        //{
        //    var names = Enum.GetNames(typeof(TEnum));
        //    foreach (var name in names)
        //    {
        //        var val = typeof(TEnum).GetField(name).GetCustomAttributes(true).OfType<EnumGuidAttribute>()
        //            .Select(ss => ss.Guid)
        //            .FirstOrDefault();
        //        if (val == value)
        //        {
        //            return (TEnum)Enum.Parse(typeof(TEnum), name);
        //        }
        //    }

        //    throw new InvalidOperationException();
        //}

        public static TEnum ConvertToEnum(Guid value)
        {
            return value.ConvertGuidToEnum<TEnum>();
        }
    }
}
