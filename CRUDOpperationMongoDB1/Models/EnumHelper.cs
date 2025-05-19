using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
public static class EnumHelper
{
    // Cache: Type enum --> (enumName --> displayName)
    public static readonly ConcurrentDictionary<Type, Dictionary<string, string>> _cache = new();

        // Hàm mở rộng dùng để lấy tên hiển thị  từ Enum
    public static string GetDisplayName<TEnum>(this TEnum enumValue) where TEnum : Enum
    {
        var enumType = enumValue.GetType();
        var name = enumValue.ToString();
        var map = _cache.GetOrAdd(enumType, t =>
        {
            return t.GetFields(BindingFlags.Public | BindingFlags.Static).ToDictionary(f => f.Name, f =>
            {
                var disp = f.GetCustomAttribute<DisplayAttribute>()?.Name;
                if (!string.IsNullOrEmpty(disp)) return disp;
                var desc = f.GetCustomAttribute<DescriptionAttribute>()?.Description;
                return !string.IsNullOrEmpty(desc) ? desc : f.Name;
            });
        });
        return map.TryGetValue(name, out var display) ? display : name;
        

    }
 
 }
