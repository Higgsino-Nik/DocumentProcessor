using System.ComponentModel;

namespace DocumentProcessor.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var members = value.GetType().GetMember(value.ToString());
            if (members is null || members.Length == 0)
                return value.ToString();

            var attrs = members[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attrs is null || attrs.Length == 0
                ? value.ToString()
                : ((DescriptionAttribute)attrs[0]).Description;
        }
    }
}
