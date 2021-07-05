using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//A WebApi szerver megszólítására készült eljárás
public static class Client
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"> Visszadandó osztály típusa </typeparam>
    /// <param name="baseUrl"> Szerver elérési útvonala </param>
    /// <param name="url"> Eljárás relatív útvonala </param>
    /// <param name="callMethod"> Hívási mód </param>
    /// <param name="obj"> Átadott objektum </param>
    /// <returns> Visszatérési objektum </returns>
    public static T CallApi<T>(string baseUrl, string url, string callMethod, object obj)
    {
        T result = default(T);

        string responseString = "";
        HttpWebResponse response = null;

        var request = (HttpWebRequest)WebRequest.Create($"{baseUrl}{url}");
        request.Accept = "application/json";
        request.Method = callMethod;
        request.ContentType = "application/json";

        if (callMethod == "POST")
        {
            var json = JsonUtility.ToJson(obj);
            var content = Encoding.UTF8.GetBytes(json);

            request.ContentLength = content.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(content, 0, content.Length);
            }
        }

        response = (HttpWebResponse)request.GetResponse();

        responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        result = JsonUtility.FromJson  <T>(responseString);

        return result;
    }
}

