﻿using System.Collections.Generic;
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
using System.Net.Http;
using System.Net;

namespace DotnetSpider.Extension.Pipeline
{
	/// <summary>
	/// 把解析到的爬虫实体数据序列化成JSON并存到文件中
	/// </summary>
	public class XmlEntityPipeline : EntityPipeline
	{
		private readonly Dictionary<string, StreamWriter> _writers = new Dictionary<string, StreamWriter>();
		/// <summary>
		/// 标题
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// 描述
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// 链接
		/// </summary>
		public string Link { get; set; }

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
		///turn datas to list<dictionary<string,string>>
		/// </summary>
		/// <param name="datas"></param>
		/// <returns></returns>

		private static List<Dictionary<string, string>> GetListDic(IEnumerable<IBaseEntity> datas)
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

		protected override int Process(IEnumerable<IBaseEntity> datas, dynamic sender)
		{
			if (datas == null || datas.Count() == 0)
			{
				return 0;
			}

			Rss feed = new Rss();
			feed.channel.title = Title;
			feed.channel.link = Link;
			feed.channel.description = Description;
			feed.channel.lastBuildDate = DateTime.Now.ToString();

			var lists = GetListDic(datas);
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
			Stream stream = new MemoryStream();
			serializer.Serialize(stream, feed);
			var result = new ResultItems();
			Httpresponse.Content = new StreamContent(stream);

			return datas.Count();
		}
	}
}
