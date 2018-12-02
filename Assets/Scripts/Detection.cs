using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{

    public Npc NPC;

    private void Start()
    {
        gameObject.GetComponent<CircleCollider2D>().radius = NPC.detectionRange;
    }

    private void Update()
    {
        transform.position = NPC.transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("see dude");
            if (collision.GetComponent<linker>().player.GetComponent<Player>().victim != null)
            {
                collision.GetComponent<linker>().player.GetComponent<Player>().increaseSuspicion(NPC.suspicionValue);
                Debug.Log("see dude baaaddd");
            }
        }
    }
}
