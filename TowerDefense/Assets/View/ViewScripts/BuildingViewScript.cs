class BuildingViewScript : ViewScript<Building>
{
    //Épületek sprite-jainak frissítése.
    protected override void Refresh()
    {
        UpdatePosition(); //Pozíció frissítése.

        //Tornyok.
        if (Target is SimpleTower && Target.Faction is Faction.Horde)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("HordeBuildings_36");
        }
        else if (Target is SimpleTower && Target.Faction is Faction.Alliance)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("AllianceBuildings_4");
        }
        else if (Target is RangeTower && Target.Faction is Faction.Horde)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("HordeBuildings_42");
        }
        else if (Target is RangeTower && Target.Faction is Faction.Alliance)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("AllianceBuildings_18");
        }
        else if (Target is CannonTower && Target.Faction is Faction.Horde)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("HordeBuildings_21");
        }
        else if (Target is CannonTower && Target.Faction is Faction.Alliance)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("AllianceBuildings_27");
        }
        else if (Target is Barrack && Target.Faction is Faction.Horde)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("HordeBuildings_5");
        }
        else if (Target is Barrack && Target.Faction is Faction.Alliance)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("AllianceBuildings_42");
        }

        //Kastély, életerőtől függően.
        else if (Target is Castle && Target.Faction is Faction.Horde && Target.Health >= 75)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("HordeCastle_0");
        }
        else if (Target is Castle && Target.Faction is Faction.Horde && Target.Health >= 50 && Target.Health < 75)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("HordeCastle_1");
        }
        else if (Target is Castle && Target.Faction is Faction.Horde && Target.Health >= 25 && Target.Health < 50)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("HordeCastle_2");
        }
        else if (Target is Castle && Target.Faction is Faction.Horde && Target.Health >= 0 && Target.Health < 25)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("HordeCastle_3");
        }

        else if (Target is Castle && Target.Faction is Faction.Alliance && Target.Health >= 75)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("AllianceCastle_0");
        }
        else if (Target is Castle && Target.Faction is Faction.Alliance && Target.Health >= 50 && Target.Health < 75)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("AllianceCastle_1");
        }
        else if (Target is Castle && Target.Faction is Faction.Alliance && Target.Health >= 25 && Target.Health < 50)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("AllianceCastle_2");
        }
        else if (Target is Castle && Target.Faction is Faction.Alliance && Target.Health >= 0 && Target.Health < 25)
        {
            SpriteRenderer.sprite = SpriteManager.GetSprite("AllianceCastle_3");
        }
    }

    private new void Awake() //Ébredés, létrehozáskor hívódik meg.
    {
        base.Awake();

        SpriteRenderer.sortingLayerName = "Entity"; //Unity Layer.
    }
}
