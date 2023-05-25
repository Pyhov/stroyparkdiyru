using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace stroyparkdiyru
{
	public  class LinkProductCollector
	{
		bool running = false;
		public async  Task Start(string StartUrl)
		{
			running = true;
			Work(StartUrl);
		}
		public bool Running => running;
		async Task Work(string StartUrl)
		{
			try
			{
				
				GetRequest getRequest = new GetRequest();
				await getRequest.RunAsync(StartUrl);
				var doc = getRequest.ToDocument();
				var links = doc.GetCategorys();
				Task[] tasks = new Task[links.Count];
				for (int i = 0; i < links.Count; i++)
				{
					tasks[i] = DirectoryTraversal(new GetRequest(), links[i]);
				}
				await Task.WhenAll(tasks);	
			}
			catch (Exception ex) 
			{
				Debug.WriteLine(ex);	
			}

			running = false;	
			
		}

		async Task DirectoryTraversal(GetRequest CategoryRequest, string url)
		{
			try
			{
				
				await CategoryRequest.RunAsync(url); 
				Console.WriteLine(url);
				var doc = CategoryRequest.ToDocument();
				if (doc.HasGoods())
				{
					Console.WriteLine("has Goods");
				}
				else
				{
					var links = doc.GetCategorys();
					foreach (var item in links)
					{
						await DirectoryTraversal(CategoryRequest, item);
					}
				}



			}
			catch (Exception ex) 
			{
				Debug.WriteLine(url);
				Debug.WriteLine(ex.StackTrace);
			}
		}
	}
}
