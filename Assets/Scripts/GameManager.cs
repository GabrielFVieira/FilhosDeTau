using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public string objetosalvo;
    public string caminho;
    public GameObject player;

    public SaveGame save;

    public List<KeyCode> buttons = new List<KeyCode>();
    public Button[] keyButtons;
    public Text[] keyButtonsText;

    public bool changeActive;
    public int i;
    public GameObject buttonWarning;
    // Use this for initialization
    void Start () {

        buttonWarning.SetActive(false);
        if (GameObject.FindObjectsOfType<GameManager>().Length > 1)
            Destroy(gameObject);

        else
            DontDestroyOnLoad(this.gameObject);

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        else
        {
            player = null;
        }

        LoadState();
    }
	
	// Update is called once per frame
	void Update () {
        if(player == null && GameObject.FindGameObjectWithTag("Player") != null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
                SaveState(false);
                player = null;
                SceneManager.LoadScene("Menu");
        }
        if (Input.GetKeyDown(KeyCode.R) && player != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(changeActive)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyUp(key))
                {
                    if (buttons.Contains(key) == false || buttons[i] == key)
                    {
                        buttons[i] = key;
                        keyButtonsText[i].text = key.ToString();
                        foreach (Button b in keyButtons)
                            b.interactable = true;
                        changeActive = false;

                        buttonWarning.SetActive(false);
                    }

                    else
                        buttonWarning.SetActive(true);
                }
            }
        }
    }
    public void ChangeButton(int buttonIndex)
    {
        if (keyButtons[buttonIndex].interactable)
        {
            if (!changeActive)
            {
                foreach (Button b in keyButtons)
                    b.interactable = false;

                i = buttonIndex;
                keyButtonsText[i].text = "Press any button";
                changeActive = true;
            }
        }
    }

    public void SaveState(bool start)
    {
        save = new SaveGame();
        if (start == true) // Reset variables
        {
            player.GetComponent<PlayerMovement>().ammo = player.GetComponent<PlayerMovement>().maxAmmo;       
            player.transform.position = new Vector3();
            player.GetComponent<PlayerHealth>().curHealth = player.GetComponent<PlayerHealth>().maxHealth;
            player.GetComponent<EnergyBar>().curEnergy = player.GetComponent<EnergyBar>().maxEnergy;
        }
        save.ammo = player.GetComponent<PlayerMovement>().ammo;
        save.playerPos = player.transform.position;
        save.playerHealth = player.GetComponent<PlayerHealth>().curHealth;
        save.playerEnergy = player.GetComponent<EnergyBar>().curEnergy;

        string caminho = Path.Combine(Application.persistentDataPath, "savegame.dat");
        string objetosalvo = JsonUtility.ToJson(save, true);
        File.WriteAllText(caminho, objetosalvo);
    }

    public void LoadGame()
    {
         SceneManager.LoadScene("Main");
         StartCoroutine(WaitLoadScene(false));
    }

    public void LoadState()
    {
        string caminho = Path.Combine(Application.persistentDataPath, "savegame.dat");
        string texto = File.ReadAllText(caminho);
        save = JsonUtility.FromJson<SaveGame>(texto);
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Main");
        StartCoroutine(WaitLoadScene(true));
    }

    IEnumerator WaitLoadScene(bool newG)
    {
        AsyncOperation asyncLoadLevel;
        asyncLoadLevel = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            yield return null;
        }
        if(newG == true) // Reset
            SaveState(true);

        if(newG == false) // Load
        {
            player.GetComponent<PlayerMovement>().ammo = save.ammo;
            player.transform.position = save.playerPos;
            player.GetComponent<PlayerHealth>().curHealth = save.playerHealth;
            player.GetComponent<EnergyBar>().curEnergy = save.playerEnergy;
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(save.playerPos.x, save.playerPos.y, GameObject.FindGameObjectWithTag("MainCamera").transform.position.z);
        }
    }
}
