using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Core.Extensions
{
    public static class EnumExtension
    {
        public static string GetEnumNameByValue<TEnum>(int id) where TEnum : Enum
        {
            return Enum.GetName(typeof(TEnum), id);
        }

    }
}
