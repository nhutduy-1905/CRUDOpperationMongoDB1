using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;
using System;
using System.Linq;
using System.Reflection;
public static class EnumHelper
{
        public static string GetDisplayName<TEnum>(this TEnum enumValue) where TEnum : Enum
        {
            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            var displayAttr = memberInfo?.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;
        return displayAttr?.Name ?? enumValue.ToString();
        }
 }
