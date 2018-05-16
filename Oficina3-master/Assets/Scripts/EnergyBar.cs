using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour {
    public float curEnergy;
    public float maxEnergy;

    [SerializeField]
    private GameObject energyBar;

    [SerializeField]
    private GameObject energyBarCanvas;

    private float calcEnergy;

    private bool recharge;
    private float rechargeTimer;
    private float rechargeMaxTimer;
    // Use this for initialization
    void Start()
    {
        maxEnergy = 50;
        curEnergy = maxEnergy;

        rechargeMaxTimer = 3;
    }

    private void FixedUpdate()
    {
        calcEnergy = curEnergy / maxEnergy;
        energyBar.transform.localScale = new Vector3(calcEnergy, energyBar.transform.localScale.y, energyBar.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (curEnergy < 0)
            curEnergy = 0;

        if (curEnergy > maxEnergy)
            curEnergy = maxEnergy;

        else if (curEnergy < maxEnergy)
        {
            recharge = true;
        }

        else
        {
            recharge = false;
            rechargeTimer = 0;
        }

        if(recharge)
        {
            rechargeTimer += Time.deltaTime;

            if(rechargeTimer >= rechargeMaxTimer && curEnergy < maxEnergy)
            {
                curEnergy += 5 * Time.deltaTime;
            }
        }

        if(Input.GetKey(GetComponent<PlayerMovement>().runButton) && GetComponent<PlayerMovement>().canRun && GetComponent<Animator>().GetBool("isWalking") || GetComponent<Animator>().GetBool("Roll"))
        {
            rechargeTimer = 0;
        }
    }

}
