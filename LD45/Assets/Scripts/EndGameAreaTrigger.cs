using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameAreaTrigger : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        Player p = col.GetComponent<Player>();
        if (p)
        {
            Camera.main.GetComponent<ScreenFX>().SetEndGame(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Player p = col.GetComponent<Player>();
        if (p)
        {
            Camera.main.GetComponent<ScreenFX>().SetEndGame(false);
        }
    }

}
