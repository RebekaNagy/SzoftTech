using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationModel
{
    //A feladott műveletek, illetve a lekérdezések eredményeként visszaadott osztály
    [Serializable]
    public class User
    {
        public string UserID; //Felhasználó egyedi azonosítója
        public string Operation; //A szerveren elvégzendő művelet
        public bool IsWin; //Nyert-e a felhasználó
        public string Message; //Exception-ök kiírása, 3. felhasználó figyelmeztetése
        public ServerResult Result; //Szerver állapotát adja vissza
    }

    //Szerver állapot osztály
    [Serializable]
    public class ServerResult
    {
        public ServerGameState GameState; //Játékállást adja vissza
        public int PlayerOrder; //Hogy hányadik játékos
        public int WinnerId; //Győztes játékos sorszáma
        public int MapId; //Térkép sorszáma
        public List<ServerEntity> Entities; //Szerverre feltöltött tornyok, egységek
    }

    //Szerver entity-k osztálya (unit, tower)
    [Serializable]
    public class ServerEntity
    {
        public int X; //X pozíciója
        public int Y; //Y pozíciója
        public string EntityType; //lehelyezett objektum típusa
    }

    //Játékállás állapota
    [Serializable]
    public enum ServerGameState
    {
        WaitStarted, //Várakozás a kezdésre
        SelectMap, //Térkép választás
        Step, //Játékos lép
        WaitOtherPlayer, //Várakozás másik játékos csatlakozására
        OtherPlayerStepped, //Másik játékos lépése van folyamatban
        EndOfCycle, //Kör vége
        EndOfGame //Játék vége
    }

    [Serializable]
    public class Map
    {
        public int MapID; //Térkép beállítására szolgáló azonosító
    }

    //WebApi service által visszaadott osztály a POST metódusoknál
    [Serializable]
    public class ApiResult
    {
        public bool Success; //Sikeres volt-e
        public string Message; //Szerver hiba üzenet
    }
}
