using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public float damage;
    public int per;
    Rigidbody2D rigid;
    Vector3 initialDirection; // �Ѿ��� �߻�� �ʱ� ����
                              //Transform player;         // �÷��̾��� Transform ������Ʈ ����



    Vector3 moveDirection; // �Ѿ��� �̵� ����

    void Start()
    {
        // ������ ������ ���� (���ʿ� �� ���� ����)
        SetRandomDirection();
    }

    void Update()
    {
        // �Ѿ��� ������ �������� �̵�
        transform.Translate(moveDirection * bulletSpeed * Time.deltaTime);
    }

    void SetRandomDirection()
    {
        // ������ ������ ����
        float randomAngle = Random.Range(0f, 360f);
        // ������ ������ �̿��Ͽ� ���� ���� ���
        moveDirection = Quaternion.Euler(0, 0, randomAngle) * Vector3.right;
    }
    /*void Update()
    {
       
        // �Ʒ� ������ �Ѿ��� �����Ǿ� ��� ������� �ڵ�
        player = GameManager.instance.player.transform;
        Vector3 targetPosition = player.position;
        Vector3 dir = targetPosition - transform.position; //ũ�Ⱑ ���Ե� ���� : ��ǥ ��ġ(�÷��̾�) - ���� ��ġ
        dir = dir.normalized; //normalized : ���� ������ ������ �����ϰ� ũ�⸦ 1�� ��ȯ�ϴ� �Ӽ�
       // rigid.velocity = dir * bulletSpeed;
        transform.Translate(dir * bulletSpeed * Time.deltaTime);
    }*/

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir) //�ȿ� �ִ°� �޴� �� �� parameter
    {
        this.damage = damage;  //this : �ش� Ŭ������ ������ ����  
                               // this.damage = Bullet �Լ� ���� damage, �׳� damage = Init�Լ��� �޾ƿ��� �Ű����� damage
        this.per = per;

        if (per >= 0)
        { //������ ������ �ƴϸ� ���Ÿ�
            initialDirection = dir.normalized;
            //rigid.velocity = dir * 15f;  //�ӵ��� ����
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || per == -100) // || �� or �̴�
            return;

        // �÷��̾�� ����� ������
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }

        per--;

        if (per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
        else
        {
            // �ٽ� ������ ������ ����
            SetRandomDirection();
        }

    }

    //�Ѿ��� �� ������ ������ ��� ���������� ó�� 
    void OnTriggerExit2D(Collider2D collision)
    {
        //OnTriggerExit2D �̺�Ʈ�� Area�� Ȱ���Ͽ� ���� ��Ȱ��ȭ
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }

}
