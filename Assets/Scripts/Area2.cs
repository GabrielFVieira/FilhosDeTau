using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area2 : MonoBehaviour {
    public RagDool[] ragdolls;
    public string[] colors;
    public int index;
    public List<int> validChoices = new List<int>();
    public int[] order;

    public TutorialManager tutorial;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < order.Length; i++)
        {
            int x = Random.Range(0, validChoices.Count);
            order[i] = validChoices[x];
            validChoices.RemoveAt(x);
        }

        Debug.Log("Ordem: " + colors[order[0]] + ", " + colors[order[1]] + ", " + colors[order[2]] + ", " + colors[order[3]] + ", " + colors[order[4]] + ", " + colors[order[5]]);
	}
	
	// Update is called once per frame
	void Update () {
        if (index != 5)
        {
            foreach (RagDool r in ragdolls)
            {
                if (r.attack)
                {
                    index++;
                    r.attack = false;
                    r.controle = true;
                }
            }
        }
        if (index >= 0)
        {
            if (!ragdolls[order[index]].controle)
            {
                index = -1;
                Debug.Log("Errou");
                foreach (RagDool r in ragdolls)
                {
                    r.controle = false;
                    r.attack = false;
                }
            }
        }

        if (index >= 5 && !tutorial.partCompleted[2])
        {
            tutorial.partCompleted[2] = true;
            Debug.Log("Passou");
        }
	}
}
