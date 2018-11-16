using DotnetSpider.Core.Pipeline;
using DotnetSpider.Sample;
using Microsoft.AspNetCore.Mvc;


namespace DotnetSpider.Broker.Controllers
{
	public class PoJieController : Controller
	{
		public IActionResult Index()
		{

			//TestSpider spider = new TestSpider();
			//System.Xml.Linq.XDocument message = new System.Xml.Linq.XDocument();
			//spider.Run();
			//foreach (IPipeline item in spider.Pipelines)
			//{
			//	message = item.ResponseMessage;
			//}
			//Response.ContentType = "application/xml";
			////Request.ContentType
			//return Content(message.ToString());
			return View();
		}
		
	}
}