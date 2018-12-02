using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour {

    bool kidnapped = false;
    bool following = false;

    public GameObject npcmain;

    public Player player;
    public int soulValue;
    public int goldValue;
    public int bribeCost;
    public int suspicionValue;
    public float detectionRange;

    private void Update()
    {
        if (kidnapped==true && player!=null)
        {
            this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y+0.1f, player.transform.position.z);
        }

        if (following == true && player != null)
        {
            this.transform.position = new Vector3(player.transform.position.x+0.2f, player.transform.position.y, player.transform.position.z);
        }
    }

    public void getKidnapped()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 90);
        this.kidnapped = true;
    }

    public void kill()
    {
        Destroy(npcmain);
    }

    public void getFollowing()
    {
        this.following = true;
    }

    public void getReleased()
    {
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        this.kidnapped = false;
        this.following = false;
    }
}
