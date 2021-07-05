using UnityEngine;

public sealed class TileViewScript : ViewScript<Tile>
{
    //Tile-ok spritejainak kezelése.
    protected override void Refresh()
    {
        UpdatePosition(); //pozíció
        
        SpriteRenderer.sprite = SpriteManager.GetSprite("Tiles_" + SpriteMaps.ActualMap[Target.X, Target.Y]); //Sprite beolvasása kiválasztott térképből.
    }

    private new void Awake() //Ébredés, létrehozáskor hívódik meg.
    {
        base.Awake();

        SpriteRenderer.sortingLayerName = "Tile";
    }
}