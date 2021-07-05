using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera Camera; //Kamera objektum.

    //Kamera mozgatásához szükséges paraméterek.
    public int Border = 10; 
    public Vector2 LowerLimits = new Vector2(0, 0); 
    public float Speed = 20f;
    public Vector2 UpperLimits = new Vector2(19, 19);

    //Indításnál a kamerát létrehozzuk és adott pozícióra helyezzük.
    private void Start()
    {
        Camera = GetComponent<Camera>();
        transform.position = new Vector3(10, 10, -10);
    }
    
    //Folyamatos frissítés függvénye.
    private void Update()
    {
        // Egérpozíció lekérése.
        var position = transform.position;

        // Játékfelület megfelelő oldalának kiválasztása.
        if (Input.mousePosition.x >= Screen.width - Border)
        {
            position.x += Speed * Time.deltaTime;
        }

        if (Input.mousePosition.x <= Border)
        {
            position.x -= Speed * Time.deltaTime;
        }

        if (Input.mousePosition.y >= Screen.height - Border)
        {
            position.y += Speed * Time.deltaTime;
        }

        if (Input.mousePosition.y <= Border)
        {
            position.y -= Speed * Time.deltaTime;
        }

        //Pozíció frissítése.
        position.x = Mathf.Clamp(position.x, LowerLimits.x, UpperLimits.x);
        position.y = Mathf.Clamp(position.y, LowerLimits.y, UpperLimits.y);

        transform.position = position;
    }
}
