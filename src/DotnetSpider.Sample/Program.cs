using DotnetSpider.Downloader;
using DotnetSpider.Sample.docs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
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
			#region MyRegion
			TestSpider spider = new TestSpider();
			Dictionary<string, object> headers = new Dictionary<string, object>();
			headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
			headers.Add("Accept-Encoding", "gzip, deflate, br");
			headers.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
			headers.Add("Cache-Control", "max-age=0");
			headers.Add("Cookie", "_uab_collina=153110491333754395238005; _umdata=ED82BDCEC1AA6EB92406A172A4D19CAE3A36DDEC073F37020607F5D942FD7196A7E4978E7EC5EC42CD43AD3E795C914CA987280943182C85F5CF575E3A194823; htVD_2132_widthauto=-1; Hm_lvt_46d556462595ed05e05f009cdafff31a=1539768453,1540259209; SL_GWPT_Show_Hide_tmp=1; SL_wptGlobTipTmp=1; htVD_2132_pc_size_c=0; htVD_2132_ulastactivity=1540262277%7C; htVD_2132_lastact=1540261763%09forum.php%09guide; Hm_lpvt_46d556462595ed05e05f009cdafff31a=1540262287");
			headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.67 Safari/537.36");
			spider.AddHeaders("www.52pojie.cn", headers);
			spider.Run(); 
			#endregion
			//AutoIncrementTargetRequestExtractorrSpider.Run();
			//AfterDownloadCompleteHandlerSpider.Run();
		}



	}
}
