using System;

public interface IDisplayable //Unity osztály, megjelenítéshez szükséges.
{
    event Action Changed;

    int X { get; }
    int Y { get; }

    void OnChange();
}