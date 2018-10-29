using System;
using System.Web;
using CacheManager.Core;
using CacheManager.Core.Internal;

namespace DotnetSpider.Extension.Scheduler
{
	/// <summary>
	/// 缓存相关的操作类
	/// Copyright (C) Maticsoft
	/// </summary>
	public class CacheScheduler
	{

		private static ICacheManager<object> objCache = CacheFactory.Build("getStartedCache", settings =>
		   {
			   settings.WithSystemRuntimeCacheHandle("handleName");
		   });
		/// <summary>
		/// 获取当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="key">用于引用该对象的缓存键。</param>
		/// <returns>缓存中的对象。</returns>
		public static object Get(string key)
		{
			return objCache.Get(key);
		}
		/// <summary>
		/// 获取当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey">用于引用该对象的缓存键。</param>
		/// <returns>缓存中的对象。</returns>
		public static object GetCache(string CacheKey)
		{
			return objCache[CacheKey];
		}

		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey">用于引用该对象的缓存键。</param>
		/// <param name="objObject">要插入缓存中的对象。</param>
		public static bool AddCache(string CacheKey, object objObject)
		{

			return objCache.Add(CacheKey, objObject);
		}

		/// <summary>
		/// 
		/// </summary>
		public static void Clear()
		{
			objCache.Clear();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool Exists(string key)
		{
			return objCache.Exists(key);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool Remove(string key)
		{
			return objCache.Remove(key);
		}


	}
}

