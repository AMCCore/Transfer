using Transfer.Common.Attributes;

namespace Transfer.Common.Extensions;

public static class GuidEnumConverterExtension
{
    public static TEnum ConvertGuidToEnum<TEnum>(this Guid value) where TEnum : Enum
    {
        var names = Enum.GetNames(typeof(TEnum));
        foreach (var name in names)
        {
            var val = typeof(TEnum).GetField(name).GetCustomAttributes(true).OfType<EnumGuidAttribute>()
                .Select(ss => ss.Guid)
                .FirstOrDefault();
            if (val == value)
            {
                return (TEnum)Enum.Parse(typeof(TEnum), name);
            }
        }

        throw new InvalidOperationException();
    }
}
