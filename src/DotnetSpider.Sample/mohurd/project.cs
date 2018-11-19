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
			List<string> JsxmTypeList = new List<string>() { "房屋建筑工程", "市政工程", "其他" };
			foreach (var item in provinces.Take(30))
			{
				foreach (var typeitem in JsxmTypeList)
				{
					AddRequests(string.Format("http://jzsc.mohurd.gov.cn/dataservice/query/project/list?$pg=1&jsxm_region_id={0}&jsxm_type={1}", item, typeitem));
					EntityProcessor<BaiduSearchEntry2> tempmodel = AddEntityType<BaiduSearchEntry2>();
				}
			}
			AddPipeline(new JsonFileEntityPipeline());
			//AddEntityType<BaiduSearchEntry2>().SetRequestExtractor(new AutoIncrementRequestExtractor(@"\$pg=1", 1)).SetLastPageChecker(new TestLastPageChecker());  
		}

		//?和id不能使用
		[Schema("project", "项目数据")]
		[Entity(Expression = "html")]
		public class BaiduSearchEntry2 : BaseEntity
		{
			[Column]
			[Field(Expression = "total\":\\d*", Type = SelectorType.Regex)]
			public string Category { get; set; }

			[Column]
			[Field(Expression = "jsxm_region_id=\\d*", Type = SelectorType.Regex)]
			public string RegionID { get; set; }
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
			//“E:\working\DotnetSpider\src\DotnetSpider.Sample\bin\
			Stream stream = new FileStream(".../.../.../excel/ProvincesCode.xlsx", FileMode.Open, FileAccess.Read);
			var table = helper.ExcelToDataTable(stream, "id;name;address", "xlsx");
			foreach (DataRow item in table.Rows)
			{
				list.Add(item["id"].ToString());
			}

			return list;
		}


	}
}
