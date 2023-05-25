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
	public delegate void NewGoods(Goods goods);
	public delegate void NewGoodes(IEnumerable<Goods> goodes);
	public  class LinkProductCollector
	{
		bool running = false;
		public async  Task Start(string StartUrl)
		{
			running = true;
			await Work(StartUrl);
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
				doc = null;
				Task[] tasks = new Task[links.Count];
				for (int i = 0; i < links.Count; i++)
				{
                    if (StartUrl == links[i])
                        continue;
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
					//Console.WriteLine("has Goods");
				}
				else
				{
					var links = doc.GetCategorys();
					foreach (var item in links)
					{
						if (url == item)
							continue;
						await DirectoryTraversal(CategoryRequest, item);
					}
				}
                doc = null;



            }
			catch (Exception ex) 
			{
				Debug.WriteLine(url);
				Debug.WriteLine(ex.StackTrace);
			}
		}
	}
}
