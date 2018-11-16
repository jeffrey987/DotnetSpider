using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wdkk.Excel
{
    internal class NPOIHeader
    {
        /// <summary>
        /// 表头
        /// </summary>
        public string headerName { get; set; }
        /// <summary>
        /// 起始行
        /// </summary>
        public int firstRow { get; set; }
        /// <summary>
        /// 结束行
        /// </summary>
        public int lastRow { get; set; }
        /// <summary>
        /// 起始列
        /// </summary>
        public int firstCol { get; set; }
        /// <summary>
        /// 结束列
        /// </summary>
        public int lastCol { get; set; }
        /// <summary>
        /// 是否跨行
        /// </summary>
        public int isRowSpan { get; private set; }
        /// <summary>
        /// 是否跨列
        /// </summary>
        public int isColSpan { get; private set; }
        /// <summary>
        /// 外加行
        /// </summary>
        public int rows { get; set; }

        public NPOIHeader() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="headerName">表头</param>
        /// <param name="firstRow">起始行</param>
        /// <param name="lastRow">结束行</param>
        /// <param name="firstCol">起始列</param>
        /// <param name="lastCol">结束列</param>
        /// <param name="rows">外加行</param>
        /// <param name="cols">外加列</param>
        public NPOIHeader(string headerName, int firstRow, int lastRow, int firstCol, int lastCol, int rows = 0)
        {
            this.headerName = headerName;
            this.firstRow = firstRow;
            this.lastRow = lastRow;
            this.firstCol = firstCol;
            this.lastCol = lastCol;
            // 是否跨行判断
            if (firstRow != lastRow)
                isRowSpan = 1;
            if (firstCol != lastCol)
                isColSpan = 1;

            this.rows = rows;
        }
    }
}
