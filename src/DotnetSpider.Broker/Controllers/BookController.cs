
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using DotnetSpider.Core;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Pipeline;
using DotnetSpider.Extension.Processor;
using DotnetSpider.Extraction.Model.Attribute;
using DotnetSpider.Extraction.Model.Formatter;
using Microsoft.AspNetCore.Mvc;

namespace DotnetSpider.Broker.Controllers
{
	public class BookController : Controller
	{
		public IActionResult Index()
		{
			BookSpider spider = new BookSpider();

			spider.StrCookie = GetbookcookieAsync().Result;
			System.Xml.Linq.XDocument message = new System.Xml.Linq.XDocument();
			spider.Run();
			foreach (IPipeline item in spider.Pipelines)
			{
				message = item.ResponseMessage;
			}
			if (message == null)
			{
				return Redirect(Url.Action("Error", "Home"));
			}
			Response.ContentType = "application/xml";
			return Content(message.ToString());
		}

		public async System.Threading.Tasks.Task<string> GetbookcookieAsync()
		{
			string postString = "rdid=DC493495X&rdPasswd=b570e60c3677d253fd8c36e27a0f4143&returnUrl=&password=";
			string url = "http://interweb.xmlib.net/opac/reader/doLogin";                                                                                               // 创建请求
			HttpClient httpClient = new HttpClient();
			httpClient.Timeout = TimeSpan.FromSeconds(20);

			string Allurl = url + postString; ;
			HttpContent httpcontent = null;
			HttpResponseMessage response = await httpClient.PostAsync(url, httpcontent);
			// 请求失败
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
			//这句话是关键点
			var cookies = response.Headers.GetValues("Set-Cookie").ToList();
			// 解析请求结果
			//string content = await response.Content.ReadAsStringAsync();
			Regex regex = new Regex(@"JSESSIONID=[-A-Za-z0-9]*;");
			string result = regex.Match(cookies.FirstOrDefault()).Value;
			return result;
		}
		[TaskName("BookSpider2")]
		public class BookSpider : EntitySpider
		{
			public string StrCookie = "";
			protected override void OnInit(params string[] arguments)
			{
				Dictionary<string, object> headers = new Dictionary<string, object>();
				Downloader.AddCookies(StrCookie, "interweb.xmlib.net");
				headers.Add("Cookie", StrCookie);
				AddHeaders("interweb.xmlib.net", headers);
				AddRequests("http://interweb.xmlib.net/opac/reader/space");

				var pipeine = new XmlEntityPipeline();
				//pipeine.Title = "厦门图书馆";
				//pipeine.Description = "厦门图书馆";
				//pipeine.Link = "http://interweb.xmlib.net/opac/reader/space";
				AddPipeline(pipeine);
				AddEntityType<BookEntry>();
			}

			[Schema("baidu", "xmlib")]
			[Entity(Expression = ".//tbody/tr")]
			public class BookEntry : BaseEntity
			{
				[Column]
				[Field(Expression = ".//font[@class='space_font']")]
				public string titles { get; set; }

				[Column]
				[Field(Expression = ".//font")]
				public string titles2 { get; set; }

				[Column]
				[Field(Expression = ".//h3")]
				public string name { get; set; }
			}
		}

	}
}