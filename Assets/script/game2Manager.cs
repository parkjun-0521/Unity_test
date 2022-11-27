using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

public class game2Manager : MonoBehaviour 
{
    // ���� �α��� ������ ���� ID
    // gameManager.ID
    bool isLogoutCheck = false;

    int gameStart = 1;

    UnityWebRequest wwwData;

    Player player;

    public string LogoutUrl = "http://localhost/LogoutUnity.php";
    public string PlayerCondition = "http://localhost/Player_Condition.php";
    public string PDataReceive = "http://localhost/PlayerDataReceive.php";
    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    //----------------------------------------------------- �α׾ƿ� �� ������ ���� ���� -----------------------------------------------------//
    public void LogoutGame()
    {
        StartCoroutine(Logout());
    }
    IEnumerator Logout()
    {
        Debug.Log(gameManager.ID);

        WWWForm form = new WWWForm();
        form.AddField("Input_user", gameManager.ID);

        wwwData = UnityWebRequest.Post(LogoutUrl, form);
        yield return wwwData.SendWebRequest();
        Debug.Log(wwwData.downloadHandler.text);
        string Createlogindata = wwwData.downloadHandler.text;

        if ("1" == Createlogindata) {
            Debug.Log("�α׾ƿ� �Ϸ�");
            isLogoutCheck = true;
            SceneManager.LoadScene("Scene1");
        }
        wwwData.Dispose();
    }
    void OnDisable()
    {
        if(isLogoutCheck == false)
            EditorApplication.isPaused = true;
    }

    //----------------------------------------------------- �÷��̾� ������ ���� -----------------------------------------------------//
    public void PlayerConditionFunc()
    {
        StartCoroutine(Player());
    }
    IEnumerator Player()
    {
        Debug.Log(gameManager.ID);

        WWWForm form = new WWWForm();
        form.AddField("Input_user", gameManager.ID);
        form.AddField("Input_health", player.health);
        form.AddField("Input_damage", player.damage);
        form.AddField("GameStart", gameStart);

        wwwData = UnityWebRequest.Post(PlayerCondition, form);
        yield return wwwData.SendWebRequest();
        Debug.Log(wwwData.downloadHandler.text);
        string check = wwwData.downloadHandler.text;

        if ("1" == check) {
            Debug.Log("������ ���� �Ϸ�");
            gameStart = 0;
        }
        wwwData.Dispose();
    }

    //----------------------------------------------------- �÷��̾� ������ �޾ƿ��� -----------------------------------------------------//
    public void PlayerDataReceive()
    {
        StartCoroutine(DataReceive());
    }
    IEnumerator DataReceive()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        Debug.Log(gameManager.ID);

        WWWForm form = new WWWForm();
        form.AddField("Input_user", gameManager.ID);

        wwwData = UnityWebRequest.Post(PDataReceive, form);
        yield return wwwData.SendWebRequest();
        Debug.Log(wwwData.downloadHandler.text);
        string Receive = wwwData.downloadHandler.text;
        
        player.health = int.Parse(Receive.Substring(0, 3));
        player.damage = int.Parse(Receive.Substring(3));

        Debug.Log(player.health);
        Debug.Log(player.damage);
        wwwData.Dispose();
    }
    
}
