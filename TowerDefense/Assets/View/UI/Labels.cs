using UnityEngine;
using UnityEngine.UI;

public class Labels : MonoBehaviour
{
    public Text actPlayer; //Aktuális játékos nevének kiíratáshoz szükséges.
    public Text coins; //Aktuális játékos pénzének kiíratásához szükséges.

    void Update()
    {
        //Ha a játékos jelenleg Alliance, akkor az ő nevét írjuk ki, szerver esetén azt is melléírva, hogy az ő köre tart-e.
        if (World.Current.ActualPlayer.Faction == Faction.Alliance)
        {
            if (World.Current.CanStep)
            {
                actPlayer.text = "Alliance\n\n It's your turn!";
            }
            else
            {
                actPlayer.text = "Alliance\n\n Your buttons are blocked. It's the other player's turn!";
            }
        }
        else
        { // Ugyanez, csak Horde-al.
            if (World.Current.CanStep)
            {
                actPlayer.text = "Horde\n\n It's your turn!";
            }
            else
            {
                actPlayer.text = "Horde\n\n Your buttons are blocked. It's the other player's turn!";
            }
        }
        //Játékos pénzmennyiségét a Worldből kapjuk meg.
        coins.text = World.Current.ActualPlayer.Coins.ToString();
    }
}
