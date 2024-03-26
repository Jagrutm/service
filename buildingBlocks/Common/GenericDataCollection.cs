namespace CredECard.Common.BusinessService
{
	using System;

	[Serializable()]
	public class GenericDataCollection : DataCollection
	{
        /// <summary>
        /// Adds item to list
        /// </summary>
        /// <param name="typeToAdd"></param>
		public void Add(DataItem typeToAdd)
		{
			List.Add(typeToAdd);
		}

        /// <summary>
        /// Removes item at given index
        /// </summary>
        /// <param name="index">int</param>
		public void Remove(int index)
		{
			if (index > this.Count - 1 || index < 0)
			{
				throw new IndexOutOfRangeException(CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
			}
			else
			{
				List.RemoveAt(index); 
			}
		}

        /// <summary>
        /// Removes item from list
        /// </summary>
        /// <param name="itemToRemove">DataItem</param>
		public void Remove(DataItem itemToRemove)
		{
			List.Remove(itemToRemove);
		}

        /// <summary>
        /// Gets the item at given index and if index our of bounds then throws exception IndexOutOfRangeException
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
		public DataItem Item(int index)
		{
			if (index > this.Count - 1 || index < 0)
			{
				throw new IndexOutOfRangeException(CommonMessage.GetMessage(EnumErrorConstants.SPECIFIED_INDEX_NOT_FOUND));
			}
			else
			{
				return (DataItem) List[index];
			}
		}
	}
}
