using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linker : MonoBehaviour {

    public Player player;

    void Update()
    {

        transform.position = player.transform.position;
        
    }
}
