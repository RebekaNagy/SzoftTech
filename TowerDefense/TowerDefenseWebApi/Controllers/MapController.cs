using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using TowerDefenseWebApi.Models;

namespace TowerDefenseWebApi.Controllers
{
    public class MapController : ApiController
    {
        //Használt térkép beállítása
        public HttpResponseMessage Post([FromBody]Map value)
        {
            ApiResult result = new ApiResult();
            result.Success = true;
            result.Message = string.Empty;

            try
            {
                using (TowerDefenseEntities e = new TowerDefenseEntities())
                {
                    var game = e.TD_Game.First(); //Kiválasztjuk a játékot
                    game.GameMapId = value.MapID; //Beállítjuk a térkép azonosítót
                    game.GameState = "FirstStep"; //Játék állapot: "első lépés következik"

                    var players = e.TD_Player.OrderBy(x => x.Ordered).ToArray();
                    //1. játékos léphet, 2. nem
                    players[0].OnStep = true;
                    players[1].OnStep = false;

                    e.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return Request.CreateResponse<ApiResult>(HttpStatusCode.OK, result);
        }

        //Szerver verziót adja vissza
        public ApiResult Get()
        {
            ApiResult result = new ApiResult();
            result.Success = true;
            result.Message = string.Empty;

            try
            {
                result.Message = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}
