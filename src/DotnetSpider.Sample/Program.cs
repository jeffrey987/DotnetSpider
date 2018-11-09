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
			TestSpider spider = new TestSpider();
			spider.Run();
		}

		//// 合成
		//public void Tts()
		//{
		//	// 设置APPID/AK/SK
		//	var APP_ID = "你的 App ID";
		//	var API_KEY = "你的 Api Key";
		//	var SECRET_KEY = "你的 Secret Key";

		//	var client = new Baidu.Aip.Speech.Tts(API_KEY, SECRET_KEY);
		//	client.Timeout = 60000;
		//	// 可选参数
		//	var option = new Dictionary<string, object>() { };
		//	var result = client.Synthesis("众里寻他千百度", option);

		//	if (result.ErrorCode == 0)  // 或 result.Success
		//	{
		//		File.WriteAllBytes("合成的语音文件本地存储地址.mp3", result.Data);
		//	}
		//}

	}

}
