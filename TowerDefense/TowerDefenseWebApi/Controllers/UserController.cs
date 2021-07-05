using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TowerDefenseWebApi.Models;

namespace TowerDefenseWebApi.Controllers
{
    //A szervernek küldött üzenetek végrehajtását szolgálja
    public class UserController : ApiController
    {
        public HttpResponseMessage Post([FromBody]User value)
        {
            ApiResult result = new ApiResult();
            result.Success = true;
            result.Message = string.Empty;

            try
            {
                if (value.Operation == "LoginUser") //Felhasználó bejelentkezés
                {
                    using (TowerDefenseEntities e = new TowerDefenseEntities())
                    {
                        int count = e.TD_Player.Count();
                        switch (count)
                        {
                            //ha 0, akkor az első játékos jelentkezik be
                            case 0:
                                //létrehozzuk a játékost
                                TD_Player player = AddPlayer(value.UserID);
                                e.TD_Player.Add(player);
                                //Kiürítjük a többi táblát, ha maradt benne valami
                                if (e.TD_Game.Count() > 0)
                                {
                                    e.TD_Game.RemoveRange(e.TD_Game.ToList());
                                }
                                if (e.TD_GameStep.Count() > 0)
                                {
                                    e.TD_GameStep.RemoveRange(e.TD_GameStep.ToList());
                                }
                                e.SaveChanges();
                                //Létrehozzuk a játékot
                                TD_Game game = new TD_Game();
                                game.GameId = 1;
                                game.GameState = "WaitOtherPlayer";
                                game.StateCount = 0;
                                e.TD_Game.Add(game);
                                e.SaveChanges();
                                break;
                            //ha 1, akkor bejelentkezik a második játékos és kisorsoljuk a sorrendet
                            case 1:
                                //létrehozzuk a játékost
                                TD_Player player2 = AddPlayer(value.UserID);
                                e.TD_Player.Add(player2);
                                e.SaveChanges();
                                //Kisorsoljuk a sorrendet
                                int i = 0;
                                var time = DateTime.Now.Millisecond;
                                if ((time / 2) * 2 == time)
                                {
                                    i = 1;
                                }
                                int j = i == 0 ? 1 : 0;
                                var players = e.TD_Player.ToList();
                                players[i].Ordered = 1;
                                players[i].OnStep = false;
                                players[j].Ordered = 2;
                                players[j].OnStep = true;
                                //Átállítjuk a játék státuszát
                                var game2 = e.TD_Game.First();
                                game2.GameState = "WaitStarted";
                                e.SaveChanges();
                                break;
                            //minden egyéb esetben üzenetet küldünk
                            default:
                                result.Message = "Játék van folyamatban. Kérjük később próbálkozzon!";
                                break;
                        }
                    }
                }
                if (value.Operation == "SendStep") //Lépés elküldése (egységek, tornyok)
                {
                    using (TowerDefenseEntities e = new TowerDefenseEntities())
                    {
                        //Kiürítjük az előző lépéseket
                        if (e.TD_GameStep.Count() > 0)
                        {
                            e.TD_GameStep.RemoveRange(e.TD_GameStep.ToList());
                            e.SaveChanges();
                        }

                        //Feltöltjük az adatbázist az új objektumokkal
                        int i = 1;
                        foreach (var entity in value.Result.Entities)
                        {
                            TD_GameStep step = new TD_GameStep();
                            step.StepId = i;
                            step.PositionX = entity.X;
                            step.PositionY = entity.Y;
                            step.EntityType = entity.EntityType;

                            e.TD_GameStep.Add(step);
                            i++;
                        }

                        //Beállítjuk a játék állapotát
                        var game = e.TD_Game.First();
                        var players = e.TD_Player.OrderBy(x => x.Ordered).ToArray();
                        if (game.GameState == "FirstStep")
                        {
                            game.GameState = "SecondStep";
                            players[0].OnStep = false;
                            players[1].OnStep = true;
                        }
                        else
                        {
                            game.GameState = "EndOfRound";
                            players[0].OnStep = false;
                            players[1].OnStep = false;
                        }

                        e.SaveChanges();

                    }
                }
                if (value.Operation == "SetEndOfRound") //Kör vége
                {
                    using (TowerDefenseEntities e = new TowerDefenseEntities())
                    {
                        var game = e.TD_Game.First();
                        if (game.StateCount == 1)
                        {
                            game.StateCount = 0;
                            game.CalledUser = null;
                            game.GameState = "FirstStep";

                            var players = e.TD_Player.OrderBy(x => x.Ordered).ToArray();
                            players[0].OnStep = true;
                            players[1].OnStep = false;
                        }
                        else
                        {
                            game.StateCount = 1;
                            game.CalledUser = value.UserID;
                        }

                        e.SaveChanges();
                    }
                }
                if (value.Operation == "SetEndOfGame") //Játék vége
                {
                    using (TowerDefenseEntities e = new TowerDefenseEntities())
                    {
                        var game = e.TD_Game.First();
                        if (game.StateCount == 1)
                        {
                            game.GameState = "GameOver";
                            game.CalledUser = null;
                            game.StateCount = 0;
                        }
                        else
                        {
                            game.StateCount = 1;
                            game.CalledUser = value.UserID;
                        }
                        if (value.IsWin)
                        {
                            game.WinnerUser = value.UserID;
                        }

                        e.SaveChanges();
                    }
                }
                if (value.Operation == "ExitGame") //Kilépés a játékból, kiürítjük az adatbázis tábláit
                {
                    using (TowerDefenseEntities e = new TowerDefenseEntities())
                    {
                        if (e.TD_Player.Count() > 0)
                        {
                            e.TD_Player.RemoveRange(e.TD_Player.ToList());
                        }
                        if (e.TD_Game.Count() > 0)
                        {
                            e.TD_Game.RemoveRange(e.TD_Game.ToList());
                        }
                        if (e.TD_GameStep.Count() > 0)
                        {
                            e.TD_GameStep.RemoveRange(e.TD_GameStep.ToList());
                        }

                        e.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                result.Success = false;
                result.Message = ex.Message;
            }

            return Request.CreateResponse<ApiResult>(HttpStatusCode.OK, result);
        }

        //Adott játékosnak visszaadja a játék állapotot és az egyéb objektumokat (pl. lépés)
        public User Get(string id)
        {
            User user = new User();
            user.Message = string.Empty;

            try
            {
                using (TowerDefenseEntities e = new TowerDefenseEntities())
                {
                    var player = e.TD_Player.FirstOrDefault(x => x.UserId == id);
                    if (player != null)
                    {
                        var game = e.TD_Game.First();
                        user.Result = new ServerResult();

                        switch (game.GameState) //Játékállapot alapján eldöntjük, hogy mit adunk vissza
                        {
                            case "WaitOtherPlayer": //Másik játékos bejelentkezésére várunk
                                user.Result.GameState = ServerGameState.WaitOtherPlayer;
                                break;
                            case "WaitStarted": //2. játékos kiválasztja a térképet, 1. vár erre
                                if (player.Ordered == 2)
                                {
                                    user.Result.GameState = ServerGameState.SelectMap;
                                    user.Result.PlayerOrder = 2;
                                }
                                else
                                {
                                    user.Result.GameState = ServerGameState.WaitStarted;
                                    user.Result.PlayerOrder = 1;
                                }
                                break;
                            case "FirstStep": //Az 1. játékos lép, 2. játékos nem csinál semmit
                                user.Result.PlayerOrder = player.Ordered;
                                user.Result.MapId = game.GameMapId;
                                if (player.Ordered == 1 && (bool)player.OnStep)
                                {
                                    user.Result.GameState = ServerGameState.Step;
                                }
                                else
                                {
                                    user.Result.GameState = ServerGameState.OtherPlayerStepped;
                                    user.Result.PlayerOrder = 2;
                                }
                                break;
                            case "SecondStep": //A 2. játékos lép, előtte lerakja a változásokat
                                user.Result.PlayerOrder = player.Ordered;
                                user.Result.MapId = game.GameMapId;
                                if (player.Ordered == 2 && (bool)player.OnStep)
                                {
                                    user.Result.GameState = ServerGameState.Step;
                                    var steps = e.TD_GameStep.OrderBy(x => x.StepId).ToList();
                                    user.Result.Entities = new List<ServerEntity>();
                                    foreach (var step in steps)
                                    {
                                        ServerEntity entity = new ServerEntity();
                                        entity.X = step.PositionX;
                                        entity.Y = step.PositionY;
                                        entity.EntityType = step.EntityType;

                                        user.Result.Entities.Add(entity);
                                    }
                                }
                                else
                                {
                                    user.Result.GameState = ServerGameState.OtherPlayerStepped;
                                }
                                break;
                            case "EndOfRound": //Kör vége, 1. játékos lerakja a változásokat
                                if (game.CalledUser == null || game.CalledUser != id)
                                {
                                    user.Result.GameState = ServerGameState.EndOfCycle;
                                }
                                else
                                {
                                    user.Result.GameState = ServerGameState.WaitOtherPlayer;
                                }
                                user.Result.PlayerOrder = player.Ordered;
                                user.Result.MapId = game.GameMapId;
                                if (player.Ordered == 1)
                                {
                                    var steps = e.TD_GameStep.OrderBy(x => x.StepId).ToList();
                                    user.Result.Entities = new List<ServerEntity>();
                                    foreach (var step in steps)
                                    {
                                        ServerEntity entity = new ServerEntity();
                                        entity.X = step.PositionX;
                                        entity.Y = step.PositionY;
                                        entity.EntityType = step.EntityType;

                                        user.Result.Entities.Add(entity);
                                    }
                                }
                                break;
                            case "GameOver": //Játék vége
                                user.Result.GameState = ServerGameState.EndOfGame;
                                user.Result.PlayerOrder = player.Ordered;
                                user.Result.WinnerId = e.TD_Player.First(x => x.UserId == game.WinnerUser).Ordered;
                                if (game.StateCount == 0)
                                {
                                    game.StateCount = 1;
                                }
                                else
                                {
                                    e.TD_Player.RemoveRange(e.TD_Player.ToList());
                                }
                                e.SaveChanges();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                user.Result = null;
                user.Message = ex.Message;
            }

            return user;
        }

        //Játékos felvétele az adatbázisba
        private TD_Player AddPlayer(string userID)
        {
            TD_Player player = new TD_Player();
            player.UserId = userID;
            player.LoginTime = DateTime.Now;

            return player;
        }
    }
}
