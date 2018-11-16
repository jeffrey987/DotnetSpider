using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Wdkk.Excel
{
	internal class JsonHelper
	{
		public static List<T> DataTableToList<T>(DataTable dt) where T : class, new()
		{
			List<T> ts = new List<T>();
			string tmpName = string.Empty;
			foreach (DataRow dr in dt.Rows)
			{
				T t = new T();
				PropertyInfo[] propertys = t.GetType().GetProperties();
				foreach (PropertyInfo pi in propertys)
				{
					tmpName = pi.Name;
					if (dt.Columns.Contains(tmpName))
					{
						object value = dr[tmpName];
						if (value != null & value != DBNull.Value)
						{
							pi.SetValue(t, value, null);
						}
					}
				}
				ts.Add(t);
			}
			return ts;
		}
	}
}