using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class StartingLabel : MonoBehaviour
{
    public void LocalButtonClicked() //Lokális játék indítása.
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2); //mapselect scene
    }

    public void GlobalButtonClicked() //Szerverjáték indítása.
    {
        World.Network = new Network();
        string message;
        bool success = Network.Current.LoginUser(out message);

        if (message == string.Empty && success)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Network.Current = null;
        }
    }
}

