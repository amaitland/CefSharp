﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Internals.Messaging
{
	/// <summary>
	/// Class to store TaskCompletionSources indexed by a unique id.
	/// </summary>
	public sealed class PendingTaskRepository : IDisposable
	{
		private readonly ConcurrentDictionary<long, TaskCompletionSource<JavascriptResponse>> pendingTasks =
			new ConcurrentDictionary<long, TaskCompletionSource<JavascriptResponse>>();
		private volatile bool disposed;
		private long lastId;

		public KeyValuePair<long, TaskCompletionSource<JavascriptResponse>> CreatePendingTask(TimeSpan? timeout)
		{
			ThrowIfDisposed();

			var completionSource = new TaskCompletionSource<JavascriptResponse>();

			var id = Interlocked.Increment(ref lastId);
			if (pendingTasks.TryAdd(id, completionSource))
			{
				if (timeout.HasValue)
				{
					Timer timer = null;
					timer = new Timer(state =>
					{
						timer.Dispose();
						RemovePendingTask(id);
						((TaskCompletionSource<JavascriptResponse>)state).TrySetCanceled();
					}, completionSource, timeout.Value, TimeSpan.FromMilliseconds(-1));
				}

				return new KeyValuePair<long, TaskCompletionSource<JavascriptResponse>>(id, completionSource);
			}

			throw new Exception("Unable to add TaskCompletionSource to ConcurrentDictionary");
		}

		public TaskCompletionSource<JavascriptResponse> RemovePendingTask(long id)
		{
			ThrowIfDisposed();

			TaskCompletionSource<JavascriptResponse> result;
			pendingTasks.TryRemove(id, out result);
			return result;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!disposed)
			{
				disposed = true;

				if (disposing)
				{
					foreach (var t in pendingTasks.Values.Where(t => !t.Task.IsCompleted))
					{
						t.TrySetCanceled();
					}
					pendingTasks.Clear();
				}
			}
		}

		private void ThrowIfDisposed()
		{
			if (disposed)
			{
				throw new ObjectDisposedException("PendingTaskRepository", "Task repository has already been disposed.");
			}
		}

		~PendingTaskRepository()
		{
			Dispose(false);
		}
	}
}
