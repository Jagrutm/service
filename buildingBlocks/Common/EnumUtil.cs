using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CredECard.Common.BusinessService
{
    public class EnumUtil
    {
        /// <author>Nidhi Thakrar</author>
        /// <created>18-Mar-2015</created>
        /// <summary>Gets the description of enum.</summary>
        public static string GetEnumDescription(Enum Enum)
        {
            string description = string.Empty;

            FieldInfo field = Enum.GetType().GetField(Enum.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute != null)
                description = attribute.Description;

            return description;
        }

        public static T GetEnumValueFromDescription<T>(string description) // RS#112137 -- added ignoreCase
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (string.Compare(attribute.Description, description, false) == 0)
                        return (T)field.GetValue(null);

                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
        }


        /// <author>Denish Makwana</author>
        /// <created>09-MAR-2018</created>
        /// <summary>
        /// Method to GetEnumValueFromDescription
        /// </summary>
        /// <param name="value">description</param>
        /// <param name="ignoreCase">default false, true will ignore case on description comparision</param>
        /// <returns>returns string</returns>
        public static T GetEnumValueFromDescription<T>(string description, bool ignoreCase) // RS#112137 -- added ignoreCase
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (string.Compare(attribute.Description, description, ignoreCase) == 0)
                        return (T)field.GetValue(null);

                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", "description");
        }
    }
}
