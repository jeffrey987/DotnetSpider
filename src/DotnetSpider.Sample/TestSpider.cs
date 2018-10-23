﻿using DotnetSpider.Core;
using DotnetSpider.Core.Processor.RequestExtractor;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Pipeline;
using DotnetSpider.Extension.Processor;
using DotnetSpider.Extraction.Model.Attribute;
using DotnetSpider.Extraction.Model.Formatter;
using System;


namespace DotnetSpider.Sample
{
	[TaskName("TestSpider")]
	public class TestSpider : EntitySpider
	{
		public TestSpider()
		{
		}

		protected override void OnInit(params string[] arguments)
		{
			AddRequests("https://www.52pojie.cn/forum.php?mod=guide&view=hot&page=1");
			EntityProcessor<BaiduSearchEntry> tempmodel = AddEntityType<BaiduSearchEntry>();
			AddPipeline(new XmlFileEntityPipeline());
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
