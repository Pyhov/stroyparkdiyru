using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stroyparkdiyru
{
	public static  class HtmlHelper
	{
		public static HtmlDocument ToDocument(this GetRequest getRequest)
		{
			if (getRequest.Response == string.Empty)
				throw new Exception("GetRequest:Ответ не получен");
			return getRequest.Response.CreateDocument();
		}
		public static HtmlDocument CreateDocument(this string html)
		{
			HtmlDocument document = new HtmlDocument();
			document.LoadHtml(html);
			return document;
		}
		
		public  static List<string> GetCategorys(this HtmlDocument document) 
		{
			object objLock = new object();
			var categories = new List<string>();
			var root = document.DocumentNode;
			var aNodes = root.SelectNodes("//a[@class='c-category-item-meta']");
			if(aNodes !=null)
			{
				aNodes.AsParallel().ForAll(a =>
				{
					var href = a.Attributes["href"].Value;
					lock(objLock) 
					{
						categories.Add(href);
					}
				});
			}

			return categories;
		}
		public static bool HasGoods(this HtmlDocument document)
		{
			var root = document.DocumentNode;
			var spProductNode = root.SelectSingleNode("//div[@class='sp-product-list ajax-product-list']");
			return spProductNode!=null;
		}

	}
}
