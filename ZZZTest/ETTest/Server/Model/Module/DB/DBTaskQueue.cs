﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ETModel
{
	[ObjectSystem]
	public class DbTaskQueueAwakeSystem : AwakeSystem<DBTaskQueue>
	{
		public override void Awake(DBTaskQueue self)
		{
			self.queue.Clear();
		}
	}

	[ObjectSystem]
	public class DbTaskQueueStartSystem : StartSystem<DBTaskQueue>
	{
		public override async void Start(DBTaskQueue self)
		{
			while (true)
			{
				if (self.IsDisposed)
				{
					return;
				}

				DBTask task = await self.Get();

				try
				{
					await task.Run();

					task.Dispose();
				}
				catch (Exception e)
				{
					Log.Error(e);
				}
			}
		}
	}

	public sealed class DBTaskQueue : Component
	{
		public Queue<DBTask> queue = new Queue<DBTask>();

		public TaskCompletionSource<DBTask> tcs;

		public void Add(DBTask task)
		{
			if (this.tcs != null)
			{
				var t = this.tcs;
				this.tcs = null;
				t.SetResult(task);
				return;
			}
			
			this.queue.Enqueue(task);
		}

		public Task<DBTask> Get()
		{
			if (this.queue.Count > 0)
			{
				DBTask task = this.queue.Dequeue();
				return Task.FromResult(task);
			}

			TaskCompletionSource<DBTask> t = new TaskCompletionSource<DBTask>();
			this.tcs = t;
			return t.Task;
		}
	}
}