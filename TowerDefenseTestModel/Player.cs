using System;

public class Player
{
    //Játékos osztálya.
    public int Coins { get; set; } //Pénzmennyisége.
    public Faction Faction { get; private set; } //Faction-je.
    public Castle Castle { get; set; } //Kastélya.
    public Barrack[] Barracks { get; set; } //Barakkjai.

    public Player(Faction faction) //Konstruktor.
    {
        Faction = faction;
        Coins = 50;
        Barracks = new Barrack[2];
    }
}
