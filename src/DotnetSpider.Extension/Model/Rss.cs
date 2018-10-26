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
	public class rss : IRss
	{
		[XmlAttribute]
		public string version { get; set; } = "2.0";
		public channel channel { get; set; } = new channel();


	}
	[XmlRoot("channel")]
	public class channel
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
	
		public List<Extension.Model.item> item { get; set; } = new List<item>();
	}
	[XmlRoot("item")]
	public class item
	{
		public string title { get; set; }
		public string link { get; set; }
		public string guid { get; set; }
		public string description { get; set; }
	}


}
