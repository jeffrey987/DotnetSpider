using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DotnetSpider.Extraction.Model;

namespace DotnetSpider.Extension.Model
{
	/// <summary>
	/// 
	/// </summary>
	[XmlRoot("rss")]
	public class Rss : IRss
	{
		[XmlAttribute]
		public string version { get; set; } = "2.0";
		public Channel channel { get; set; } = new Channel();

		public override Item ToRssItem()
		{
			throw new NotImplementedException();
		}
	}
	[XmlRoot("channel")]
	public class Channel
	{
		public string title { get; set; }
		public string link { get; set; }
		public string description { get; set; }
		public string generator { get; set; }
		public string webMaster { get; set; }

		public string language { get; set; }

		public string lastBuildDate { get; set; }

		public string ttl { get; set; }

		[XmlElement]
		public List<Item> item { get; set; } = new List<Item>();
	}


}
