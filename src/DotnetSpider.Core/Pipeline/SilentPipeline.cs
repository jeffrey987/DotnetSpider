using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Senparc.NeuChar.Entities;

namespace DotnetSpider.Core.Pipeline
{
	public class SilentPipeline : IPipeline
	{
		public ILogger Logger { get; set; }
		public System.Xml.Linq.XDocument ResponseMessage { get; set; }

		public void Dispose()
		{
		}

		public void Process(IList<ResultItems> resultItems, dynamic sender = null)
		{
		}
	}
}
