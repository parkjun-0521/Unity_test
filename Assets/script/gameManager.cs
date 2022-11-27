using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class gameManager : MonoBehaviour
{
    // 누가 로그인 한지 확인 하는 static 변수 선언 
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


    //----------------------------------------------------- 로그인 버튼을 눌렀을 때 -----------------------------------------------------//
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
    //----------------------------------------------------- 비정상적인 종료 발생 시 -----------------------------------------------------//
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
            Debug.Log("로그아웃 완료");
            AbnormalGameExitPanelObj.SetActive(false);
        }
        wwwData.Dispose();
    }
    //----------------------------------------------------- 계정생성 버튼을 눌렀을 때 -----------------------------------------------------//
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
        else if (New_PassInputField.text != New_RePassInputField.text) 
            Debug.Log("비밀번호가 다릅니다.");
        else if(New_NickNameField.text == "")
            Debug.Log("닉네임을 입력하세요.");
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
            Debug.Log("계정생성 완료");
            CreateAccountPanelObj.SetActive(false);
        }
        else {
            Debug.Log("계정생성 실패");
        }
        wwwData.Dispose();
    }

    //----------------------------------------------------- 계정생성 뒤로가기 버튼을 눌렀을 때 -----------------------------------------------------//
    public void back()
    {
        CreateAccountPanelObj.SetActive(false);
        FailedLoginPanelObj.SetActive(false);
        New_IDInputField.text = "";
        New_PassInputField.text = "";
        New_RePassInputField.text = "";
        New_NickNameField.text = "";
    }

    //----------------------------------------------------- 게임플레이 씬  -----------------------------------------------------//
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
