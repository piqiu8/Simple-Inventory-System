using UnityEngine;

public class Quit : MonoBehaviour
{
    void Update()
    {
        EndingGame();
    }

    public void EndingGame()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //#if UNITY_EDITOR��#endif�������ô������Ϸ��Ҫ�������еĴ��룬UnityEditor����unity��ʹ�õģ���������ò��˵ģ��ᵼ�´������
#if UNITY_EDITOR
            //��������״̬Ϊfalse���˳�unity����״̬
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            //�˳���Ϸ
            Application.Quit();
        }
    }
}
