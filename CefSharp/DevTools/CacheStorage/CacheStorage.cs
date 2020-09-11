// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CacheStorage
{
    using System.Linq;

    /// <summary>
    /// CacheStorage
    /// </summary>
    public partial class CacheStorage : DevToolsDomainBase
    {
        public CacheStorage(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Deletes a cache.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DeleteCacheAsync(string cacheId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cacheId", cacheId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CacheStorage.deleteCache", dict);
            return methodResult;
        }

        /// <summary>
        /// Deletes a cache entry.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DeleteEntryAsync(string cacheId, string request)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cacheId", cacheId);
            dict.Add("request", request);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CacheStorage.deleteEntry", dict);
            return methodResult;
        }

        /// <summary>
        /// Requests cache names.
        /// </summary>
        public async System.Threading.Tasks.Task<RequestCacheNamesResponse> RequestCacheNamesAsync(string securityOrigin)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("securityOrigin", securityOrigin);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CacheStorage.requestCacheNames", dict);
            return methodResult.DeserializeJson<RequestCacheNamesResponse>();
        }

        /// <summary>
        /// Fetches cache entry.
        /// </summary>
        public async System.Threading.Tasks.Task<RequestCachedResponseResponse> RequestCachedResponseAsync(string cacheId, string requestURL, System.Collections.Generic.IList<CefSharp.DevTools.CacheStorage.Header> requestHeaders)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cacheId", cacheId);
            dict.Add("requestURL", requestURL);
            dict.Add("requestHeaders", requestHeaders.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CacheStorage.requestCachedResponse", dict);
            return methodResult.DeserializeJson<RequestCachedResponseResponse>();
        }

        /// <summary>
        /// Requests data from cache.
        /// </summary>
        public async System.Threading.Tasks.Task<RequestEntriesResponse> RequestEntriesAsync(string cacheId, int? skipCount = null, int? pageSize = null, string pathFilter = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("cacheId", cacheId);
            if (skipCount.HasValue)
            {
                dict.Add("skipCount", skipCount.Value);
            }

            if (pageSize.HasValue)
            {
                dict.Add("pageSize", pageSize.Value);
            }

            if (!(string.IsNullOrEmpty(pathFilter)))
            {
                dict.Add("pathFilter", pathFilter);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("CacheStorage.requestEntries", dict);
            return methodResult.DeserializeJson<RequestEntriesResponse>();
        }
    }
}