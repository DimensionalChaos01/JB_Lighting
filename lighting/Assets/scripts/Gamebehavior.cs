using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class gamebehavior : MonoBehaviour
{
    public bool showwinscreen = false;
    private int _itemscollected = 0;
    private int _playerhp = 20;
    public bool get;
    public bool set;
    public int _jetpack = 0;

    public string labeltext = "Collect all 4 items and win your freedom!";
    public int maxItems = 4;
    public Transform player;
    public GameObject enemy;

    public Stack<string> lootstack = new Stack<string>();

    public bool showLossScreen = false;

    private string _state;

    public delegate void DebugDelegate(string newText);
    public DebugDelegate debug = Print;

    public string State
    {
        get { return _state; }
        set { _state = value; }
    }
    void Start()
    {

    }

    public void Initialize()
    {
        _state = "Manager initialized...";        Debug.Log(_state);

        lootstack.Push("Sword of doom");
        lootstack.Push("HP+");
        lootstack.Push("Golden Key");
        lootstack.Push("Winged Boots");
        lootstack.Push("Mythril bracers");

        debug(_state);
        LogWithDelegate(debug);

        GameObject player = GameObject.Find("Player");
        playermove playerBehavior = player.GetComponent<playermove>();
        playerBehavior.playerJump += HandlePlayerJump;
    }

    public void HandlePlayerJump()
    {
        debug("Player has jumped...");
    }

    public static void Print(string newText)
    {
        Debug.Log(newText);
    }

    public void LogWithDelegate(DebugDelegate del)
    {
        del("Delegating the Debug task...");
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
    public int Items
    {
        get
        {
            return _itemscollected;
        }
        set
        {
            _itemscollected = value;
            if (_itemscollected >= maxItems)
            {
                labeltext = "You found them all!";

                showwinscreen = true;

                Time.timeScale = 0F;
            }
            else
            {
                labeltext = "You found an item! only " + (maxItems - _itemscollected) + " More to go!";
            }
            Debug.LogFormat("Items: {0}", _itemscollected);
        }
    }
    public int playerhealth
    {
        get { return _playerhp; }
        set
        {
            _playerhp = value;
            Debug.LogFormat("Jetpack: {0}", _playerhp);
            if (_playerhp <= 0)
            {
                Debug.Log("health is at or less than 0");
                showLossScreen = true;
                Time.timeScale = 0;
            }

            if (_playerhp == 0)
            {
                Debug.Log("health is at or less than 0");
                showLossScreen = true;
                Time.timeScale = 0;
            }

            else
            {
                labeltext = "Ouch, that hurt";
            }
        }
    }
    public int jetpack
    {
        get { return _jetpack; }
        set
        {
            _jetpack = value;
            Debug.LogFormat("Jetpack: {0}", _jetpack);
        }
    }

    public int HP
    {
        get { return _playerhp; }
        set
        {
            _playerhp = value;
            Debug.LogFormat("lives: {0}", _playerhp);
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(20, 20, 150, 25), "Player Health: " + (_playerhp / 2));
        GUI.Box(new Rect(20, 50, 150, 25), "Items Collected: " + _itemscollected);
        GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height - 50, 300, 50), labeltext);

        if (_jetpack >= 1)
        {
            GUI.Box(new Rect(20, 80, 150, 25), "Jump-Pack Active");
        }


        if (showwinscreen)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "YOU WON!"))
            {
            }
        }

        if (showLossScreen)
        {
            Debug.Log("game over attempted");
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "YOU LOSE"))
            {
                try
                {
                    debug("Level Restarted Successfully");
                }

                catch (System.ArgumentException e)
                {
                    debug("Reverting to Scene 0: " + e.ToString());
                }

                finally
                {
                    debug("Restart Handled...");
                }
            }
        }
    }

    public void printlootreport()
    {
        var currentitem = lootstack.Pop();
        var nextitem = lootstack.Peek();
        Debug.LogFormat("you got a {0}! You've got a chance of finding a {1} next!", currentitem, nextitem);


        Debug.LogFormat("There are {0} random loot items waiting for you!", lootstack.Count);
    }
}
