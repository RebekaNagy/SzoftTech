using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TowerDefenseWebApi.Models;
using System.Web.Script.Serialization;
using System.Text;
using System.Net;
using System.IO;

namespace UnitTestWebApi
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SetMapTest()
        {
            try
            {
                Map map = new Map();
                map.MapID = 3;

                var result = CallApi<ApiResult>("api/map", "POST", map);
            }
            catch (Exception ex)
            {

            }
        }

        [TestMethod]
        public void GetVersionTest()
        {
            var result = CallApi<ApiResult>("api/map", "GET", null);
        }

        [TestMethod]
        public void LoginUserTest()
        {
            try
            {
                User user = new User();
                user.Operation = "LoginUser";
                user.UserID = "Éva";

                var result = CallApi<ApiResult>("api/user", "POST", user);
            }
            catch (Exception ex)
            {

            }

        }

        [TestMethod]
        public void GetServerStateTest()
        {
            try
            {
                var user = CallApi<User>("api/user/Zoli", "GET", null);
            }
            catch (Exception ex)
            {

            }

        }

        [TestMethod]
        public void SendStepTest()
        {
            User user = new User();
            user.Operation = "SendStep";
            ServerResult result = new ServerResult();

            List<ServerEntity> list = new List<ServerEntity>();

            ServerEntity entity = new ServerEntity();
            entity.X = 200;
            entity.Y = 200;
            entity.EntityType = "CannonTower";
            list.Add(entity);

            ServerEntity entity2 = new ServerEntity();
            entity2.X = 450;
            entity2.Y = 300;
            entity2.EntityType = "SimpleUnit";
            list.Add(entity2);

            result.Entities = list;
            user.Result = result;

            var apiResult = CallApi<ApiResult>("api/user", "POST", user);
        }

        [TestMethod]
        public void SetEndOfRoundTest()
        {
            User user = new User();
            user.Operation = "SetEndOfRound";
            user.UserID = "Zoli";

            var result = CallApi<ApiResult>("api/user", "POST", user);
        }

        [TestMethod]
        public void SetEndOfGameTest()
        {
            User user = new User();
            user.Operation = "SetEndOfGame";
            user.UserID = "Éva";

            var result = CallApi<ApiResult>("api/user", "POST", user);
        }

        private T CallApi<T>(string url, string callMethod, object obj)
        {
            T result = default(T);

            string responseString = "";
            HttpWebResponse response = null;
            string baseUrl = "http://towerdefensewebapi2.azurewebsites.net/";

            var request = (HttpWebRequest)WebRequest.Create($"{baseUrl}{url}");
            request.Accept = "application/json";
            request.Method = callMethod;
            request.ContentType = "application/json";

            if (callMethod == "POST")
            {
                var json = new JavaScriptSerializer().Serialize(obj);
                var content = Encoding.UTF8.GetBytes(json);

                request.ContentLength = content.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(content, 0, content.Length);
                }
            }

            response = (HttpWebResponse)request.GetResponse();

            responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            result = new JavaScriptSerializer().Deserialize<T>(responseString);

            return result;
        }

        [TestMethod]
        public void ExitGameTest()
        {
            try
            {
                User user = new User();
                user.Operation = "ExitGame";
                var json = new JavaScriptSerializer().Serialize(user);
                var content = Encoding.UTF8.GetBytes(json);

                var result = CallApi<ApiResult>("api/user", "POST", user);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
