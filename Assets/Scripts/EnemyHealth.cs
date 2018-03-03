using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public float curHealth;
    public float maxHealth;

    [SerializeField]
    private GameObject healthBar;
    private float initialSize;

    [SerializeField]
    private GameObject healthBarCanvas;

    private float calcHealth;

    private float maxDistInv;
    // Use this for initialization
    void Start () {
        maxHealth = 100;
        curHealth = maxHealth;

        healthBarCanvas.SetActive(false);

        maxDistInv = 5;
    }

    private void FixedUpdate()
    {
        calcHealth = curHealth / maxHealth;
        healthBar.transform.localScale = new Vector3(calcHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    // Update is called once per frame
    void Update () {
        if(curHealth < maxHealth)
            healthBarCanvas.SetActive(true);


        if (curHealth < 0)
        {
            curHealth = 0;
        }

        if (curHealth != 0)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player.transform.position.x > transform.position.x + maxDistInv || player.transform.position.x < transform.position.x - maxDistInv || player.transform.position.y > transform.position.y + maxDistInv || player.transform.position.y < transform.position.y - maxDistInv)
            {
                healthBarCanvas.SetActive(false);
            }

            else
            {
                healthBarCanvas.SetActive(true);
            }
        }

        else
            healthBarCanvas.SetActive(false);
    }

    public void TakeDamage(int dano)
    {
        curHealth -= dano;
    }
}
