using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] titles;

    // �̹��� ������Ʈ�� Ȱ��ȭ�ϴ� �¸�, �й� �Լ� �ϳ��� �ۼ�
    public void Lose()
    {
        titles[0].SetActive(true);
    }

    public void Win()
    {
        titles[1].SetActive(true);
    }
}
