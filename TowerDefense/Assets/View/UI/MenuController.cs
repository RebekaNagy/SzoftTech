using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;
using static MouseController;
using Button = UnityEngine.Experimental.UIElements.Button;
using System.Collections.Generic;
using System.Threading;

public class MenuController : MonoBehaviour
{
    //Menuben található gombok kezelése.
    public void SimpleTowerClicked()
    {
        ButtonClicking = true; //Blockoljuk a gombot, hogy kattintásnál ne rakja le rögtön a folyamatosan meghívódó update miatt.
        TowerButtonClicked = true;
        actualTower = "SimpleTower";
    }

    public void RangeTowerClicked()
    {
        ButtonClicking = true;
        TowerButtonClicked = true;
        actualTower = "RangeTower";
    }

    public void CannonTowerClicked()
    {
        ButtonClicking = true;
        TowerButtonClicked = true;
        actualTower = "CannonTower";
    }

    public void SimpleUnitClicked()
    {
        var result = World.Current.CreateUnit("SimpleUnit"); //Egység létrehozása.
        
    }

    public void StrongUnitClicked()
    {
        var result = World.Current.CreateUnit("StrongUnit");
        
    }

    //Kör vége gomb.
    public void EndRoundClicked()
    {
        //Szervertől függően adott kör végét hívjuk meg.
        if(Network.Current != null)
        {
            World.Current.ServerTurn();
        }
        else
        {
            World.Current.Turn();
        }

        if (World.Current.IsEnd)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void ButtonClick() //New Game gomb megnyomása.
    {
        if(Network.Current != null)
        {
            Network.Current.ExitGame();
            Network.Current = null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }

    public void ExitClicked() //Exit gomb megnyomása.
    {
        if (Network.Current != null)
        {
            Network.Current.ExitGame(); //Szerver leállítása, ürítése.
            Network.Current = null;
        }

        Application.Quit(); //Kilépés.
    }

    //Szerverhez szükséges metódus
    void Update() 
    {
        if(Network.Current != null)
        {
            //2. játékos lekéri a lépéseket és lehelyezi magánál
            if(Network.Current.GetServerResult().GameState == CommunicationModel.ServerGameState.Step && Network.Current.GetPlayerOrder() == 2 && World.Current.OnPlaceEntity)
            {
                World.Current.OnPlaceEntity = false;
                var list = Network.Current.GetEntities();
                World.Current.PlaceEntity(list);
            }

            //1. játékos lekéri a lépéseket és lehelyezi magánál
            else if (Network.Current.GetServerResult().GameState == CommunicationModel.ServerGameState.EndOfCycle && Network.Current.GetPlayerOrder() == 1 && World.Current.OnPlaceEntity)
            {
                World.Current.OnPlaceEntity = false;
                var list = Network.Current.GetEntities();
                World.Current.PlaceEntity(list);
            }

            //Kör végét végrehajtjuk szerverjátéknál
            else if(Network.Current.GetServerResult().GameState == CommunicationModel.ServerGameState.EndOfCycle && World.Current.OnEndTurn)
            {
                World.Current.OnEndTurn = false;
                World.Current.EndTurn();
            }
        }
    }
}