using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace stroyparkdiyru
{
    public class GetRequest
    {

            string BaseAddress = "https://stroyparkdiy.ru";
            public string Response { get; set; }
            public async Task RunAsync(string Url)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseAddress);
                    client.DefaultRequestHeaders.Add("method", "GET");

                    var responce = await client.GetAsync(Url);
                    try
                    {
                        if (responce.StatusCode == HttpStatusCode.OK)
                        {
                            Response = await responce.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            throw new Exception(responce.StatusCode.ToString());
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Print(e.Message);
                        Response = string.Empty;
                    }

                }
            await Task.Delay(new Random().Next(500, 1000));
        }
        }
    
}
