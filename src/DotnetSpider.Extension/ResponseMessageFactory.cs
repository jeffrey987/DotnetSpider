using System;
using System.Xml.Linq;
using DotnetSpider.Extension.Model;
using Senparc.NeuChar;
using Senparc.NeuChar.Entities;
using Senparc.NeuChar.Helpers;

namespace DotnetSpider.Extension
{

	public static class ResponseMessageFactory
	{
		/// <summary>
		/// 获取XDocument转换后的IResponseMessageBase实例（通常在反向读取日志的时候用到）。
		/// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
		/// </summary>
		/// <returns></returns>
		public static IResponseMessageBase GetResponseEntity(XDocument doc)
		{
			ResponseMessageBase responseMessage = null;
			ResponseMsgType msgType;
			try
			{
				//msgType = MsgTypeHelper.GetResponseMsgType(doc);
				//switch (msgType)
				//{
				//	case ResponseMsgType.Text:
				//		responseMessage = new ResponseMessageText();
				//		break;
				//	default:
				responseMessage = new ResponseMessageText();
				//		break;

				//}
				Senparc.NeuChar.Helpers.EntityHelper.FillEntityWithXml(responseMessage, doc);
			}
			catch (ArgumentException ex)
			{
				throw new Exception(string.Format("ResponseMessage转换出错！可能是MsgType不存在！，XML：{0}", doc.ToString()), ex);
			}
			return responseMessage;
		}


		/// <summary>
		/// 获取XDocument转换后的IRequestMessageBase实例。
		/// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
		/// </summary>
		/// <returns></returns>
		public static IResponseMessageBase GetResponseEntity(string xml)
		{
			return GetResponseEntity(XDocument.Parse(xml));
		}

		/// <summary>
		/// 将ResponseMessage实体转为XML
		/// </summary>
		/// <param name="entity">ResponseMessage实体</param>
		/// <returns></returns>
		public static XDocument ConvertEntityToXml(ResponseMessageBase entity)
		{
			return Senparc.NeuChar.Helpers.EntityHelper.ConvertEntityToXml(entity);
		}

	}
}
