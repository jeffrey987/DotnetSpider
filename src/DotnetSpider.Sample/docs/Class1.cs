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

namespace DotnetSpider.Sample.docs
{
	public class Class1 : EntitySpider
	{
		public Class1() : base()
		{

		}
		protected override void OnInit(params string[] arguments)
		{
			Dictionary<string, object> headers = new Dictionary<string, object>();
			headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
			headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.67 Safari/537.36");
			headers.Add("Request Method", "POST");
			AddHeaders("jzsc.mohurd.gov.cn", headers);
			AddRequests("http://jzsc.mohurd.gov.cn/dataservice/query/comp/list?$pg=1");
			EntityProcessor<class1Entity> tempmodel = AddEntityType<class1Entity>();
			var pipeine = new ExcelEntityPipeline();
			AddPipeline(pipeine);
	
		}

	}

	public class class1Entity : BaseEntity
	{

	}
}
