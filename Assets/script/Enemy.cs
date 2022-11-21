using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int curHealtn;

    Rigidbody rigid;
    BoxCollider boxCollider;

    MeshRenderer mesh;
    Material mat;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();  
        boxCollider = GetComponent<BoxCollider>();
        mesh = GetComponent<MeshRenderer>();
        mat = mesh.material;
    }
    void OnTriggerEnter( Collider other )
    {
        if(other.tag == "Attack") {

            curHealtn -= Player.damage;

            Debug.Log(curHealtn);

            StartCoroutine(OnDamage());
        }
    }
    IEnumerator OnDamage()
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        if (curHealtn > 0) {
            mat.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
        else {
            mat.color = Color.black;
            yield return new WaitForSeconds(0.2f);
            gameObject.SetActive(false);
        }

    }
}
