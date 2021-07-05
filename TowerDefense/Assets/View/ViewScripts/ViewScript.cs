using UnityEngine;

public abstract class ViewScript<T> : MonoBehaviour where T : IDisplayable
{
    //Megjelenített objektumok ősosztálya.
    protected SpriteRenderer SpriteRenderer { get; set; } //Sprite hozzárendeléséhez szükséges.

    protected T Target { get; set; } //Adott objektum.

    public virtual void SetTarget(T target) //Frissítés.
    {
        if (Target != null)
        {
            Target.Changed -= Refresh;
        }

        Target = target;
        Target.Changed += Refresh;
        Refresh();
    }
    
    protected void Awake() //Ébredés, létrehozáskor hívódik meg.
    {
        SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    protected abstract void Refresh(); //Felülírva az alosztályokban.

    protected virtual void UpdatePosition() //Pozíció felülírása.
    {
        gameObject.transform.position = new Vector3(Target.X, Target.Y, 0);
    }
}