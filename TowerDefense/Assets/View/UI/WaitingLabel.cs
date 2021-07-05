using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class WaitingLabel : MonoBehaviour
{
    //Várakoztató Scene megvalósítása.

    private int time = 0; //Várakoztatás ideje.
    void Update()
    {
        if (time > 1000 && Network.Current.GetServerResult() != null)
        {
            //Térkép kiválasztására dobó elágazás
            if(Network.Current.GetServerResult().GameState == CommunicationModel.ServerGameState.SelectMap)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            //Másik játékos által kiválasztott térkép betöltése
            else if(Network.Current.GetServerResult().GameState == CommunicationModel.ServerGameState.Step)
            {
                SpriteMaps.SelectMap(Network.Current.GetMapID());
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            }
        }
        else
        {
            time++;
        }

    }
}

