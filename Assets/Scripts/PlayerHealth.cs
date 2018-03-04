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

    // Use this for initialization
    void Start()
    {
        maxHealth = 100;
        curHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        calcHealth = curHealth / maxHealth;
        healthBar.transform.localScale = new Vector3(calcHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

	// Update is called once per frame
	void Update () {
        if (curHealth < 0)
            curHealth = 0;

        if (curHealth == 0)
        {
            GetComponent<Animator>().SetBool("Died", true);
            healthBarCanvas.SetActive(false);
        }
	}

    public void TakeDamage(int dano)
    {
        curHealth -= dano;
    }
}
