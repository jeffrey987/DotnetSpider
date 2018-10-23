using DotnetSpider.Core;
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
	[TaskName("TestSpider")]
	public class TestSpider : EntitySpider
	{
		public TestSpider() :base()
		{		
		}

		protected override void OnInit(params string[] arguments)
		{
			Dictionary<string, object> headers = new Dictionary<string, object>();
			headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
			headers.Add("Accept-Encoding", "gzip, deflate, br");
			headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
			headers.Add("Cache-Control", "max-age=0");
			headers.Add("Cookie", "_uab_collina=153110491333754395238005; _umdata=ED82BDCEC1AA6EB92406A172A4D19CAE3A36DDEC073F37020607F5D942FD7196A7E4978E7EC5EC42CD43AD3E795C914CA987280943182C85F5CF575E3A194823; htVD_2132_widthauto=-1; Hm_lvt_46d556462595ed05e05f009cdafff31a=1539768453,1540259209; SL_GWPT_Show_Hide_tmp=1; SL_wptGlobTipTmp=1; htVD_2132_pc_size_c=0; htVD_2132_ulastactivity=1540262277%7C; htVD_2132_lastact=1540261763%09forum.php%09guide; Hm_lpvt_46d556462595ed05e05f009cdafff31a=1540262287");
			headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.67 Safari/537.36");
			AddHeaders("www.52pojie.cn", headers);
			AddRequests("https://www.52pojie.cn/forum.php?mod=guide&view=hot&page=1");
			EntityProcessor<BaiduSearchEntry> tempmodel = AddEntityType<BaiduSearchEntry>();
			var pipeine = new XmlEntityPipeline();
			pipeine.Title = "抓取吾爱破解最热文章";
			pipeine.Description = "抓取吾爱破解最热文章";
			pipeine.Link = "https://www.52pojie.cn/forum.php?mod=guide&view=hot&page=1";
			AddPipeline(pipeine);
			//AddEntityType<BaiduSearchEntry>().SetRequestExtractor(new AutoIncrementRequestExtractor("page=1"));
		}

		[Schema("baidu", "52POJIE")]
		[Entity(Expression = ".//tbody ")]
		public class BaiduSearchEntry : BaseEntity
		{

			//[Column]
			//[Field(Expression = "Keyword", Type = SelectorType.Enviroment)]
			//public string Keyword { get; set; }

			[Column]
			[Field(Expression = ".//a[@class='xst']")]
			[ReplaceFormatter(NewValue = "", OldValue = "<em>")]
			[ReplaceFormatter(NewValue = "", OldValue = "</em>")]
			public string title { get; set; }

			[Column]
			[Field(Expression = ".//a[@class='xst']/@href")]
			public string link { get; set; }

			[Column]
			[Field(Expression = ".//span[@class='xi1']")]
			//[ReplaceFormatter(NewValue = "", OldValue = "人参与")]
			public string guid { get; set; }


			[Column]
			[Field(Expression = ".//img[@alt='recommend']/@title")]
			public string recommend { get; set; }

			[Column]
			[Field(Expression = ".//img[@alt='heatlevel']/@title")]
			public string heatlevel { get; set; }
			[Column]
			[Field(Expression = ".//img[@alt='agree']/@title")]
			public string agree { get; set; }

			[Column]
			[Field(Expression = ".//em/span")]
			public string description { get; set; }
		}
	}
}
