using Senparc.NeuChar.Entities;

using Senparc.NeuChar;
using DotnetSpider.Extension.Model;

namespace DotnetSpider.Extension.Model
{
	public class ResponseMessageText : ResponseMessageBase, IResponseMessageText
	{
		public override ResponseMsgType MsgType
		{
			get { return ResponseMsgType.Text; }
		}
		public string Content { get; set; }

		public rss result { get; set; }
		
	}
}