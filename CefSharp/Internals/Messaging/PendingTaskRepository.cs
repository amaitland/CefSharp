// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CefSharp.Internals.Messaging
{
	/// <summary>
	/// Class to store TaskCompletionSources indexed by a unique id.
	/// </summary>
	/// <typeparam name="TResult">The type of the result produced by the tasks held.</typeparam>
	public sealed class PendingTaskRepository<TResult> : IDisposable
	{
		private readonly ConcurrentDictionary<long, TaskCompletionSource<TResult>> pendingTasks =
			new ConcurrentDictionary<long, TaskCompletionSource<TResult>>();
		private volatile bool disposed;
		private long lastId;

		public long CreatePendingTask(out TaskCompletionSource<TResult> completionSource)
		{
			ThrowIfDisposed();

			completionSource = new TaskCompletionSource<TResult>();
			return SaveCompletionSource(completionSource);
		}

		public long CreatePendingTaskWithTimeout(out TaskCompletionSource<TResult> completionSource, TimeSpan timeout)
		{
			ThrowIfDisposed();

			completionSource = new TaskCompletionSource<TResult>();
			var id = SaveCompletionSource(completionSource);
			Timer timer = null;
			timer = new Timer(state =>
			{
				timer.Dispose();
				RemovePendingTask(id);
				((TaskCompletionSource<TResult>)state).TrySetCanceled();
			}, completionSource, timeout, TimeSpan.FromMilliseconds(-1));

			return id;
		}

		public TaskCompletionSource<TResult> RemovePendingTask(long id)
		{
			ThrowIfDisposed();

			TaskCompletionSource<TResult> result;
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

		private long SaveCompletionSource(TaskCompletionSource<TResult> completionSource)
		{
			var id = Interlocked.Increment(ref lastId);
			pendingTasks.TryAdd(id, completionSource);
			return id;
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
