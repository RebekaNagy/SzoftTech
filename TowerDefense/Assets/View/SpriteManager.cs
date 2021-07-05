using System.Collections.Generic;
using UnityEngine;

public static class SpriteManager
{
    private static readonly Dictionary<string, Sprite> Sprites;

    static SpriteManager() //Projecthez rendelt sprite-ok betöltése, és listába helyezése.
    {
        Sprites = new Dictionary<string, Sprite>();
        var spriteArray = Resources.LoadAll<Sprite>("");
        foreach (var sprite in spriteArray)
        {
            if (!Sprites.ContainsKey(sprite.name))
            {
                Sprites.Add(sprite.name, sprite);
            }
        }
    }

    public static Sprite GetSprite(string spriteName) //Getter.
    {
        return Sprites[spriteName];
    }
}