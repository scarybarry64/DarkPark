//Andrew's
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerInteract : MonoBehaviour
{
    //"inventory" max capacity 1 for each item
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Text InteractMessage;
    [SerializeField] private Image icon;
    [SerializeField] private DarqueStatue statue;
    Image icon_Coin, icon_Candy, icon_Teddy, icon_Key;
    bool coin = false, teddy = false, candy = false, key = false;
    private GameObject TargetObj;
    private int interactType; //0 = interactable object, 1 = item pickup

    // Start is called before the first frame update
    void Start()
    {
        getItemIcons();
        updateItems();
    }

    // Update is called once per frame
    private void getItemIcons()
    {
        GameObject src = GameObject.Find("icon_Coin");
        icon_Coin = src.GetComponent<Image>();

        src = GameObject.Find("icon_Candy");
        icon_Candy = src.GetComponent<Image>();

        src = GameObject.Find("icon_Teddy");
        icon_Teddy = src.GetComponent<Image>();

        src = GameObject.Find("icon_Key");
        icon_Key = src.GetComponent<Image>();
    }
    void Update()
    {
        HandleInteraction();
    }
    private bool addItem(string name)
    {
        switch (name)
        {
            case "coin":
                if (coin)
                    return false;
                icon_Coin.enabled = true;
                coin = true;
                printMessage("I got a coin from the fountain");
                break;
            case "teddy":
                teddy = true;
                printMessage("I got a teddy bear");
                icon_Teddy.enabled = true;
                break;
            case "candy":
                if (candy)
                    return false;
                printMessage("I got candy");
                icon_Candy.enabled = true;
                candy = true;
                break;
            case "key":
                if (key)
                    return false;
                audioManager.Play("keys");
                printMessage("I got a key");
                icon_Key.enabled = true;
                key = true;
                break;
            default:
                return false;
                break;
        }
        updateItems();
        return true;
    }
    private void updateItems()
    {
    }
    //event executor
    private void HandleInteraction()
    {
        if (TargetObj)
        {
            if (Input.GetMouseButtonDown(1))
            {
                switch (interactType)
                {
                    case 0: //interactables
                        switch (TargetObj.name)
                        {
                            case "vendor": //vending machine
                                if (coin)
                                {
                                    if (!candy)
                                    {
                                        audioManager.Play("pickup");
                                        coin = false;
                                        icon_Coin.enabled = false;
                                        addItem("candy");
                                    }
                                    else
                                        InteractMessage.text = "I already have candy";
                                }
                                else if (!candy)
                                {
                                    audioManager.Play("VHS");
                                    InteractMessage.text = "I need a coin to buy a snack";
                                }
                                else
                                    InteractMessage.text = "I already have candy";
                                break;
                            case "coin":
                                if (coin)
                                {
                                    printMessage("It would be disrespectful to hold more than one!");
                                }
                                else
                                {
                                    addItem("coin");
                                    audioManager.Play("pickup");
                                    statue.PissOff();
                                }
                                break;
                            case "Mary":
                                if (candy)
                                {
                                    candy = false;
                                    audioManager.Play("yay");
                                    printMessage("Mary has been rescued!");
                                    kidIcon("icon_Mary"); //show mary icon
                                    icon.enabled = false;
                                    icon_Candy.enabled = false;
                                    Destroy(TargetObj);
                                    statue.PissOff();
                                }
                                else
                                {
                                    audioManager.Play("VHS");
                                    printMessage("I want candy!");
                                }
                                break;
                            case "Timmy":
                                if (teddy)
                                {
                                    teddy = false;
                                    audioManager.Play("yay");
                                    printMessage("Timmy has been rescued!");
                                    kidIcon("icon_Timmy"); //show timmy icon
                                    icon.enabled = false;
                                    icon_Teddy.enabled = false;
                                    Destroy(TargetObj);
                                    statue.PissOff();
                                }
                                else
                                {
                                    audioManager.Play("VHS");
                                    InteractMessage.text = "I'm not going until I get Mr. Biggles!";
                                }
                                break;
                            case "door": //locked door
                                if (key)
                                {
                                    key = false;
                                    updateItems();
                                    audioManager.Play("unlock");
                                    printMessage("Unlocked with Key");
                                    TargetObj.name = "door2";
                                    icon_Key.enabled = false;
                                    //Destroy(TargetObj);
                                }
                                else
                                {
                                    audioManager.Play("door locked");
                                    InteractMessage.text = "Locked. I need a key";
                                }
                                break;
                            case "door2":
                                audioManager.Play("door open");
                                icon.enabled = false;
                                Destroy(TargetObj);
                                break;
                            case "demon":
                                if (coin)
                                {
                                    coin = false;
                                    icon_Coin.enabled = false;
                                    updateItems();
                                    audioManager.Play("VHS");
                                    printMessage("Thanks for the coin, loser!");
                                    Destroy(TargetObj);
                                }
                                else //you die
                                {
                                    printMessage("AND NOW YOU DIE!");
                                    Invoke("killPlayer", 2);
                                }
                                break;
                        }
                        break;
                    case 1: //item pickups
                        if (addItem(TargetObj.name)) //successful add to "inventory"
                        {
                            audioManager.Play("pickup");
                            icon.enabled = false;
                            Destroy(TargetObj);
                            //hideMessage();
                        }
                        else //item already full or doesn't exist
                        {
                            printMessage("I don't really need this now");//InteractMessage.text = "I don't really need this now";
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void kidIcon(string name)
    {
        GameObject src = GameObject.Find(name);
        Image img = src.GetComponent<Image>();
        img.enabled = true;
    }
    private void killPlayer()
    {
        SceneManager.LoadScene("MenuScene");
    }
    private void printMessage(string text)
    {
        InteractMessage.text = text;
        Invoke("hideMessage", 2);
    }
    private void hideMessage()
    {
        icon.enabled = false;
        InteractMessage.enabled = false;
        interactType = -1;
        TargetObj = null;
    }
    private void OnTriggerStay(Collider collider)
    {
        //check interactable object

        if (collider.CompareTag("Interactable"))
        {
            //interacting with different object than before
            if (TargetObj != collider.gameObject)
                InteractMessage.text = "";
            icon.enabled = true;
            InteractMessage.enabled = true;
            interactType = 0;
            TargetObj = collider.gameObject;
        }
        else if (collider.CompareTag("Item"))
        {
            if (TargetObj != collider.gameObject)
                InteractMessage.text = "";
            icon.enabled = true;
            InteractMessage.enabled = true;
            interactType = 1;
            TargetObj = collider.gameObject; //object being interacted with
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Interactable") || collider.CompareTag("Item"))
        {
            hideMessage();
        }
    }
}
