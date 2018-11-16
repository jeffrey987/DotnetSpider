using DotnetSpider.Core;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Processor.RequestExtractor;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Pipeline;
using DotnetSpider.Extension.Processor;
using DotnetSpider.Extraction;
using DotnetSpider.Extraction.Model.Attribute;
using DotnetSpider.Extraction.Model.Formatter;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using Wdkk.Excel;

namespace DotnetSpider.Sample.mohurd
{
	[TaskName("project")]
	public class project : EntitySpider
	{
		public project() : base()
		{
		}

		protected override void OnInit(params string[] arguments)
		{
			Dictionary<string, object> headers = new Dictionary<string, object>();
			headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
			headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.67 Safari/537.36");
			headers.Add("Request Method", "GET");
			AddHeaders("jzsc.mohurd.gov.cn", headers);
			List<string> provinces = GetProvinces();
			foreach (var item in provinces.Take(5))
			{
				AddRequests("http://jzsc.mohurd.gov.cn/dataservice/query/project/list?$pg=1&jsxm_region_id=" + item);
				EntityProcessor<BaiduSearchEntry2> tempmodel = AddEntityType<BaiduSearchEntry2>();
			}
				//AddEntityType<BaiduSearchEntry2>().SetRequestExtractor(new AutoIncrementRequestExtractor(@"\$pg=1", 1)).SetLastPageChecker(new TestLastPageChecker());  
				AddPipeline(new JsonFileEntityPipeline());
		}



		[Schema("project", "项目数据")]
		[Entity(Expression = "html")]
		public class BaiduSearchEntry2 : BaseEntity
		{
			[Column]
			[Field(Expression = "total\":\\d*", Type = SelectorType.Regex)]
			public string Category { get; set; }

			
		}


		private class TestLastPageChecker : ILastPageChecker
		{
			public bool IsLastPage(Page page)
			{
				return page.Request.Url.Contains("$pg=1");
			}
		}
		private List<string> GetProvinces()
		{
			List<string> list = new List<string>();
			Wdkk.Excel.ExcelHelper helper = new ExcelHelper();
			Stream stream = new FileStream("D:/github/DotnetSpider/src/DotnetSpider.Sample/excel/ProvincesCode.xlsx", FileMode.Open, FileAccess.Read);
			var table = helper.ExcelToDataTable(stream, "id;name;address", "xlsx");
			foreach (DataRow item in table.Rows)
			{
				list.Add(item["id"].ToString());
			}

			return list;
		}


	}
}
