using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TowerDefenseWebApi.Models
{
    //WebApi service által visszaadott osztály a POST metódusoknál
    public class ApiResult
    {
        public bool Success; //Sikeres volt-e
        public string Message; //Szerver hiba üzenet
    }
}