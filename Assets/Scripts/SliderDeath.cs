using UnityEngine;
using UnityEngine.SceneManagement;

public class SliderDeath : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadMenuScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMenuScene()
    {
        // ���������� Canvas ����� ��������� �� ����� ����
        GameObject canvas = GameObject.Find("Canvas"); // �������� "MainCanvas" �� ��� ������ Canvas
        if (canvas != null)
        {
            Destroy(canvas);
        }

       
    }
}
