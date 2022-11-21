using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public bool succecss = false;
    public bool LoginCheck = false;
    UnityWebRequest wwwData;

    [Header("LoginPanel")]
    public InputField NumberInputField;
    public InputField IDInputField;
    public InputField PassInputField;

    [Header("CreateAccountPanel")]
    public InputField New_InputField;
    public InputField New_IDInputField;
    public InputField New_PassInputField;
    public InputField New_InfoInputField;

    public GameObject CreateAccountPanelObj;
    public GameObject SuccessLoginPanelObj;
    public GameObject FailedLoginPanelObj;

    public string LoginUrl = "http://localhost/LoginUnity.php";
    public string CreateUrl = "http://localhost/CreateAccountUnity.php";

    void Start()
    {
    }

    public void LoginBtn()
    {
        StartCoroutine(LoginCo());
        //IDInputField.text = "";
        PassInputField.text = "";
        LoginCheck = true;
    }

    IEnumerator LoginCo()
    {
        Debug.Log(NumberInputField.text);
        Debug.Log(IDInputField.text);
        Debug.Log(PassInputField.text);

        WWWForm form = new WWWForm();
        form.AddField("Input_id", NumberInputField.text);
        form.AddField("Input_user", IDInputField.text);
        form.AddField("Input_pass", PassInputField.text);

        wwwData = UnityWebRequest.Post(LoginUrl, form);
        yield return wwwData.SendWebRequest();
        string logindata = wwwData.downloadHandler.text;
        Debug.Log(logindata);
        if ("1" == logindata) {
            succecss = true;
            SuccessLoginPanelObj.SetActive(true);
        }
        else {
            succecss = false;
            FailedLoginPanelObj.SetActive(true);
        }
    }

    public void OpenCreatrAccountBtn()
    {
        CreateAccountPanelObj.SetActive(true);
        IDInputField.text = "";
        PassInputField.text = "";
    }

    public void CreatAccountBtn()
    {
        if (New_IDInputField.text == "")
            Debug.Log("아이디를 입력하세요");
        else if (New_PassInputField.text == "")
            Debug.Log("비밀번호를 입력하세요");
        else {
            StartCoroutine(CreateCo());
            New_InputField.text = "";
            New_IDInputField.text = "";
            New_PassInputField.text = "";
            New_InfoInputField.text = "";
        }
    }

    IEnumerator CreateCo()
    {
        Debug.Log(New_InputField.text);
        Debug.Log(New_IDInputField.text);
        Debug.Log(New_PassInputField.text);
        Debug.Log(New_InfoInputField.text);

        
        WWWForm form = new WWWForm();
        form.AddField("Input_id", New_InputField.text);
        form.AddField("Input_user", New_IDInputField.text);
        form.AddField("Input_pass", New_PassInputField.text);
        form.AddField("Input_info", New_InfoInputField.text);

        wwwData = UnityWebRequest.Post(CreateUrl, form);
        yield return wwwData.SendWebRequest();
        Debug.Log(wwwData.downloadHandler.text);
        string Createlogindata = wwwData.downloadHandler.text;
        if ("1" == Createlogindata) {
            Debug.Log("계정생성 완료");
            CreateAccountPanelObj.SetActive(false);
        }
        else {
            Debug.Log("계정생성 실패");
        }
    }

    public void back()
    {
        CreateAccountPanelObj.SetActive(false);
        FailedLoginPanelObj.SetActive(false);
        New_InputField.text = "";
        New_IDInputField.text = "";
        New_PassInputField.text = "";
        New_InfoInputField.text = "";
    }
    public void openScene()
    {
        if (succecss && LoginCheck)
            SceneManager.LoadScene("Scene2");
    }
}
