using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System.Reflection;
using CommunicationModel;

//Szervert és klienst összekötő osztály
public class Network
{
    public static Network Current { get; set; } //Aktuális osztály objektum

    public Timer timer;

    private TDConfiguration configuration;

    private string userID;

    private CommunicationModel.ServerResult serverResult;

    public Network()
    {
        Current = this;

        configuration = GetConfiguration();

        WriteError($"EndpointAddress: {configuration.EndpointAddress}");

        timer = new Timer();
        timer.Interval = configuration.TimerInterval;
        timer.Elapsed += new ElapsedEventHandler(TimerElapsed);

        userID = configuration.UserID;
    }

    //Visszaadja a szerver verziót
    public string GetVersion()
    {
        string result = string.Empty;
        ApiResult apiResult = null;

        try
        {
            apiResult = Client.CallApi<ApiResult>(configuration.EndpointAddress, "api/map", "GET", null);
            if (apiResult.Success)
            {
                result = apiResult.Message;
            }
            else
            {
                throw new Exception($"API ERROR: {apiResult.Message}");
            }
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }

        return result;
    }

    //Játékos bejelentkezése
    public bool LoginUser(out string message)
    {
        bool result = false;
        message = string.Empty;
        ApiResult apiResult = null;

        try
        {
            User user = new User();
            user.Operation = "LoginUser";
            user.UserID = userID;

            apiResult = Client.CallApi<ApiResult>(configuration.EndpointAddress, "api/user", "POST", user);

            //Ha rendesen bejelentkeztünk, akkor indul a timer
            if (apiResult.Success)
            {
                message = apiResult.Message;
                result = true;
                if (message == string.Empty)
                {
                    timer.Start();
                }
            }
            else
            {
                throw new Exception($"API ERROR: {apiResult.Message}");
            }
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }

        return result;
    }

    //Térkép beállítása
    public bool SetMap(int mapID)
    {
        bool result = false;
        ApiResult apiResult = null;

        try
        {
            Map map = new Map();
            map.MapID = mapID;

            apiResult = Client.CallApi<ApiResult>(configuration.EndpointAddress, "api/map", "POST", map);

            //Ha beállítottuk a térképet, akkor indul a timer
            if (apiResult.Success)
            {
                result = true;
                timer.Start();
            }
            else
            {
                throw new Exception($"API ERROR: {apiResult.Message}");
            }
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }

        return result;
    }

    //Szerver lekérdezésekért felelős időzítő és végrehajtja az állapotfüggő műveletet
    private void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        try
        {
            User user = Client.CallApi<User>(configuration.EndpointAddress, $"api/user/{userID}", "GET", null);
            if (user.Message == string.Empty)
            {
                ServerResult result = user.Result;
                if (result != null)
                {
                    serverResult = result;

                    switch (result.GameState)
                    {
                        case ServerGameState.WaitOtherPlayer:
                            break;

                        case ServerGameState.OtherPlayerStepped:
                            World.Current.CanStep = false;
                            break;

                        case ServerGameState.SelectMap:
                            timer.Stop();
                            break;

                        case ServerGameState.WaitStarted:
                            break;

                        case ServerGameState.Step:
                            timer.Stop();
                            if (result.PlayerOrder == 2)
                            {
                                World.Current.OnPlaceEntity = true;
                            }

                            World.Current.CanStep = true;
                            World.Current.NewRound = true;
                            break;

                        case ServerGameState.EndOfCycle:
                            timer.Stop();
                            if (result.PlayerOrder == 1 && World.Current.NewRound)
                            {
                                World.Current.OnPlaceEntity = true;
                                World.Current.OnEndTurn = true;
                                World.Current.NewRound = false;
                            }
                            else if(World.Current.NewRound)
                            {
                                World.Current.OnEndTurn = true;
                                World.Current.NewRound = false;
                            }
                            
                            break;

                        case ServerGameState.EndOfGame:
                            timer.Stop();
                            break;
                    }
                }
            }
            else
            {
                throw new Exception($"API ERROR: {user.Message}");
            }
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }
    }

    //Lépéseket elküldő eljárás
    public bool SendStep(List<CommunicationModel.ServerEntity> entities)
    {
        bool result = false;
        ApiResult apiResult = null;

        try
        {
            User user = new User();
            user.Operation = "SendStep";
            ServerResult serverResult = new ServerResult();
            serverResult.Entities = entities;
            user.Result = serverResult;

            apiResult = Client.CallApi<ApiResult>(configuration.EndpointAddress, "api/user", "POST", user);
            //Ha sikerült elküldeni a lépést, akkor indul a timer
            if (apiResult.Success)
            {
                result = true;
                timer.Start();
            }
            else
            {
                throw new Exception($"API ERROR: {apiResult.Message}");
            }
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }

        return result;
    }

    //Kör végét beállító eljárás
    public bool SetEndOfRound()
    {
        bool result = false;
        ApiResult apiResult = null;

        try
        {
            User user = new User();
            user.Operation = "SetEndOfRound";
            user.UserID = userID;

            apiResult = Client.CallApi<ApiResult>(configuration.EndpointAddress, "api/user", "POST", user);
            //Ha sikerült beállítani a kör végét, akkor indul a timer
            if (apiResult.Success)
            {
                result = true;
                timer.Start();
            }
            else
            {
                throw new Exception($"API ERROR: {apiResult.Message}");
            }
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }

        return result;
    }

    //Játék végét beállító eljárás
    public bool SetEndOfGame(bool isWin)
    {
        bool result = false;
        ApiResult apiResult = null;

        try
        {
            User user = new User();
            user.Operation = "SetEndOfGame";
            user.UserID = userID;

            apiResult = Client.CallApi<ApiResult>(configuration.EndpointAddress, "api/user", "POST", user);
            //Ha sikerült beállítani a kör végét, akkor indul a timer
            if (apiResult.Success)
            {
                result = true;
                timer.Start();
            }
            else
            {
                throw new Exception($"API ERROR: {apiResult.Message}");
            }
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }

        return result;
    }

    //Játék végét lejelentő (összes adatot törli a szerveren)
    public bool ExitGame()
    {
        bool result = false;
        ApiResult apiResult = null;

        try
        {
            User user = new User();
            user.Operation = "ExitGame";
            user.UserID = userID;

            apiResult = Client.CallApi<ApiResult>(configuration.EndpointAddress, "api/user", "POST", user);
            if (apiResult.Success)
            {
                result = true;
            }
            else
            {
                throw new Exception($"API ERROR: {apiResult.Message}");
            }
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }

        return result;
    }
 
    //Szerver állapotot visszaad
    public ServerResult GetServerResult()
    {
        return serverResult;
    }

    //Térkép azonosítót visszaadja
    public int GetMapID()
    {
        return (int)serverResult.MapId;
    }

    //Játékos sorszámát visszaadja
    public int GetPlayerOrder()
    {
        return (int)serverResult.PlayerOrder;
    }

    //Másik játékos által végrehajtott lépéseket
    public List<ServerEntity> GetEntities()
    {
        return serverResult.Entities;
    }

    #region Segéd eljárások

    private TDConfiguration GetConfiguration()
    {
        TDConfiguration result = null;

        try
        {
            var path = Path.GetDirectoryName(Application.dataPath);
            WriteError($"Application path: {path}");
#if DEBUG
            path = @"c:\TowerDefense\";
#endif
            path = path + @"\Config\Configuration.xml";

            WriteError($"Configuration path: {path}");

            var xml = File.ReadAllText(path);

            XmlSerializer ser = new XmlSerializer(typeof(TDConfiguration));
            using (StringReader reader = new StringReader(xml))
            {
                result = (TDConfiguration)ser.Deserialize(reader);
            }
        }
        catch (Exception ex)
        {
            WriteError(ex);
        }

        return result;
    }

    private void SetConfiguration()
    {
        TDConfiguration configuration = new TDConfiguration();
        //configuration.Binding = new TDConfigurationBinding();
        //configuration.Binding.CloseTimeout = 10;
        //configuration.Binding.OpenTimeout = 10;
        //configuration.Binding.MaxBufferPoolSize = 1000000;
        //configuration.Binding.ReaderQuotas = new TDConfigurationBindingReaderQuotas();
        //configuration.Binding.ReaderQuotas.MaxArrayLength = 1000000;
        configuration.EndpointAddress = "http://localhost/test.svc";
        configuration.TimerInterval = 10000;
        configuration.UserID = "Zoli";

        XmlSerializer ser = new XmlSerializer(typeof(TDConfiguration));
        using (StringWriter writer = new StringWriter())
        {
            ser.Serialize(writer, configuration);
            File.WriteAllText(@"c:\TowerDefense\Config\Configuration2.xml", writer.ToString());
        }

    }

    //private BasicHttpBinding GetBinding()
    //{
    //    BasicHttpBinding binding = new BasicHttpBinding();

    //    binding.Security.Mode = BasicHttpSecurityMode.None;
    //    binding.CloseTimeout = TimeSpan.FromMinutes(configuration.Binding.CloseTimeout);
    //    binding.OpenTimeout = TimeSpan.FromMinutes(configuration.Binding.OpenTimeout);
    //    binding.ReceiveTimeout = TimeSpan.FromMinutes(configuration.Binding.ReceiveTimeout);
    //    binding.SendTimeout = TimeSpan.FromMinutes(configuration.Binding.SendTimeout);
    //    binding.MessageEncoding = WSMessageEncoding.Text;
    //    binding.TextEncoding = Encoding.UTF8;
    //    binding.MaxBufferPoolSize = configuration.Binding.MaxBufferPoolSize;
    //    binding.MaxReceivedMessageSize = configuration.Binding.MaxReceivedMessageSize;
    //    binding.ReaderQuotas.MaxDepth = configuration.Binding.ReaderQuotas.MaxDepth;
    //    binding.ReaderQuotas.MaxStringContentLength = configuration.Binding.ReaderQuotas.MaxStringContentLength;
    //    binding.ReaderQuotas.MaxArrayLength = configuration.Binding.ReaderQuotas.MaxArrayLength;
    //    binding.ReaderQuotas.MaxBytesPerRead = configuration.Binding.ReaderQuotas.MaxBytesPerRead;
    //    binding.ReaderQuotas.MaxNameTableCharCount = configuration.Binding.ReaderQuotas.MaxNameTableCharCount;

    //    return binding;
    //}

    private void WriteError(Exception ex)
    {
        var path = Path.GetDirectoryName(Application.dataPath);
#if DEBUG
        path = @"c:\TowerDefense\";
#endif
        var errorFile = path + @"\Error\Error.txt";
        string errorText = $"Időpont: {DateTime.Now} Hiba üzenet: {ex.ToString()}" + Environment.NewLine;
        if (ex.InnerException != null)
        {
            errorText += "INNER EXCEPTION:" + ex.InnerException.ToString() + Environment.NewLine;
        }

        if (File.Exists(errorFile))
        {
            File.AppendAllText(errorFile, errorText);
        }
        else
        {
            File.WriteAllText(errorFile, errorText);
        }
    }

    private void WriteError(string message)
    {
        var path = Path.GetDirectoryName(Application.dataPath);
#if DEBUG
        path = @"c:\TowerDefense\";
#endif
        var errorFile = path + @"\Error\Error.txt";

        message = $"Időpont: {DateTime.Now} Hiba üzenet: {message}" + Environment.NewLine;

        if (File.Exists(errorFile))
        {
            File.AppendAllText(errorFile, message);
        }
        else
        {
            File.WriteAllText(errorFile, message);
        }
    }

    #endregion
}


