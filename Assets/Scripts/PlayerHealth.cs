using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    public float curHealth;
    private float maxHealth;

    [SerializeField]
    private GameObject healthBar;

    [SerializeField]
    private GameObject healthBarCanvas;

    private float calcHealth;

    private float invTimer;
    private bool control;
    // Use this for initialization
    void Start()
    {
        control = true;
        maxHealth = 100;
        curHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        calcHealth = curHealth / maxHealth;
        healthBar.transform.localScale = new Vector3(calcHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        healthBarCanvas.GetComponent<Canvas>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
    }

	// Update is called once per frame
	void Update () {
        if (curHealth == maxHealth || curHealth == 0 || control)
            healthBarCanvas.SetActive(false);

        else
        {
            healthBarCanvas.SetActive(true);
            control = false;
        }

        if (curHealth < 0)
            curHealth = 0;

        if (curHealth == 0)
        {
            GetComponent<Animator>().SetBool("Died", true);
        }

        if (control == false)
        {
            invTimer += Time.deltaTime;
            if (invTimer > 10)
            {
                control = true;
                invTimer = 0;
            }
        }

        if (GetComponent<PlayerMovement>().colGO == null)
            invTimer = 0;
	}

    public void TakeDamage(int dano)
    {
        curHealth -= dano;
        control = false;
        invTimer = 0;
    }
}
