using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DotnetSpider.Extraction.Model
{


	public abstract class IRss : IBaseEntity
	{
		public abstract Item ToRssItem();
	}


	public class Item
	{
		public string title { get; set; }
		public string link { get; set; }
		public string guid { get; set; }
		public string description { get; set; }
	}

}
