using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TowerDefenseTest
{
    [TestClass]
    public class UnitTest
    {
        private World _world;

        [TestInitialize]
        public void Initialize()
        {
            _world = new World();
            Maps.Initialize("elso");
            _world.NewGame();
        }

        [TestMethod]
        public void NewGameTest()
        {
            Assert.AreEqual(_world.Size, 20);
            Assert.AreEqual(_world.Players[0].Coins, 50);
            Assert.AreEqual(_world.Tiles[9, 18].Type, TileType.Occupied);
            Assert.AreEqual(_world.ActualPlayer.Faction, Faction.Alliance);
        }

        [TestMethod]
        public void PlaceSimpleTowerTest()
        {
            var result = _world.CreateTower(4, 4, "SimpleTower");
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.ErrorMessage, "Nincs eleg penzed!");
            Assert.AreEqual(_world.Tiles[4, 4].Type, TileType.Occupied);
            Assert.AreEqual(_world.ActualPlayer.Coins, 40);
        }

        [TestMethod]
        public void PlaceRangeTowerTest()
        {
            var result = _world.CreateTower(4, 4, "RangeTower");
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.ErrorMessage, "Nincs eleg penzed!");
            Assert.AreEqual(_world.Tiles[4, 4].Type, TileType.Occupied);
            Assert.AreEqual(_world.ActualPlayer.Coins, 35);
        }

        public void PlaceCannonTowerTest()
        {
            var result = _world.CreateTower(4, 4, "CannonTower");
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.ErrorMessage, "Nincs eleg penzed!");
            Assert.AreEqual(_world.Tiles[4, 4].Type, TileType.Occupied);
            Assert.AreEqual(_world.ActualPlayer.Coins, 30);
        }

        [TestMethod]
        public void PlaceSimpleUnitTest()
        {
            var result = _world.CreateUnit("SimpleUnit");
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.ErrorMessage, "Nincs eleg penzed!");
            Assert.AreEqual(_world.ActualPlayer.Coins, 40);
        }

        [TestMethod]
        public void PlaceStrongUnitTest()
        {
            var result = _world.CreateUnit("StrongUnit");
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.ErrorMessage, "Nincs eleg penzed!");
            Assert.AreEqual(_world.ActualPlayer.Coins, 30);
        }

        [TestMethod]
        public void WrongPlaceTowerTest()
        {
            _world.CreateTower(7, 3, "RangeTower");
            Assert.AreEqual(_world.ActualPlayer.Coins, 35);

            var result = _world.CreateTower(7, 3, "RangeTower");
            Assert.AreEqual(_world.ActualPlayer.Coins, 35);
            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.ErrorMessage, "Torony nem helyezheto erre a feluletre!");
        }

        [TestMethod]
        public void ShootTest()
        {
            var myTower = new SimpleTower(_world.Tiles[8, 18], Faction.Horde);
            var myTower2 = new SimpleTower(_world.Tiles[2, 14], Faction.Horde);

            _world.CreateUnit("SimpleUnit");

            myTower.Shoot();
            myTower2.Shoot();

            Assert.AreEqual(_world.Units[0].Health, 19);
        }

        [TestMethod]
        public void TurnTest()
        {
            Assert.AreEqual(_world.ActualPlayer.Faction, Faction.Alliance);
            _world.Turn();
            Assert.AreEqual(_world.ActualPlayer.Faction, Faction.Horde);
        }

        [TestMethod]
        public void GetCoinTest()
        {
            for(var i = 0; i < 5; ++i)
            {
                _world.CreateUnit("SimpleUnit");
            }
            Assert.AreEqual(_world.ActualPlayer.Coins, 0);

            _world.Turn();

            for (var i = 0; i < 5; ++i)
            {
                _world.CreateUnit("SimpleUnit");
            }
            Assert.AreEqual(_world.ActualPlayer.Coins, 0);

            _world.Turn();
            Assert.AreEqual(_world.ActualPlayer.Coins, 5);
            _world.Turn();
            Assert.AreEqual(_world.ActualPlayer.Coins, 5);
        }

        [TestMethod]
        public void CastleHealthTest()
        {
            _world.CreateUnit("SimpleUnit");

            for(var i = 0; i < 99; ++i)
            {
                _world.Turn();
            }

            Assert.AreEqual(_world.ActualPlayer.Faction, Faction.Horde);
            Assert.AreEqual(_world.ActualPlayer.Castle.Health, 195);
        }

        [TestMethod]
        public void ExistingPathTest()
        {
            _world.CreateTower(3, 15, "SimpleTower");
            Assert.AreEqual(_world.Tiles[3, 15].Type, TileType.Occupied);

            _world.CreateTower(3, 17, "SimpleTower");
            Assert.AreEqual(_world.Tiles[3, 17].Type, TileType.Occupied);

            _world.CreateTower(4, 16, "SimpleTower");
            Assert.AreEqual(_world.Tiles[4, 16].Type, TileType.Occupied);

            var result = _world.CreateTower(2, 16, "SimpleTower");
            Assert.AreEqual(_world.Tiles[2, 16].Type, TileType.Enterable);
            Assert.AreEqual(_world.ActualPlayer.Coins, 20);

            Assert.AreEqual(result.Success, false);
            Assert.AreEqual(result.ErrorMessage, "Torony nem helyezheto erre a feluletre!");
        }

        [TestMethod]
        public void EndGameTest()
        {
            _world.CreateUnit("SimpleUnit");
            _world.Turn();
            _world.ActualPlayer.Castle.Health = 1;

            for (var i = 0; i < 99; ++i)
            {
                _world.Turn();
            }

            Assert.AreEqual(_world.IsEnd, true);
            Assert.AreEqual(_world.EndingString, "Win:Alliance");
        }
    }
}
