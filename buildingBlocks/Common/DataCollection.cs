namespace CredECard.Common.BusinessService
{
	using System;
    using System.ComponentModel;
    using System.Collections;
    using System.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CredECard.Common.BusinessService;

    /// <summary>
    /// Inherit from this when you need a collection which provides database functionalty
    /// </summary>
    [Serializable()]
	public abstract class DataCollection : CollectionBase, IPersistable
	{
        /// <Author>Dharati Metra</Author>
        /// <CreatedDate>02-Sep-2016</CreatedDate>
        /// <summary>Gets or sets a value indicating whether [show error].</summary>
        /// <value><c>true</c> if [show error]; otherwise, <c>false</c>.</value>
        public bool ShowError { get; set; }

        /// <summary>
        /// Adds item to list
        /// </summary>
        /// <param name="item">DataItem</param>
		public void AddSingle(DataItem item)
		{
			List.Add(item);
		}

        /// <summary>
        /// Adds object to list
        /// </summary>
        /// <param name="item">DataItem</param>
        public void AddSingle(object item)
        {
            List.Add(item);
        }

        /// <summary>
        /// Adds a range of items from a list into this list
        /// </summary>
        /// <param name="range">DataCollection</param>
		public void AddRange(DataCollection range)
		{
			foreach(DataItem singleItem in range)
			{
				List.Add(singleItem);
			}
		}

        /// <summary>
        /// Saves the item
        /// </summary>
		public virtual void Save()
		{
			if (this is IPersistableV2)
			{
				DataController con = new DataController();
                con.StartDatabase(CredECard.Common.BusinessService.CredECardConfig.GetReadWriteConnectionString());
				((IPersistableV2)this).Save(con);
				con.EndDatabase();
			}
			else
			{
				foreach (DataItem saveItem in this)
				{
					saveItem.Save();
				}
			}
		}

        /// <summary>
        /// Validates item before save
        /// </summary>
        /// <returns>ValidateResult</returns>
		public virtual ValidateResult Validate()
		{
			// Create a the final result
			ValidateResult newResult = new ValidateResult(null);
			
			// Validate all the DataItem files
			foreach (DataItem valItem in this)
			{
				// Validate the item and add any errors to final result
				ValidateResult tmpResult = valItem.Validate();
				if (!tmpResult.DataOk) newResult.ErrorList.AddRange(tmpResult.ErrorList);
			}

			// Return the final validation result
			return newResult;
		}

        /// <author>Nidhi Thakrar</author>
        /// <created>26-Mar-2015</created>
        /// <summary>Gets or sets the element at the specified index.</summary>
        public DataItem this[int index]
        {
            get
            {
                if (index > this.Count - 1 || index < 0)
                    throw new IndexOutOfRangeException(CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
                else
                    return (DataItem)List[index];
            }
        }

        /// <created>01-May-2015</created>
        /// <author>Nidhi Thakrar</author>
        /// <summary>this method will sort provided list object</summary>
        /// <param name="data">DataCollection object</param>
        /// <param name="sortExpressions">provide the list of Properties name on which sorting is required</param>
        public static DataCollection OrderBy(DataCollection data, List<KeyValuePair<string, bool>> sortExpressions)
        {
            // No sorting needed
            if ((sortExpressions == null) || (sortExpressions.Count <= 0))
                return data;

            // Let us sort it
            //IEnumerable<DataCollection> query = from DataCollection item in data select item;
            IEnumerable<object> query = data.Cast<object>();
            IOrderedEnumerable<object> orderedQuery = null;

            for (int i = 0; i < sortExpressions.Count; i++)
            {
                // We need to keep the loop index, not sure why it is altered by the Linq.
                var index = i;                
                Func<object, object> expression = item => getPropertyValue(item, item.GetType(), sortExpressions[index].Key);

                if (sortExpressions[index].Value)
                    orderedQuery = (index == 0) ? query.OrderBy(expression) : orderedQuery.ThenBy(expression);
                else
                    orderedQuery = (index == 0) ? query.OrderByDescending(expression) : orderedQuery.ThenByDescending(expression);
            }

            query = orderedQuery;
            object sortedList = null;

            foreach (object item in query)
            {
                if (sortedList == null) sortedList = Activator.CreateInstance(data.GetType().Assembly.GetType(data.GetType().FullName));
                ((DataCollection)sortedList).AddSingle(item);
            }

            if (sortedList != null)
                ((DataCollection)sortedList).ShowError = data.ShowError;

            return sortedList == null ? data : (DataCollection)sortedList;
        }

        /// <Author>Dharati Metra</Author>
        /// <CreatedDate>30-aug-2016</CreatedDate>
        /// <summary>Sums the specified data.</summary>
        public static string Sum(DataCollection data, string field)
        {
            if (data == null)
                return "";

            IEnumerable<object> query = data.Cast<object>();
            Func<object, double> expression = item => Convert.ToDouble(getPropertyValue(item, item.GetType(), field));
            return query.Sum(expression).ToString("0.00");
        }

        /// <Author>Dharati Metra</Author>
        /// <CreatedDate>30-aug-2016</CreatedDate>
        /// <summary>Searches the specified data.</summary>
        public static DataCollection Search(DataCollection data, DataColumn column)
        {
            if (data == null)
                return data;
            // No field or value passed
            if (column == null)
                return data;

            // Let us sort it
            IEnumerable<object> query = data.Cast<object>();
            Func<object, bool> expression = null;

            switch (column.Type)
            {
                case EnumDataColumnType.Amount:
                    double amount = double.MinValue;
                    double.TryParse(column.ValueField, out amount);
                    expression = item => Convert.ToDouble(getPropertyValue(item, item.GetType(), column.TextField)) == amount;
                    break;
                case EnumDataColumnType.DateTime:
                    DateTime date = DateTime.MinValue;
                    DateTime.TryParse(column.ValueField, out date);
                    //expression = item => Convert.ToDateTime(getPropertyValue(item, item.GetType(), column.TextField)).ToShortDateString() == date.ToShortDateString();
                    expression = item => Convert.ToDateTime(getPropertyValue(item, item.GetType(), column.TextField)).ToString("dd/MM/yyyy") == date.ToString("dd/MM/yyyy");
                    break;
                case EnumDataColumnType.Text:
                case EnumDataColumnType.Normal:  // Bhavik Patel : 05-Aug-2016 : C:28104
                    expression = item => getPropertyValue(item, item.GetType(), column.TextField).ToString().ToLower().Contains(column.ValueField.ToLower());
                    break;
                default:
                    expression = item => getPropertyValue(item, item.GetType(), column.TextField).ToString() == column.ValueField;
                    break;
            }

            IEnumerable<object> filteredQuery = query.Where(expression);

            query = filteredQuery;

            object sortedList = null;

            if (query != null)
            {
                foreach (object item in query)
                {
                    if (sortedList == null) sortedList = Activator.CreateInstance(data.GetType().Assembly.GetType(data.GetType().FullName));
                    ((DataCollection)sortedList).AddSingle(item);
                }
            }

            if (sortedList != null)
                ((DataCollection)sortedList).ShowError = data.ShowError;

            return sortedList == null ? null : (DataCollection)sortedList;
        }

        /// <Author>Dharati Metra</Author>
        /// <CreatedDate>30-aug-2016</CreatedDate>
        /// <summary>Searches the specified data.</summary>
        public static DataCollection Search(DataCollection data, List<DataColumn> searchConditions)
        {
            if (data == null)
                return data;

            if ((searchConditions == null) || (searchConditions.Count <= 0))
                return data;

            object finalList = null;

            for (int i = 0; i < searchConditions.Count; i++)
            {
                var index = i;
                DataCollection searched = null;

                if (index == 0)
                    searched = DataCollection.Search(data, searchConditions[index]);
                else
                    searched = DataCollection.Search((DataCollection)finalList, searchConditions[index]);

                finalList = Activator.CreateInstance(data.GetType().Assembly.GetType(data.GetType().FullName));

                if (searched != null)
                    ((DataCollection)finalList).AddRange(searched);
            }

            if (finalList != null)
                ((DataCollection)finalList).ShowError = data.ShowError;

            return (DataCollection)finalList;
        }

        /// <Author>Dharati Metra</Author>
        /// <CreatedDate>02-Sep-2016</CreatedDate>
        /// <summary>Gets the property information.</summary>
        private static PropertyInfo getPropertyInfo(object obj, Type type, string propertyName)
        {
            if (propertyName.Contains("."))//complex type nested
            {
                var temp = propertyName.Split(new char[] { '.' }, 2);
                var prop = getPropertyInfo(obj, type, temp[0]);
                return getPropertyInfo(prop, prop.PropertyType, temp[1]);
            }
            else
                return type.GetProperty(propertyName);
        }

        /// <Author>Dharati Metra</Author>
        /// <CreatedDate>02-Sep-2016</CreatedDate>
        /// <summary>Gets the property value.</summary>
        private static object getPropertyValue(object obj, Type type, string propertyName)
        {
            if (propertyName.Contains("."))//complex type nested
            {
                var temp = propertyName.Split(new char[] { '.' }, 2);
                var prop = getPropertyValue(obj, type, temp[0]);
                return getPropertyValue(prop, prop.GetType(), temp[1]);
            }
            else
                return type.GetProperty(propertyName).GetValue(obj, null);
        }

        public DataItem GetItem(int RecNo)
        {
            IEnumerable<object> query = this.Cast<object>();
            DataItem curItem = (DataItem)query.ElementAt(RecNo);

            return curItem;
        }

        public static void PrepareChunckedList<T>(T data, int maxRecordPerList, ref int counter
            , ref List<List<T>> MainDataList, ref List<T> DataChunkList)
        {
            if (counter < maxRecordPerList || maxRecordPerList == 0)
            {
                counter++;

                //Add item into chunk list
                if (DataChunkList == null) DataChunkList = new List<T>();
                DataChunkList.Add(data);
            }
            else
            {
                if (MainDataList == null) MainDataList = new List<List<T>>();
                MainDataList.Add(DataChunkList);

                //Reset
                DataChunkList = null;
                counter = 0;

                //Add item into next chunk list
                DataChunkList = new List<T>();
                DataChunkList.Add(data);
                counter++;
            }
        }
    }

    /// <Author>Dharati Metra</Author>
    /// <CreatedDate>30-aug-2016</CreatedDate>
    /// <summary>
    /// class for datacolumn
    /// </summary>
    [Serializable]
    public class DataColumn
    {
        private EnumDataColumnType _dataColumnType = EnumDataColumnType.Normal;
        public string TextField { get; set; }
        public string ValueField { get; set; }
        public EnumDataColumnType Type
        {
            get { return _dataColumnType; }
            set { _dataColumnType = value; }
        }
    }

    /// <Author>Dharati Metra</Author>
    /// <CreatedDate>30-aug-2016</CreatedDate>
    /// <summary>
    /// lookup values
    /// </summary>
    public enum EnumDataColumnType
    {
        Normal = 0,
        ColumnCheckbox = 1,
        ColumnAndHeaderCheckbox = 2,
        Image = 3,
        Html = 4,
        Amount = 5,
        DateTime = 6,
        LookUp = 7,
        Text = 8
    }
}
