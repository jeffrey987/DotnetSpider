using DotnetSpider.Core;
using DotnetSpider.Core.Processor;
using DotnetSpider.Core.Processor.RequestExtractor;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Pipeline;
using DotnetSpider.Extension.Processor;
using DotnetSpider.Extraction.Model.Attribute;
using DotnetSpider.Extraction.Model.Formatter;
using System;
using System.Collections.Generic;

namespace DotnetSpider.Sample
{
	[TaskName("TestSpider2")]
	public class TestSpider2 : EntitySpider
	{
		public TestSpider2() : base()
		{
		}

		protected override void OnInit(params string[] arguments)
		{
			Dictionary<string, object> headers = new Dictionary<string, object>();
			headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
			headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.67 Safari/537.36");
			headers.Add("Request Method", "POST");
			AddHeaders("jzsc.mohurd.gov.cn", headers);

			string jsxm_region_id = "";
			AddRequests("http://jzsc.mohurd.gov.cn/dataservice/query/project/list?$pg=1&$jsxm_region_id=" + jsxm_region_id);
			EntityProcessor<BaiduSearchEntry> tempmodel = AddEntityType<BaiduSearchEntry>();
			var pipeine = new ExcelEntityPipeline();

			AddPipeline(pipeine);
			AddEntityType<BaiduSearchEntry>().SetRequestExtractor(new AutoIncrementRequestExtractor(@"\$pg=1", 1)).SetLastPageChecker(new TestLastPageChecker()); ;
		}

		[Schema("baidu", "mohurd")]
		[Entity(Expression = ".//tr")]
		public class BaiduSearchEntry : BaseEntity
		{

			[Column]
			[Field(Expression = ".//td[@data-header='项目名称']/a")]
			public string title { get; set; }
			[Column]
			[Field(Expression = ".//td[@data-header='项目名称']/a/@href")]
			public string link { get; set; }
			[Column]
			[Field(Expression = ".//td[@data-header='项目编码']")]
			public string code { get; set; }
			[Column]
			[Field(Expression = ".//td[@data-header='项目名称']/span")]
			public string address { get; set; }




			[Column]
			[Field(Expression = ".//td[@data-header='项目类别']")]
			public string buildtype { get; set; }


			[Column]
			[Field(Expression = ".//td[@data-header='建设单位']")]
			public string description { get; set; }

			[Column]
			[Field(Expression = ".//td[@data-header='序号']")]
			public int Id { get; set; }
		}


		private class TestLastPageChecker : ILastPageChecker
		{
			public bool IsLastPage(Page page)
			{
				return page.Request.Url.Contains("$pg=30");
			}
		}
	}
}
