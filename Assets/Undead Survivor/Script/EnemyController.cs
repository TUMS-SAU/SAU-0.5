using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval = 2f;

    private float timeSinceLastShot = 0f;

    private Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void Update()
    {
        // �÷��̾� ���� ���ϱ�
        Vector3 playerDirection = (GameManager.instance.player.transform.position - transform.position).normalized;


        if(enemy != null && enemy.isLive) {

            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot >= shootInterval)
            {
                Shoot();
                timeSinceLastShot = 0f;
            }
        }
        // �Ѿ� �߻� ���� üũ
       
    }

    void Shoot()
    {
        // �Ѿ� �߻�
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}