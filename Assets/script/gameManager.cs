using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class gameManager : MonoBehaviour
{
    // ���� �α��� ���� Ȯ�� �ϴ� static ���� ���� 
    public static string ID;

    public bool succecss = false;
    public bool LoginCheck = false;
    UnityWebRequest wwwData;

    [Header("LoginPanel")]
    public InputField IDInputField;
    public InputField PassInputField;

    [Header("CreateAccountPanel")]
    public InputField New_IDInputField;
    public InputField New_PassInputField;
    public InputField New_RePassInputField;
    public InputField New_NickNameField;


    public GameObject LoginPenelObj;
    public GameObject CreateAccountPanelObj;
    public GameObject SuccessLoginPanelObj;
    public GameObject FailedLoginPanelObj;
    public GameObject AbnormalGameExitPanelObj;


    public string LoginUrl = "http://localhost/LoginUnity.php";
    public string CreateUrl = "http://localhost/CreateAccountUnity.php";
    public string LogoutUrl = "http://localhost/LogoutUnity.php";


    //----------------------------------------------------- �α��� ��ư�� ������ �� -----------------------------------------------------//
    public void LoginBtn()
    {
        StartCoroutine(LoginCo());
        //IDInputField.text = "";
        PassInputField.text = "";
        LoginCheck = true;
    }

    IEnumerator LoginCo()
    {
        Debug.Log(IDInputField.text);
        Debug.Log(PassInputField.text);

        WWWForm form = new WWWForm();
        form.AddField("Input_user", IDInputField.text);
        form.AddField("Input_pass", PassInputField.text);

        wwwData = UnityWebRequest.Post(LoginUrl, form);
        yield return wwwData.SendWebRequest();
        EditorApplication.isPaused = false;
        string logindata = wwwData.downloadHandler.text;
        Debug.Log(logindata);
        if (logindata == "1") {
            succecss = true;
            SuccessLoginPanelObj.SetActive(true);
        }
        else if(logindata == "2") {
            succecss = false;
            AbnormalGameExitPanelObj.SetActive(true);
        }
        else {
            succecss = false;
            FailedLoginPanelObj.SetActive(true);
        }
        wwwData.Dispose();
    }
    //----------------------------------------------------- ���������� ���� �߻� �� -----------------------------------------------------//
    public void LogoutGame()
    {
        StartCoroutine(Logout());
    }

    IEnumerator Logout()
    {
        Debug.Log(IDInputField.text);

        WWWForm form = new WWWForm();
        form.AddField("Input_user", IDInputField.text);

        wwwData = UnityWebRequest.Post(LogoutUrl, form);
        yield return wwwData.SendWebRequest();
        Debug.Log(wwwData.downloadHandler.text);
        string Createlogindata = wwwData.downloadHandler.text;

        if ("1" == Createlogindata) {
            Debug.Log("�α׾ƿ� �Ϸ�");
            AbnormalGameExitPanelObj.SetActive(false);
        }
        wwwData.Dispose();
    }
    //----------------------------------------------------- �������� ��ư�� ������ �� -----------------------------------------------------//
    public void OpenCreatrAccountBtn()
    {
        CreateAccountPanelObj.SetActive(true);
        IDInputField.text = "";
        PassInputField.text = "";
    }

    public void CreatAccountBtn()
    {
        if (New_IDInputField.text == "")
            Debug.Log("���̵� �Է��ϼ���");
        else if (New_PassInputField.text == "")
            Debug.Log("��й�ȣ�� �Է��ϼ���");
        else if (New_PassInputField.text != New_RePassInputField.text) 
            Debug.Log("��й�ȣ�� �ٸ��ϴ�.");
        else if(New_NickNameField.text == "")
            Debug.Log("�г����� �Է��ϼ���.");
        else {
            StartCoroutine(CreateCo());
            New_IDInputField.text = "";
            New_PassInputField.text = "";
            New_RePassInputField.text = "";
            New_NickNameField.text = "";
        }
    }

    IEnumerator CreateCo()
    {
        Debug.Log(New_IDInputField.text);
        Debug.Log(New_PassInputField.text);
        Debug.Log(New_RePassInputField.text);
        Debug.Log(New_NickNameField.text);

        
        WWWForm form = new WWWForm();
        form.AddField("Input_user", New_IDInputField.text);
        form.AddField("Input_pass", New_PassInputField.text);
        form.AddField("Input_nick", New_NickNameField.text);

        wwwData = UnityWebRequest.Post(CreateUrl, form);
        yield return wwwData.SendWebRequest();
        Debug.Log(wwwData.downloadHandler.text);
        string Createlogindata = wwwData.downloadHandler.text;

        if ("1" == Createlogindata) {
            Debug.Log("�������� �Ϸ�");
            CreateAccountPanelObj.SetActive(false);
        }
        else {
            Debug.Log("�������� ����");
        }
        wwwData.Dispose();
    }

    //----------------------------------------------------- �������� �ڷΰ��� ��ư�� ������ �� -----------------------------------------------------//
    public void back()
    {
        CreateAccountPanelObj.SetActive(false);
        FailedLoginPanelObj.SetActive(false);
        New_IDInputField.text = "";
        New_PassInputField.text = "";
        New_RePassInputField.text = "";
        New_NickNameField.text = "";
    }

    //----------------------------------------------------- �����÷��� ��  -----------------------------------------------------//
    public void StartGame()
    {
        if (succecss && LoginCheck) {
            LoginPenelObj.SetActive(false);
            CreateAccountPanelObj.SetActive(false);
            SuccessLoginPanelObj.SetActive(false);
            FailedLoginPanelObj.SetActive(false);

            ID =  IDInputField.text;
            SceneManager.LoadScene("Scene2");
        }
    }
}
