using UnityEngine;

public class MouseController : MonoBehaviour
{ 
    //Egér kattintásának kezelése.
    private int CurrentX { get; set; } //Pozíciók.
    private int CurrentY { get; set; }

    public static bool TowerButtonClicked; //Toronylehelyezéshez szükséges paraméterek.

    public static string actualTower;

    public static bool ButtonClicking;

    private void Update()
    {
        //Aktuális pozíció lekérése.
        var currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CurrentX = Mathf.RoundToInt(currentPosition.x);
        CurrentY = Mathf.RoundToInt(currentPosition.y);

        CurrentX = Mathf.Clamp(CurrentX, 0, 19);
        CurrentY = Mathf.Clamp(CurrentY, 0, 19);

        //Kattintás.
        if (Input.GetMouseButtonUp(0) && TowerButtonClicked is true && ButtonClicking is false)
        {
            var result = World.Current.CreateTower(CurrentX, CurrentY, actualTower); //Torony létrehozásának meghívása.
            
            TowerButtonClicked = false; //Blockolás.
            actualTower = "";
        }

        ButtonClicking = false;
    }
}