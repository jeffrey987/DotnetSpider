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
		/// <summary>
		/// 获取当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="key">用于引用该对象的缓存键。</param>
		/// <returns>缓存中的对象。</returns>
		public static object Get(string key)
		{
			var cache = TestManagers.WithManyDictionaryHandles;
			return objCache.Get(key);
		}
		/// <summary>
		/// 获取当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey">用于引用该对象的缓存键。</param>
		/// <returns>缓存中的对象。</returns>
		public static object GetCache(string CacheKey)
		{
			System.Web.Caching.Cache objCache = HttpRuntime.Cache;
			return objCache[CacheKey];
		}

		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey">用于引用该对象的缓存键。</param>
		/// <param name="objObject">要插入缓存中的对象。</param>
		public static void SetCache(string CacheKey, object objObject)
		{
			System.Web.Caching.Cache objCache = HttpRuntime.Cache;
			objCache.Insert(CacheKey, objObject);
		}
		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey">用于引用该对象的缓存键。</param>
		/// <param name="objObject">要插入缓存中的对象。</param>
		/// <param name="absoluteExpiration"> 所插入对象将到期并被从缓存中移除的时间。 要避免可能的本地时间问题（例如从标准时间改为夏时制），请使用 System.DateTime.UtcNow 而不是
		//     System.DateTime.Now 作为此参数值。 如果使用绝对到期，则 slidingExpiration 参数必须设置为 System.Web.Caching.Cache.NoSlidingExpiration。</param>
		/// <param name="slidingExpiration">缓存对象的上次访问时间和对象的到期时间之间的时间间隔。 如果该值等效于 20 分钟，则对象在最后一次被访问 20 分钟之后将到期并被从缓存中移除。 如果使用可调到期，则
		//     absoluteExpiration 参数必须设置为 System.Web.Caching.Cache.NoAbsoluteExpiration。</param>
		public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
		{
			System.Web.Caching.Cache objCache = HttpRuntime.Cache;
			objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
		}
		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey">用于引用该对象的缓存键。</param>
		/// <param name="objObject">要插入缓存中的对象。</param>
		/// <param name="dependencies"> 所插入对象的文件依赖项或缓存键依赖项。 当任何依赖项更改时，该对象即无效，并从缓存中移除。 如果没有依赖项，则此参数包含 null。</param>
		/// <param name="absoluteExpiration"> 所插入对象将到期并被从缓存中移除的时间。 要避免可能的本地时间问题（例如从标准时间改为夏时制），请使用 System.DateTime.UtcNow 而不是
		//     System.DateTime.Now 作为此参数值。 如果使用绝对到期，则 slidingExpiration 参数必须设置为 System.Web.Caching.Cache.NoSlidingExpiration。</param>
		/// <param name="slidingExpiration">缓存对象的上次访问时间和对象的到期时间之间的时间间隔。 如果该值等效于 20 分钟，则对象在最后一次被访问 20 分钟之后将到期并被从缓存中移除。 如果使用可调到期，则
		//     absoluteExpiration 参数必须设置为 System.Web.Caching.Cache.NoAbsoluteExpiration。</param>
		public static void SetCache(string CacheKey, object objObject, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
		{
			System.Web.Caching.Cache objCache = HttpRuntime.Cache;
			objCache.Insert(CacheKey, objObject, dependencies, absoluteExpiration, slidingExpiration);
		}
		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey">用于引用该对象的缓存键。</param>
		/// <param name="objObject">要插入缓存中的对象。</param>
		/// <param name="dependencies"> 所插入对象的文件依赖项或缓存键依赖项。 当任何依赖项更改时，该对象即无效，并从缓存中移除。 如果没有依赖项，则此参数包含 null。</param>
		/// <param name="absoluteExpiration"> 所插入对象将到期并被从缓存中移除的时间。 要避免可能的本地时间问题（例如从标准时间改为夏时制），请使用 System.DateTime.UtcNow 而不是
		//     System.DateTime.Now 作为此参数值。 如果使用绝对到期，则 slidingExpiration 参数必须设置为 System.Web.Caching.Cache.NoSlidingExpiration。</param>
		/// <param name="slidingExpiration">缓存对象的上次访问时间和对象的到期时间之间的时间间隔。 如果该值等效于 20 分钟，则对象在最后一次被访问 20 分钟之后将到期并被从缓存中移除。 如果使用可调到期，则
		//     absoluteExpiration 参数必须设置为 System.Web.Caching.Cache.NoAbsoluteExpiration。</param>
		/// <param name="onUpdateCallback">从缓存中移除对象之前将调用的委托。 可以使用它来更新缓存项并确保缓存项不会从缓存中移除。</param>
		public static void SetCache(string CacheKey, object objObject, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback)
		{
			System.Web.Caching.Cache objCache = HttpRuntime.Cache;
			objCache.Insert(CacheKey, objObject, dependencies, absoluteExpiration, slidingExpiration, onUpdateCallback);
		}
		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey">用于引用该对象的缓存键。</param>
		/// <param name="objObject">要插入缓存中的对象。</param>
		/// <param name="dependencies"> 所插入对象的文件依赖项或缓存键依赖项。 当任何依赖项更改时，该对象即无效，并从缓存中移除。 如果没有依赖项，则此参数包含 null。</param>
		/// <param name="absoluteExpiration"> 所插入对象将到期并被从缓存中移除的时间。 要避免可能的本地时间问题（例如从标准时间改为夏时制），请使用 System.DateTime.UtcNow 而不是
		//     System.DateTime.Now 作为此参数值。 如果使用绝对到期，则 slidingExpiration 参数必须设置为 System.Web.Caching.Cache.NoSlidingExpiration。</param>
		/// <param name="slidingExpiration">缓存对象的上次访问时间和对象的到期时间之间的时间间隔。 如果该值等效于 20 分钟，则对象在最后一次被访问 20 分钟之后将到期并被从缓存中移除。 如果使用可调到期，则
		//     absoluteExpiration 参数必须设置为 System.Web.Caching.Cache.NoAbsoluteExpiration。</param>
		/// <param name="priority"> 该对象相对于缓存中存储的其他项的成本，由 System.Web.Caching.CacheItemPriority 枚举表示。 该值由缓存在退出对象时使用；具有较低成本的对象在具有较高成本的对象之前被从缓存移除。</param>
		/// <param name="onRemoveCallback"> 在从缓存中移除对象时将调用的委托（如果提供）。 当从缓存中删除应用程序的对象时，可使用它来通知应用程序。</param>
		public static void SetCache(string CacheKey, object objObject, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
		{
			System.Web.Caching.Cache objCache = HttpRuntime.Cache;
			objCache.Insert(CacheKey, objObject, dependencies, absoluteExpiration, slidingExpiration, priority, onRemoveCallback);
		}
		/// <summary>
		///  设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey"></param>
		/// <param name="objObject"></param>
		/// <param name="dependencies"></param>
		/// <param name="absoluteExpiration"></param>
		/// <param name="slidingExpiration"></param>
		public static void Insert(string CacheKey, object objObject, SqlCacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
		{
			System.Web.Caching.Cache objCache = HttpRuntime.Cache;
			objCache.Insert(CacheKey, objObject, dependencies, absoluteExpiration, slidingExpiration);
		}
	}
}

