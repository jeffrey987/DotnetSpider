using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Wdkk.Excel
{
	public class ExcelHelper
	{
		private IWorkbook _workbook;
		private HSSFSheet _sheet;
		private DataTable _dt;
		private int[] _colWidths;
		private string[] _fileds;
		private string _sheetName;
		private string _headerName;
		private string _tableTitle;
		private int _isTitle;
		private int _isOrderby = 0;

		public ExcelHelper()
		{
			_workbook = new HSSFWorkbook();
		}

		public List<T> ExcelToEntity<T>(Stream stream, string fields, string fileExt, int HeaderRowIndex = 0) where T : class, new()
		{
			var tmpFields = "";
			if (string.IsNullOrWhiteSpace(fields))
			{
				var type = typeof(T);
				//string name = string.Empty;
				PropertyInfo[] properties = type.GetProperties();
				foreach (var pi in properties)
				{
					tmpFields += pi.Name + ";";
					//name = pi.Name;                   
				}
				tmpFields = tmpFields.Trim(';');
			}
			else
				tmpFields = fields;
			var table = this.ExcelToDataTable(stream, tmpFields, fileExt);
			if (table == null)
				return null;
			return JsonHelper.DataTableToList<T>(table);
		}
		/// <summary>
		/// excel转table
		/// </summary>
		/// <param name="stream">Excel流数据</param>
		/// <param name="fields">字段值，分号隔开</param>
		/// <param name="HeaderRowIndex">标题开始行，默认0</param>
		/// <returns></returns>
		public DataTable ExcelToDataTable(Stream stream, string fields, string fileExt, int HeaderRowIndex = 0)
		{
			if (fileExt != "xls" && fileExt != "xlsx")
				return null;
			IWorkbook workbook;
			if (fileExt == "xls")
				workbook = new HSSFWorkbook(stream);
			else
				workbook = new XSSFWorkbook(stream);
			ISheet sheet = workbook.GetSheetAt(0);//第一个工作表
			DataTable table = new DataTable();
			var fieldArray = fields.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
			try
			{
				if (fieldArray.Count() < 1)
					return null;
				foreach (var item in fieldArray)
					table.Columns.Add(new DataColumn(item));
				for (int i = (HeaderRowIndex + 1); i <= sheet.LastRowNum; i++)
				{
					IRow row = sheet.GetRow(i);
					DataRow dataRow = table.NewRow();
					for (int j = 0; j < fieldArray.Count(); j++)
					{
						ICell cell = row.GetCell(j);
						if (cell == null)
							dataRow[j] = null;
						else
						{
							switch (cell.CellType)
							{
								case CellType.Blank:
									dataRow[j] = null;
									break;
								case CellType.Boolean:
									dataRow[j] = cell.BooleanCellValue;
									break;
								case CellType.Numeric:
									dataRow[j] = cell.ToString();
									break;
								case CellType.String:
									dataRow[j] = cell.StringCellValue;
									break;
								case CellType.Error:
									dataRow[j] = cell.ErrorCellValue;
									break;
								case CellType.Formula:
								default:
									dataRow[j] = "=" + cell.CellFormula;
									break;
							}

						}
					}
					table.Rows.Add(dataRow);
				}
			}
			catch (Exception ex)
			{
				table.Clear();
				table.Columns.Clear();
				throw ex;
			}
			finally
			{
				workbook = null;
				sheet = null;
			}
			#region 清除最后的空行
			for (int i = table.Rows.Count - 1; i > 0; i--)
			{
				bool isnull = true;
				for (int j = 0; j < table.Columns.Count; j++)
				{
					if (table.Rows[i][j] != null)
					{
						if (table.Rows[i][j].ToString() != "")
						{
							isnull = false;
							break;
						}
					}
				}
				if (isnull)
				{
					table.Rows[i].Delete();
				}
			}
			#endregion

			return table;
		}

		#region 内部方法
		private void Init(DataTable dataSource, string filed, string sheetName, string headerName, string tableTitle = null, int isOrderby = 0)
		{
			if (!string.IsNullOrEmpty(filed))
			{
				this._fileds = filed.ToUpper().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

				// 移除多余数据列
				for (int i = dataSource.Columns.Count - 1; i >= 0; i--)
				{
					DataColumn dc = dataSource.Columns[i];
					if (!this._fileds.Contains(dataSource.Columns[i].Caption.ToUpper()))
					{
						dataSource.Columns.Remove(dataSource.Columns[i]);
					}
				}

				// 列索引
				int colIndex = 0;
				//// 循环排序
				for (int i = 0; i < dataSource.Columns.Count; i++)
				{
					// 获取索引
					colIndex = GetColIndex(dataSource.Columns[i].Caption.ToUpper());
					// 设置下标
					dataSource.Columns[i].SetOrdinal(colIndex);
				}
			}
			else
			{
				this._fileds = new string[dataSource.Columns.Count];
				for (int i = 0; i < dataSource.Columns.Count; i++)
				{
					this._fileds[i] = dataSource.Columns[i].ColumnName;
				}
			}
			this._dt = dataSource.Copy();

			if (!string.IsNullOrEmpty(sheetName))
				this._sheetName = sheetName;

			if (!string.IsNullOrEmpty(headerName))
				this._headerName = headerName;
			else
				this._headerName = string.Join("#", this._fileds);

			if (!string.IsNullOrEmpty(tableTitle))
			{
				this._tableTitle = tableTitle;
				this._isTitle = 1;
			}

			// 取得数据列宽 数据列宽可以和表头列宽比较，采取最长宽度  
			_colWidths = new int[this._dt.Columns.Count];
			foreach (DataColumn item in this._dt.Columns)
				_colWidths[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
			// 循环比较最大宽度
			for (int i = 0; i < this._dt.Rows.Count; i++)
			{
				for (int j = 0; j < this._dt.Columns.Count; j++)
				{
					int intTemp = Encoding.GetEncoding(936).GetBytes(this._dt.Rows[i][j].ToString()).Length;
					if (intTemp > _colWidths[j])
						_colWidths[j] = intTemp;
				}
			}

			if (isOrderby > 0)
			{
				this._isOrderby = isOrderby;
				this._headerName = "序号#" + this._headerName;
			}
		}

		private int GetColIndex(string colName)
		{
			for (int i = 0; i < this._fileds.Length; i++)
			{
				if (colName == this._fileds[i])
					return i;
			}
			return 0;
		}

		private void Export()
		{

			// 声明 Row 对象
			IRow _row;
			// 声明 Cell 对象
			ICell _cell;
			// 总列数
			int cols = 0;
			// 总行数
			int rows = 0;
			// 行数计数器
			int rowIndex = 0;
			// 单元格值
			string drValue = null;
			if (!string.IsNullOrEmpty(_sheetName))
				_sheet = (HSSFSheet)_workbook.CreateSheet(_sheetName);
			else
				_sheet = (HSSFSheet)_workbook.CreateSheet();
			// 初始化
			rowIndex = 0;
			// 获取总行数
			rows = GetRowCount(_headerName);
			// 获取总列数
			cols = GetColCount(_headerName);
			// 循环行数
			foreach (DataRow row in _dt.Rows)
			{
				#region 新建表，填充表头，填充列头，样式
				if (rowIndex == 65535 || rowIndex == 0)
				{
					if (rowIndex != 0)
						_sheet = (HSSFSheet)_workbook.CreateSheet();

					// 构建行

					for (int i = 0; i < rows + _isTitle; i++)
					{
						_row = _sheet.GetRow(i);
						// 创建行
						if (_row == null)
							_row = _sheet.CreateRow(i);
						for (int j = 0; j < cols; j++)
							_row.CreateCell(j).CellStyle = bodyStyle;
					}

					// 如果存在表标题
					if (_isTitle > 0)
					{
						// 获取行
						_row = _sheet.GetRow(0);
						// 合并单元格
						CellRangeAddress region = new CellRangeAddress(0, 0, 0, (cols - 1));
						_sheet.AddMergedRegion(region);
						// 填充值
						_row.CreateCell(0).SetCellValue(_tableTitle);
						// 设置样式
						_row.GetCell(0).CellStyle = titleStyle;
						// 设置行高
						_row.HeightInPoints = 20;
					}

					// 取得上一个实体
					NPOIHeader lastRow = null;
					IList<NPOIHeader> hList = GetHeaders(_headerName, rows, _isTitle);
					// 创建表头
					foreach (NPOIHeader m in hList)
					{
						var data = hList.Where(c => c.firstRow == m.firstRow && c.lastCol == m.firstCol - 1);
						if (data.Count() > 0)
						{
							lastRow = data.First();
							if (m.headerName == lastRow.headerName)
								m.firstCol = lastRow.firstCol;
						}

						// 获取行
						_row = _sheet.GetRow(m.firstRow);
						// 合并单元格
						CellRangeAddress region = new CellRangeAddress(m.firstRow, m.lastRow, m.firstCol, m.lastCol);
						_sheet.AddMergedRegion(region);
						// 填充值
						_row.CreateCell(m.firstCol).SetCellValue(m.headerName.Trim());
					}

					// 填充表头样式
					for (int i = 0; i < rows + _isTitle; i++)
					{
						_row = _sheet.GetRow(i);
						for (int j = 0; j < cols; j++)
						{
							_row.GetCell(j).CellStyle = bodyStyle;
							//设置列宽
							_sheet.SetColumnWidth(j, (_colWidths[j] + 1) * 256);
						}
					}

					rowIndex = (rows + _isTitle);
				}
				#endregion

				#region 填充内容
				// 构建列
				_row = _sheet.CreateRow(rowIndex);
				foreach (DataColumn column in _dt.Columns)
				{
					// 添加序号列
					if (1 == _isOrderby && column.Ordinal == 0)
					{
						_cell = _row.CreateCell(0);
						_cell.SetCellValue(rowIndex - rows);
						_cell.CellStyle = bodyStyle;
					}

					// 创建列
					_cell = _row.CreateCell(column.Ordinal + _isOrderby);
					// 获取值
					drValue = row[column.ColumnName].ToString();
					_cell.SetCellValue(drValue);
					#region 删除内容
					//switch (column.DataType.ToString())
					//{
					//    case "System.String"://字符串类型
					//        _cell.SetCellValue(drValue);
					//        _cell.CellStyle = bodyStyle;
					//        break;
					//    case "System.DateTime"://日期类型
					//        DateTime dateV;
					//        DateTime.TryParse(drValue, out dateV);
					//        _cell.SetCellValue(dateV);

					//        _cell.CellStyle = dateStyle;//格式化显示
					//        break;
					//    case "System.Boolean"://布尔型
					//        bool boolV = false;
					//        bool.TryParse(drValue, out boolV);
					//        _cell.SetCellValue(boolV);
					//        _cell.CellStyle = bodyStyle;
					//        break;
					//    case "System.Int16"://整型
					//    case "System.Int32":
					//    case "System.Int64":
					//    case "System.Byte":
					//        int intV = 0;
					//        int.TryParse(drValue, out intV);
					//        _cell.SetCellValue(intV);
					//        _cell.CellStyle = bodyRightStyle;
					//        break;
					//    case "System.Decimal"://浮点型
					//    case "System.Double":
					//        double doubV = 0;
					//        double.TryParse(drValue, out doubV);
					//        _cell.SetCellValue(doubV);
					//        _cell.CellStyle = bodyRightStyle;
					//        break;
					//    case "System.DBNull"://空值处理
					//        _cell.SetCellValue("");
					//        break;
					//    default:
					//        _cell.SetCellValue("");
					//        break;
					//}
					#endregion
				}
				#endregion

				rowIndex++;
			}
		}

		private int GetRowCount(string newHeaders)
		{
			string[] ColumnNames = newHeaders.Split(new char[] { '@' });
			int Count = 0;
			if (ColumnNames.Length <= 1)
				ColumnNames = newHeaders.Split(new char[] { '#' });
			foreach (string name in ColumnNames)
			{
				int TempCount = name.Split(new char[] { ' ' }).Length;
				if (TempCount > Count)
					Count = TempCount;
			}
			return Count;
		}

		private int GetColCount(string newHeaders)
		{
			string[] ColumnNames = newHeaders.Split(new char[] { '@' });
			int Count = 0;
			if (ColumnNames.Length <= 1)
				ColumnNames = newHeaders.Split(new char[] { '#' });
			Count = ColumnNames.Length;
			foreach (string name in ColumnNames)
			{
				int TempCount = name.Split(new char[] { ',' }).Length;
				if (TempCount > 1)
					Count += TempCount - 1;
			}
			return Count;
		}

		#region 单元格样式
		/// <summary>
		/// 数据单元格样式
		/// </summary>
		private ICellStyle bodyStyle
		{
			get
			{
				ICellStyle style = _workbook.CreateCellStyle();
				style.Alignment = HorizontalAlignment.Center; //居中
				style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
				style.WrapText = true;//自动换行
									  // 边框
				style.BorderBottom = BorderStyle.Thin;
				style.BorderLeft = BorderStyle.Thin;
				style.BorderRight = BorderStyle.Thin;
				style.BorderTop = BorderStyle.Thin;
				// 字体
				IFont font = _workbook.CreateFont();
				font.FontHeightInPoints = 10;
				font.FontName = "宋体";
				style.SetFont(font);

				return style;
			}
		}

		/// <summary>
		/// 数据单元格样式
		/// </summary>
		private ICellStyle bodyRightStyle
		{
			get
			{
				ICellStyle style = _workbook.CreateCellStyle();
				style.Alignment = HorizontalAlignment.Right; //居中
				style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
				style.WrapText = true;//自动换行
									  // 边框
				style.BorderBottom = BorderStyle.Thin;
				style.BorderLeft = BorderStyle.Thin;
				style.BorderRight = BorderStyle.Thin;
				style.BorderTop = BorderStyle.Thin;
				// 字体
				IFont font = _workbook.CreateFont();
				font.FontHeightInPoints = 10;
				font.FontName = "宋体";
				style.SetFont(font);

				return style;
			}
		}

		/// <summary>
		/// 标题单元格样式
		/// </summary>
		private ICellStyle titleStyle
		{
			get
			{
				ICellStyle style = _workbook.CreateCellStyle();
				style.Alignment = HorizontalAlignment.Center; //居中
				style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
				style.WrapText = true;//自动换行 

				IFont font = _workbook.CreateFont();
				font.FontHeightInPoints = 14;
				font.FontName = "宋体";
				font.Boldweight = (short)FontBoldWeight.Bold;
				style.SetFont(font);

				return style;
			}
		}

		/// <summary>
		/// 日期单元格样式
		/// </summary>
		private ICellStyle dateStyle
		{
			get
			{
				ICellStyle style = _workbook.CreateCellStyle();
				style.Alignment = HorizontalAlignment.Center; //居中
				style.VerticalAlignment = VerticalAlignment.Center;//垂直居中 
				style.WrapText = true;//自动换行
									  // 边框
				style.BorderBottom = BorderStyle.Thin;
				style.BorderLeft = BorderStyle.Thin;
				style.BorderRight = BorderStyle.Thin;
				style.BorderTop = BorderStyle.Thin;
				// 字体
				IFont font = _workbook.CreateFont();
				font.FontHeightInPoints = 10;
				font.FontName = "宋体";
				style.SetFont(font);

				IDataFormat format = _workbook.CreateDataFormat();
				style.DataFormat = format.GetFormat("yyyy-MM-dd");
				return style;
			}
		}
		#endregion

		private IList<NPOIHeader> GetHeaders(string header, int rows, int addRows)
		{
			// 临时表头数组
			string[] tempHeader;
			string[] tempHeader2;
			// 所跨列数
			int colSpan = 0;
			// 所跨行数
			int rowSpan = 0;
			// 单元格对象
			NPOIHeader model = null;
			// 行数计数器
			int rowIndex = 0;
			// 列数计数器
			int colIndex = 0;
			// 
			IList<NPOIHeader> list = new List<NPOIHeader>();
			// 初步解析
			string[] headers = header.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
			// 表头遍历
			for (int i = 0; i < headers.Length; i++)
			{
				// 行数计数器清零
				rowIndex = 0;
				// 列数计数器清零
				colIndex = 0;
				// 获取所跨行数
				rowSpan = GetRowSpan(headers[i], rows);
				// 获取所跨列数
				colSpan = GetColSpan(headers[i]);

				// 如果所跨行数与总行数相等，则不考虑是否合并单元格问题
				if (rows == rowSpan)
				{
					colIndex = GetMaxCol(list);
					model = new NPOIHeader(headers[i],
						addRows,
						(rowSpan - 1 + addRows),
						colIndex,
						(colSpan - 1 + colIndex),
						addRows);
					list.Add(model);
					rowIndex += (rowSpan - 1) + addRows;
				}
				else
				{
					// 列索引
					colIndex = GetMaxCol(list);
					// 如果所跨行数不相等，则考虑是否包含多行
					tempHeader = headers[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
					for (int j = 0; j < tempHeader.Length; j++)
					{

						// 如果总行数=数组长度
						if (1 == GetColSpan(tempHeader[j]))
						{
							if (j == tempHeader.Length - 1 && tempHeader.Length < rows)
							{
								model = new NPOIHeader(tempHeader[j],
									(j + addRows),
									(j + addRows) + (rows - tempHeader.Length),
									colIndex,
									(colIndex + colSpan - 1),
									addRows);
								list.Add(model);
							}
							else
							{
								model = new NPOIHeader(tempHeader[j],
										(j + addRows),
										(j + addRows),
										colIndex,
										(colIndex + colSpan - 1),
										addRows);
								list.Add(model);
							}
						}
						else
						{
							// 如果所跨列数不相等，则考虑是否包含多列
							tempHeader2 = tempHeader[j].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
							for (int m = 0; m < tempHeader2.Length; m++)
							{
								// 列索引
								colIndex = GetMaxCol(list) - colSpan + m;
								if (j == tempHeader.Length - 1 && tempHeader.Length < rows)
								{
									model = new NPOIHeader(tempHeader2[m],
										(j + addRows),
										(j + addRows) + (rows - tempHeader.Length),
										colIndex,
										colIndex,
										addRows);
									list.Add(model);
								}
								else
								{
									model = new NPOIHeader(tempHeader2[m],
											(j + addRows),
											(j + addRows),
											colIndex,
											colIndex,
											addRows);
									list.Add(model);
								}
							}
						}
						rowIndex += j + addRows;
					}
				}
			}
			return list;
		}

		private int GetMaxCol(IList<NPOIHeader> list)
		{
			int maxCol = 0;
			if (list.Count > 0)
			{
				foreach (NPOIHeader model in list)
				{
					if (maxCol < model.lastCol)
						maxCol = model.lastCol;
				}
				maxCol += 1;
			}

			return maxCol;
		}
		private int GetColSpan(string newHeaders)
		{
			return newHeaders.Split(',').Count();
		}

		private int GetRowSpan(string newHeaders, int rows)
		{
			int Count = newHeaders.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Length;
			// 如果总行数与当前表头所拥有行数相等
			if (rows == Count)
				Count = 1;
			else if (Count < rows)
				Count = 1 + (rows - Count);
			else
				throw new Exception("表头格式不正确！");
			return Count;
		}


		#endregion
	}
}
