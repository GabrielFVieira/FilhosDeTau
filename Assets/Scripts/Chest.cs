using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    public bool opened;
    public bool showItens;
    public bool recentlyOpened;

    [SerializeField]
    private GameObject itenContainer;

    private List<GameObject> itens = new List<GameObject>();

    [SerializeField]
    private InvetoryManager invManager;
	// Use this for initialization
	void Start () {
        itenContainer.SetActive(showItens);
	}
	
	// Update is called once per frame
	void Update () {
        itenContainer.SetActive(showItens);

    }
}
