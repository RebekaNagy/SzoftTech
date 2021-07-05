using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class EndingLabel : MonoBehaviour
{
    public Text Winner; //Játék végén megjelenő szöveg kiírásához.

    void Update()
    {
        Winner.text = World.Current.EndingString; //Folyamatos frissítés, a modellből kapjuk meg a szöveget.
    }

    public void NewGameButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4); //Az oldalon megjelenő Új játék gomb megnyomásának hatására a kezdőoldalra kerülünk.
    }
}

