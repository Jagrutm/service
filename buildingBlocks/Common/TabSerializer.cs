using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.ComponentModel;

namespace CredECard.Common.BusinessService
{
    /// <author>Nidhi Thakrar</author>
    /// <created>29-Feb-2016</created>
    /// <summary>This class is used to serialize any object array to tab delimeted file. 
    /// This class uses TabElementAttribute to get headers.
    /// Uses "<b>ShouldSerialize</b>PropertyName" methods to determine whether to serialize perticular property or not</summary>
    public static class TabSerializer
    {
        private const string SEPARATOR = "\t";

        /// <author>Nidhi Thakrar</author>
        /// <created>29-Feb-2016</created>
        /// <summary>Serializes the specified type of object.
        /// This method will serialize all the properties with TabElementAttribute and all those properties must have ShouldSerializePROPNAME method.</summary>
        public static string Serialize(object[] objectArray, bool ignoreShouldSerialize = false)
        {
            Type typeOfObject = objectArray[0].GetType();

            PropertyInfo[] properties = typeOfObject.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(TabElementAttribute))
              && (ignoreShouldSerialize || Convert.ToBoolean(typeOfObject.GetMethod("ShouldSerialize" + prop.Name).Invoke(objectArray[0], null)))
              && (prop.GetCustomAttributes(typeof(TabElementAttribute), false).Cast<TabElementAttribute>().FirstOrDefault().IsRequreToSerialize)
           )
            .OrderBy(prop => prop.GetCustomAttributes(typeof(TabElementAttribute), false).Cast<TabElementAttribute>().FirstOrDefault().ElementName).ToArray<PropertyInfo>();

            string header = String.Join(SEPARATOR, properties.Select(f => f.GetCustomAttributes(typeof(TabElementAttribute), false).Cast<TabElementAttribute>().FirstOrDefault().ElementName).ToArray());

            StringBuilder data = new StringBuilder();
            data.AppendLine(header);

            foreach (var o in objectArray)
                data.AppendLine(getRecords(properties, o));

            return data.ToString();
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>29-Feb-2016</created>
        /// <summary>Gets the records.</summary>
        private static string getRecords(PropertyInfo[] properties, object obj)
        {
            StringBuilder record = new StringBuilder();

            foreach (PropertyInfo f in properties)
            {
                if (record.Length > 0)
                    record.Append(SEPARATOR);

                object value = null;
                try
                {
                    value = f.GetValue(obj, null);
                }
                catch (Exception ex)
                {
                    throw new TabSerializeException(f.Name, "Property not found", ex);
                }
                if (value != null)
                    record.Append(value.ToString());
            }

            return record.ToString();
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>07-Mar-2016</created>
        /// <summary>Deserializes the specified stream.</summary>
        public static object[] Deserialize(string fileName, Type returnObjectType, bool isFileHeaderPresent, ref string schemeCode)
        {
            if (!File.Exists(fileName))
                throw new TabSerializeException(fileName, "File doesn't exists.");

            string[] columns;
            string[] rows;
            string[] fileHeader;
            int recordCount = 0;
            object[] objectArray = null;

            PropertyInfo[] _properties = returnObjectType.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(TabElementAttribute))).ToArray<PropertyInfo>();

            try
            {
                using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    using (var sr = new StreamReader(stream))
                    {
                        if (isFileHeaderPresent)
                        {
                            fileHeader = sr.ReadLine().Split(Convert.ToChar(SEPARATOR));

                            if (fileHeader.Length == 4)
                            {
                                schemeCode = fileHeader[2];
                                int.TryParse(fileHeader[3], out recordCount);
                            }
                            else
                                throw new TabSerializeException(fileName, "Invalid file header");
                        }

                        columns = sr.ReadLine().Split(Convert.ToChar(SEPARATOR));
                        rows = sr.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Where(line => !string.IsNullOrEmpty(line.Trim())).ToArray<string>();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new TabSerializeException(fileName, "File format is invalid. Error while parsing", ex);
            }

            // match record count
            if (isFileHeaderPresent && rows.Length != recordCount)
                throw new TabSerializeException(fileName, String.Format("Record count mismatch. Records in file {0}, and records mentioned in file header {1}", rows.ToString(), recordCount.ToString()));

            //check if file contains valid headers or not
            string[] extraColumns = (from column in columns
                                     where !_properties.Select(f => f.GetCustomAttributes(typeof(TabElementAttribute), false).Cast<TabElementAttribute>().FirstOrDefault().ElementName.ToLower()).ToArray().Contains(column.ToLower())
                                     select column).ToArray<string>();
            if (extraColumns != null && extraColumns.Length > 0)
                throw new TabSerializeException(fileName, string.Format("Invalid header {0}", string.Join(", ", extraColumns)));

            for (int row = 0; row <= rows.Length - 1; row++)
            {
                if (objectArray == null)
                    objectArray = new object[rows.Length];

                var line = rows[row];
                var parts = line.Split(Convert.ToChar(SEPARATOR));

                if (parts.Length != columns.Length)
                    throw new TabSerializeException(fileName, string.Format("Invalid record. Line number {0}", row.ToString()));

                object obj = Activator.CreateInstance(returnObjectType, null);
                for (int i = 0; i < parts.Length; i++)
                {
                    var value = parts[i];
                    var column = columns[i];
                    var p = _properties.First(f => f.GetCustomAttributes(typeof(TabElementAttribute), false).Cast<TabElementAttribute>().FirstOrDefault().ElementName == column);

                    TabElementAttribute objElement = ((PropertyInfo)p).GetCustomAttributes(typeof(TabElementAttribute), false).Cast<TabElementAttribute>().FirstOrDefault<TabElementAttribute>();

                    if (objElement.ElementLength != 0 && value != string.Empty && value.Length > objElement.ElementLength)
                    {
                        string errorlength = string.Format("Invalid record. Line number {0}", row.ToString()) + " length of " + column + " Invalied.";
                        throw new TabSerializeException(fileName, errorlength);
                    }

                    if (objElement.IsMandatory == true && value == string.Empty)
                    {
                        string errorlength = string.Format("Invalid record. Line number {0}", row.ToString()) + " must provide " + column;
                        throw new TabSerializeException(fileName, errorlength);
                    }


                    p.SetValue(obj, value.ToString().Parse(p.PropertyType), null);
                }
                objectArray[row] = obj;
            }
            return objectArray;
        }

        /// <author>Nidhi Thakrar</author>
        /// <created>07-Mar-2016</created>
        /// <summary>Parses the specified type, type cast.</summary>
        public static object Parse(this string value, Type type)
        {
            return TypeDescriptor.GetConverter(type).ConvertFromString(value);
        }
    }
}
