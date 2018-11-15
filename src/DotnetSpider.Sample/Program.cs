using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DotnetSpider.Sample
{
	class Program
	{
		static void Main(string[] args)
		{
#if NETCOREAPP
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#else
			ThreadPool.SetMinThreads(256, 256);
#endif



			//HttpClientDownloader downloader = new HttpClientDownloader();
			//var response = downloader.Download(new Request("http://www.163.com")
			//{
			//	Method = HttpMethod.Post,
			//	Content = JsonConvert.SerializeObject(new { a = "bb" }),
			//	ContentType = "application/json"
			//});

			//AutoIncrementTargetRequestExtractorrSpider.Run();
			//AfterDownloadCompleteHandlerSpider.Run();
			//TestSpider spider = new TestSpider();
			//spider.Run();
		}



	}

}
