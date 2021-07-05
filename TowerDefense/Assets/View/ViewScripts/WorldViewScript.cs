using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.View
{
    public class WorldViewScript : ViewScript<World>
    {
        public Dictionary<Building, GameObject> BuildingViewObjects = new Dictionary<Building, GameObject>(); //Épületek listája.
        public Dictionary<Unit, GameObject> UnitViewObjects = new Dictionary<Unit, GameObject>(); //Egységek listája.
        public Dictionary<Tile, GameObject> TileViewObjects = new Dictionary<Tile, GameObject>(); //Tile-ok listája.
        public World World;
        public Network Network;

        public void NewGame()
        {
            World = new World(); //Új World létrehozása
            World.NewGame(); //Játék leindítása

            //Listák tisztítása.
            foreach (var keyValuePair in TileViewObjects)
            {
                Destroy(keyValuePair.Value);
            }

            foreach (var keyValuePair in BuildingViewObjects)
            {
                Destroy(keyValuePair.Value);
            }

            foreach (var keyValuePair in UnitViewObjects)
            {
                Destroy(keyValuePair.Value);
            };
            // Új listák
            TileViewObjects = new Dictionary<Tile, GameObject>();
            BuildingViewObjects = new Dictionary<Building, GameObject>();
            UnitViewObjects = new Dictionary<Unit, GameObject>();
            SetTarget(World);
            Refresh(); //Frissítés.
        }

        protected override void Refresh()
        {
            foreach (var tile in World.Tiles) //Tile-ok lekérdezése, és listába helyezése.
            {
                if (!TileViewObjects.ContainsKey(tile))
                {
                    var tileGo = Instantiate(ViewPrefabManager.GetViewObject("Tile"));
                    tileGo.GetComponent<TileViewScript>().SetTarget(tile);
                    TileViewObjects.Add(tile, tileGo);
                }
            }

            var newBuildingsToDisplay = World.Buildings.Where(b => !BuildingViewObjects.ContainsKey(b)).ToList();
            foreach (var building in newBuildingsToDisplay) //Épületek lekérdezése, és listába helyezése.
            {
                var buildingGo = Instantiate(ViewPrefabManager.GetViewObject("Building"));
                buildingGo.GetComponent<BuildingViewScript>().SetTarget(building);
                BuildingViewObjects.Add(building, buildingGo);
            }

            var buildingsToDestroy = BuildingViewObjects.Keys.Where(b => !World.Buildings.Contains(b)).ToList();
            foreach (var building in buildingsToDestroy) //Már nem szereplő objektumok törlése.
            {
                Destroy(BuildingViewObjects[building]);
                BuildingViewObjects.Remove(building);
            }

            var newUnitsToDisplay = World.Units.Where(e => !UnitViewObjects.ContainsKey(e)).ToList();
            foreach (var unit in newUnitsToDisplay) //Unittal lekérdezése, és listába helyezése.
            {
                var unitViewObject = Instantiate(ViewPrefabManager.GetViewObject("Unit"));
                unitViewObject.GetComponent<UnitViewScript>().SetTarget(unit);
                UnitViewObjects.Add(unit, unitViewObject);
            }

            var unitsToDestroy = UnitViewObjects.Keys.Where(e => !World.Units.Contains(e)).ToList();
            foreach (var unit in unitsToDestroy)//Már nem létező egységek törlése
            {
                Destroy(UnitViewObjects[unit]);
                UnitViewObjects.Remove(unit);
            }
        }

        private new void Awake() //Ébredés, létrehozáskor hívódik meg.
        {
            NewGame(); 
        }
    }
}
