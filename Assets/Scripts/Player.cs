using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public float speed = 5f;
    public int gold;
    public int corruption;
    public int suspicion;
    public int health;

    int maxSuspicion = 100;
    int maxHealth = 100;

    public Text goldText;
    public Text corruptionText;
    public Image SuspicionBar;
    public Image HealthBar;

    public GameObject InteractBox;
    public GameObject InteractBox2;
    public GameObject victim;

    Rigidbody2D rb2d;

    Vector2 moveVelocity;
    GameObject Interactable;

    // Use this for initialization
    void Start () {
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        gold = 0;
        corruption = 0;
        health = 100;
        suspicion = 0;
        victim = null;

        SuspicionBar.fillAmount = (float)suspicion / (float)maxSuspicion;
        HealthBar.fillAmount = (float)health / (float)maxHealth;

        goldText.text = "GOLD: " + gold;
        corruptionText.text = "CORRUPTION: " + corruption;
    }
	
	void Update () {
        //MOVEMENT
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        //INTERACTIONS
        if (Input.GetKeyDown("g"))
        {
            if (Interactable.tag == "NPC" && gold >= Interactable.GetComponent<Npc>().bribeCost)
            {
                victim = Interactable;
                victim.GetComponent<Npc>().getFollowing();
                decreaseGold(victim.GetComponent<Npc>().bribeCost);
                InteractBox2.SetActive(false);
                InteractBox.GetComponentInChildren<Text>().text = "Press 'F' to dismiss";
                InteractBox.SetActive(true);
            }
            else Debug.Log("Not Enough Gold");

            if (Interactable.tag == "Altar")
            {
                decreaseHealth(Interactable.GetComponent<Altar>().bloodPactCost);
                increaseCorruption(Interactable.GetComponent<Altar>().soulValue);
                if (health <= Interactable.GetComponent<Altar>().bloodPactCost) InteractBox2.GetComponentInChildren<Text>().color = Color.red;//new Color(255f, 20f, 20f);
            }
        }

        if (Input.GetKeyDown("f"))
        {
            if (victim != null && Interactable == null)
            {
                victim.GetComponent<Npc>().getReleased();
                InteractBox.GetComponentInChildren<Text>().text = "Press 'F' to kidnap";
                InteractBox.SetActive(true);
                if (gold <= victim.GetComponent<Npc>().bribeCost) InteractBox2.GetComponentInChildren<Text>().color = Color.red; else InteractBox2.GetComponentInChildren<Text>().color = Color.white;
                InteractBox2.GetComponentInChildren<Text>().text = "Press 'G' to Bribe ("+ victim.GetComponent<Npc>().bribeCost + " Gold)";
                InteractBox2.SetActive(true);
                victim = null;
                return;
            }

            if (Interactable.tag != null)
            {
                if (Interactable.tag == "NPC" && victim == null) //On and NPC with no victim
                {
                    victim = Interactable;
                    victim.GetComponent<Npc>().getKidnapped();
                    InteractBox2.SetActive(false);
                    InteractBox.GetComponentInChildren<Text>().text = "Press 'F' to release";
                    InteractBox.SetActive(true);
                    Interactable = null;
                    return;
                }

                if (Interactable.tag == "Altar") //On an Altar
                {
                    if (victim != null)
                    {
                        Destroy(victim);
                        victim.GetComponent<Npc>().kill();
                        increaseCorruption(victim.GetComponent<Npc>().soulValue);
                        increaseGold(victim.GetComponent<Npc>().goldValue);
                        victim = null;
                        InteractBox.SetActive(false);
                    }
                    return;
                }
            }
            else Debug.Log("Nothin here");
        } 
    }

    private void FixedUpdate()
    {
        rb2d.MovePosition(rb2d.position + moveVelocity * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable = collision.gameObject;
        Debug.Log(Interactable);
        if (Interactable.tag == "NPC" && victim == null)
        {
            InteractBox.GetComponentInChildren<Text>().text = "Press 'F' to kidnap";
            InteractBox.SetActive(true);
            if (gold <= Interactable.GetComponent<Npc>().bribeCost) InteractBox2.GetComponentInChildren<Text>().color = Color.red; else InteractBox2.GetComponentInChildren<Text>().color = Color.white;
            InteractBox2.GetComponentInChildren<Text>().text = "Press 'G' to Bribe (" + Interactable.GetComponent<Npc>().bribeCost + " Gold)";
            InteractBox2.SetActive(true);
        }

        if (Interactable.tag == "Altar")
        {
            if (victim != null) { InteractBox.GetComponentInChildren<Text>().text = "Press 'F' to sacrifice"; }
            else { InteractBox.GetComponentInChildren<Text>().text = "It needs a sacrifice"; }
            InteractBox.SetActive(true);
            if (health <= Interactable.GetComponent<Altar>().bloodPactCost) InteractBox2.GetComponentInChildren<Text>().color = Color.red; else InteractBox2.GetComponentInChildren<Text>().color = Color.white;
            InteractBox2.GetComponentInChildren<Text>().text = "Press 'G' for Blood Pact (" + Interactable.GetComponent<Altar>().bloodPactCost + " HP)";
            InteractBox2.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (victim == null) { InteractBox.SetActive(false); }
        else { InteractBox.GetComponentInChildren<Text>().text = "Press 'F' to release"; }

        if (Interactable != null)
        {
            if (Interactable.tag == "NPC" && victim == null)
            {
                InteractBox.GetComponentInChildren<Text>().text = "Press 'F' to kidnap";
                InteractBox.SetActive(true);
                InteractBox2.GetComponentInChildren<Text>().text = "Press 'G' to Bribe (" + Interactable.GetComponent<Npc>().bribeCost + " Gold)";
                InteractBox2.SetActive(true);
            }

            if (Interactable.tag == "Altar")
            {
                if (victim != null) { InteractBox.GetComponentInChildren<Text>().text = "Press 'F' to sacrifice"; }
                else { InteractBox.GetComponentInChildren<Text>().text = "It needs a sacrifice"; }
                InteractBox.SetActive(true);
                InteractBox2.SetActive(false);
            }
        }

        InteractBox.SetActive(false);
        InteractBox2.SetActive(false);
        Interactable = null;
    }

    public void increaseSuspicion(int sus)
    {
        suspicion+=sus;
        if (suspicion > maxSuspicion)suspicion = maxSuspicion;
        SuspicionBar.fillAmount = (float)suspicion / (float)maxSuspicion;
    }

    void decreaseSuspicion(int sus)
    {
        suspicion -= sus;
        if (suspicion < 0) suspicion = 0;
        SuspicionBar.fillAmount = (float)suspicion / (float)maxSuspicion;
    }

    void increaseHealth(int hp)
    {
        health += hp;
        if (health > maxHealth) health = maxHealth;
        HealthBar.fillAmount = (float)health / (float)maxHealth;
    }

    public void decreaseHealth(int hp)
    {
        health -= hp;
        if (health <= 0)
        {
            health = 0;
            die();
        }else HealthBar.fillAmount = (float)health / (float)maxHealth;
    }

    void increaseCorruption(int cor)
    {
        corruption += cor;
        corruptionText.text = "CORRUPTION: " + corruption;
    }

    void decreaseCorruption(int cor)
    {
        corruption -= cor;
        corruptionText.text = "CORRUPTION: " + corruption;
    }

    void increaseGold(int g)
    {
        gold += g;
        goldText.text = "GOLD: " + gold;
    }

    void decreaseGold (int g)
    {
        gold -= g;
        goldText.text = "GOLD: " + gold;
    }

    void die()
    {
        SceneManager.LoadScene("Death_Screen");
    }
}
