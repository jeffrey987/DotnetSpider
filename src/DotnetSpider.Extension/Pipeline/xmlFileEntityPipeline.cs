using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using DotnetSpider.Core;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extraction.Model;
using System.Xml;
using System.Xml.Serialization;
using System;

namespace DotnetSpider.Extension.Pipeline
{
	/// <summary>
	/// 把解析到的爬虫实体数据序列化成JSON并存到文件中
	/// </summary>
	public class XmlFileEntityPipeline : EntityPipeline
	{
		private readonly Dictionary<string, StreamWriter> _writers = new Dictionary<string, StreamWriter>();

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public override void Dispose()
		{
			base.Dispose();
			foreach (var writer in _writers)
			{
				writer.Value.Dispose();
			}
		}

		/// <summary>
		/// 把解析到的爬虫实体数据序列化成Xml并存到文件中
		/// </summary>
		/// <param name="entityName">爬虫实体类的名称</param>
		/// <param name="datas">实体类数据</param>
		/// <param name="sender">调用方</param>
		/// <returns>最终影响结果数量(如数据库影响行数)</returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		protected override int Process(IEnumerable<IBaseEntity> datas, dynamic sender = null)
		{
			if (datas == null || datas.Count() == 0)
			{
				return 0;
			}

			var tableInfo = new TableInfo(datas.First().GetType());

			StreamWriter writer;
			//var identity = GetIdentity(sender);
			var dataFolder = Path.Combine(Env.BaseDirectory, "rss", DateTime.Now.ToString("yyyyMMddHHmmss"));
			var jsonFile = Path.Combine(dataFolder, $"{tableInfo.Schema.FullName}.xml");
			if (_writers.ContainsKey(jsonFile))
			{
				writer = _writers[jsonFile];
			}
			else
			{
				if (!Directory.Exists(dataFolder))
				{
					Directory.CreateDirectory(dataFolder);
				}
				writer = new StreamWriter(File.OpenWrite(jsonFile), Encoding.UTF8);
				_writers.Add(jsonFile, writer);
			}
			Rss feed = new Rss();
			feed.channel.title = "Test feed";
			feed.channel.link = "http://blogs.msdn.com/dancre";
			feed.channel.description = "Simple feed using XmlSerialization";
			feed.channel.lastBuildDate = DateTime.Now.ToString();

			var lists = NewMethod(datas);
			foreach (var itemr in lists)
			{
				Item model = new Item();
				model.title = itemr["title"];
				model.description = itemr["description"];
				model.guid = itemr["guid"];
				model.link = itemr["link"];

				feed.channel.item.Add(model);
			}
			var serializer = new XmlSerializer(typeof(Rss));
			serializer.Serialize(writer, feed);

		
			return datas.Count();
		}

		private static List<Dictionary<string, string>> NewMethod(IEnumerable<IBaseEntity> datas)
		{
			List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
			foreach (var itemrss in datas)
			{
				Dictionary<string, string> dic = new Dictionary<string, string>();
				System.Reflection.PropertyInfo[] properties = itemrss.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
				foreach (System.Reflection.PropertyInfo item in properties)
				{
					string name = item.Name;
					object value = item.GetValue(itemrss, null);
					dic.Add(name, value == null ? "" : value.ToString());
				}
				list.Add(dic);
			}
			return list;
		}
	}
}
