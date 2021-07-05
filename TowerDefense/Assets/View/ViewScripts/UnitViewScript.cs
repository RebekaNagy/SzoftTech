using System.Globalization;
using UnityEngine;

class UnitViewScript : ViewScript<Unit>
{
    //Egységek spritejainak kezelése.
    protected override void UpdatePosition()
    {
        //Pozíció frissítése, ha több unit van egy tile-on, akkor sorszámától függő lehelyezés.
        if (Target.Number == 1)
        {
            gameObject.transform.position = new Vector3(Target.X, Target.Y, 0);
        }
        else
        {
            switch (Target.Number % 4)
            {
                case 0:
                    gameObject.transform.position = new Vector3(Target.X + (1 / Target.Number) * 0.4f,
                        Target.Y + (1 / Target.Number) * 0.4f, 0);
                    break;
                case 1:
                    gameObject.transform.position = new Vector3(Target.X + (1 / Target.Number) * 0.4f,
                        Target.Y - (1 / Target.Number) * 0.4f, 0);
                    break;
                case 2:
                    gameObject.transform.position = new Vector3(Target.X - (1 / Target.Number) * 0.4f,
                        Target.Y + (1 / Target.Number) * 0.4f, 0);
                    break;
                case 3:
                    gameObject.transform.position = new Vector3(Target.X - (1 / Target.Number) * 0.4f,
                        Target.Y - (1 / Target.Number) * 0.4f, 0);
                    break;
            }
        }
    }
    protected override void Refresh()
    {
        UpdatePosition(); //Pozíció frissítése.

        if (Target is SimpleUnit && Target.Faction is Faction.Horde)
        {
            if (Target.Health >= 18)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("SimpleHor_0");
            }
            else if (Target.Health >= 12 && Target.Health < 18)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("SimpleHor_1");
            }
            else if (Target.Health >= 6 && Target.Health < 12)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("SimpleHor_2");
            }
            else if (Target.Health >= 0 && Target.Health < 6)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("SimpleHor_3");
            }
        }
        else if (Target is SimpleUnit && Target.Faction is Faction.Alliance)
        {
            if (Target.Health >= 18)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("SimpleAll_0");
            }
            else if (Target.Health >= 12 && Target.Health < 18)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("SimpleAll_1");
            }
            else if (Target.Health >= 6 && Target.Health < 12)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("SimpleAll_2");
            }
            else if (Target.Health >= 0 && Target.Health < 6)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("SimpleAll_3");
            }
        }
        else if (Target is StrongUnit && Target.Faction is Faction.Horde)
        {
            if (Target.Health >= 33)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("StrongHor_0");
            }
            else if (Target.Health >= 22 && Target.Health < 33)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("StrongHor_1");
            }
            else if (Target.Health >= 11 && Target.Health < 22)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("StrongHor_2");
            }
            else if (Target.Health >= 0 && Target.Health < 11)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("StrongHor_3");
            }
        }
        else if (Target is StrongUnit && Target.Faction is Faction.Alliance)
        {
            if (Target.Health >= 33)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("StrongAll_0");
            }
            else if (Target.Health >= 22 && Target.Health < 33)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("StrongAll_1");
            }
            else if (Target.Health >= 11 && Target.Health < 22)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("StrongAll_2");
            }
            else if (Target.Health >= 0 && Target.Health < 11)
            {
                SpriteRenderer.sprite = SpriteManager.GetSprite("StrongAll_3");
            }
        }
    }


    private new void Awake() //Ébredés, létrehozáskor hívódik meg.
    {
        base.Awake();

        SpriteRenderer.sortingLayerName = "Entity";
    }
}
