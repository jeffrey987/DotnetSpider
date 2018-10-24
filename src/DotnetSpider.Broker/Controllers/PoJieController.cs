using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetSpider.Sample;
using Microsoft.AspNetCore.Mvc;


namespace DotnetSpider.Broker.Controllers
{
	public class PoJieController : Controller
	{
		public IActionResult Index()
		{

			TestSpider spider = new TestSpider();

			spider.Run();
			
			return View();
		}
	}
}