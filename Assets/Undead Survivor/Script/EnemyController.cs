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
        // 플레이어 방향 구하기
        Vector3 playerDirection = (GameManager.instance.player.transform.position - transform.position).normalized;


        if(enemy != null && enemy.isLive) {

            timeSinceLastShot += Time.deltaTime;
            if (timeSinceLastShot >= shootInterval)
            {
                Shoot();
                timeSinceLastShot = 0f;
            }
        }
        // 총알 발사 간격 체크
       
    }

    void Shoot()
    {
        // 총알 발사
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}