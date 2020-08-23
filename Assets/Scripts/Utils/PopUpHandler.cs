using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUpHandler : MonoBehaviour
{

    #region public variables

    public GameObject LoadingPopUpParent;
    public static PopUpHandler popUpHandler;

    public enum Panel
    {
        Loading,
        Menu
    };

    #endregion

    #region private variables

    private GameObject CurrentPopUp=null;

    #endregion

    #region MonoBehaviour Methods

    void Start()
    {
        popUpHandler = this;
    }

    #endregion

    #region Alert Popup

    #region Configuring The PopUp

    public void ConfigurePopUp(string message, Panel panel, UnityAction funcRight, string textButtonRight, UnityAction funcLeft, string textButtonLeft, UnityAction funcCenter, string textButtonCenter, bool isCloseVisible, bool isPopUpCloseOnAllButton)
    {
        if (CurrentPopUp == null)
        {
            if (LoadingPopUpParent != null && panel == Panel.Loading)
            {
                CurrentPopUp = Instantiate(Resources.Load("Alert_PopUp"), LoadingPopUpParent.transform) as GameObject;
            }
            else
            {
                return;
            }
            ResetPopUp(panel, funcRight != null ? true : false, funcLeft != null ? true : false, funcCenter != null ? true : false, isCloseVisible);

            // Adding On Click values.
            foreach (Transform child in CurrentPopUp.transform.GetChild(0).transform)
            {
                if ((funcRight != null ? true : false) && child.name == "Btn_Right")
                {
                    child.GetComponent<Button>().onClick.AddListener(funcRight);
                    if (isPopUpCloseOnAllButton)
                        child.GetComponent<Button>().onClick.AddListener(ClosePopUp);
                    child.Find("Text").GetComponent<Text>().text = textButtonRight;
                }
                else if ((funcLeft != null ? true : false) && child.name == "Btn_Left")
                {
                    child.GetComponent<Button>().onClick.AddListener(funcLeft);
                    if (isPopUpCloseOnAllButton)
                        child.GetComponent<Button>().onClick.AddListener(ClosePopUp);
                    child.Find("Text").GetComponent<Text>().text = textButtonLeft;
                }
                else if ((funcCenter != null ? true : false) && child.name == "Btn_Center")
                {
                    child.GetComponent<Button>().onClick.AddListener(funcCenter);
                    if (isPopUpCloseOnAllButton)
                        child.GetComponent<Button>().onClick.AddListener(ClosePopUp);
                    child.Find("Text").GetComponent<Text>().text = textButtonCenter;
                }
                else if (isCloseVisible && child.name == "Btn_Close")
                {
                    child.GetComponent<Button>().onClick.AddListener(ClosePopUp);
                }
                else if (child.name == "Text_Info")
                {
                    child.GetComponent<Text>().text = message;
                }
            }
        }
        else
        {
            Debug.Log("Popup is not empty");
        }
    }

    #endregion

    #region Reseting all elements of PopUp

    void ResetPopUp(Panel panel, bool isRight, bool isLeft, bool isCenter, bool isUpperLeft)
    {
        // Enabling Popup background.
         foreach (Transform child in CurrentPopUp.transform.GetChild(0).transform)
        {
            // enabling or disabling the buttons accoring to requirement.
            if (child.name == "Btn_Center")
            {
                child.transform.gameObject.SetActive(isCenter);
            }
            else if (child.name == "Btn_Close")
            {
                child.transform.gameObject.SetActive(isUpperLeft);
            }
            else if (child.name == "Btn_Left")
            {
                child.transform.gameObject.SetActive(isLeft);
            }
            else if (child.name == "Btn_Right")
            {
                child.transform.gameObject.SetActive(isRight);
            }
            // Removing Old Listeners if any from all buttons.
            if (child.GetComponent<Button>() != null)
                child.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }

    #endregion

    #region Close / Hide PopUp

    public void ClosePopUp()
    {
        Debug.Log("Close Popup");
        if (CurrentPopUp != null)
        {
            Debug.Log("Killed Popup");
            Destroy(CurrentPopUp);
            //CurrentPopUp = null;
        }
        else
            Debug.Log("Didn't Killed Popup");
    }

    #endregion

    #endregion


}
