using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player_Move")]
    float hAxis;
    float vAxis;
    public int moveSpeed;
    Vector3 moveVec;

    [Header("Player_Jump")]
    bool jDown;
    bool jumpCheck = false;
    public int jumpPower;

    [Header("Player_Attack")]
    public bool fDown;
    public float curDelay;
    public float maxDelay;
    bool isFireReady = true;

    [Header("Player_Condition")]
    public int health;
    public int damage;

    bool isBorder;

    Rigidbody rigid;
    game2Manager game2_Manager;
    // Start is called before the first frame update

    void Awake()
    {
        game2_Manager = GameObject.Find("Main Camera").GetComponent<game2Manager>();
        game2_Manager.PlayerDataReceive();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Attack();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButtonDown("Jump");
        fDown = Input.GetButtonDown("Attack");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0 , vAxis).normalized;

        if (!isBorder) {
            transform.position += (moveVec * moveSpeed) * Time.deltaTime;
        }

    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);
    }
    void Jump()
    {
        if (jDown && !jumpCheck) {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            jumpCheck = true;
        }
    }

    public void Attack()
    {
        // 공격 텀 시간 
        curDelay += Time.deltaTime;

        isFireReady = maxDelay < curDelay;

        if (fDown && isFireReady) {
            GameObject.Find("Player").transform.Find("Attack_Range").gameObject.SetActive(true);
            damage += 1;
            // 데이터 베이스에 데이터 추가 
            game2_Manager.PlayerConditionFunc();
            curDelay = 0;
            Invoke("End", 0.3f);
        }
    }
    void End()
    {
        GameObject.Find("Player").transform.Find("Attack_Range").gameObject.SetActive(false);
    }
    void OnCollisionEnter( Collision collision )
    {
        // 점프 후 바닥에 닿을 시 
        // bool 변수 초기화 
        if (collision.gameObject.tag == "Floor") {
            jumpCheck = false;
        }
    }

    void FixedUpdate()
    {
        StopToWall();
    }
    void StopToWall()
    {
        // Ray를 쏴서 앞에 벽을 확인 
        Debug.DrawRay(transform.position, transform.forward * 10, Color.green);

        // Ray 값을 변수에 저장 
        isBorder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

}
