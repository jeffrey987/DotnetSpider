using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DotnetSpider.Core.Pipeline;
using DotnetSpider.Sample;
using Microsoft.AspNetCore.Mvc;


namespace DotnetSpider.Broker.Controllers
{
	public class PoJieController : Controller
	{
		public IActionResult Index()
		{

			TestSpider spider = new TestSpider();
			HttpResponseMessage message;
			spider.Run();
			foreach (IPipeline item in spider.Pipelines)
			{
				message = item.Httpresponse;
			}


			return View();
		}
	}
}