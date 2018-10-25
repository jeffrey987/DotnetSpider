using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace DotnetSpider.Core.Pipeline
{
	public class SilentPipeline : IPipeline
	{
		public ILogger Logger { get; set; }
		public HttpResponseMessage Httpresponse { get; set; }

		public void Dispose()
		{
		}

		public void Process(IList<ResultItems> resultItems, dynamic sender = null)
		{
		}
	}
}
