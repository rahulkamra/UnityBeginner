using UnityEngine;
using System.Collections;
using System.Threading;

public class ThreadFunctionMapper : MonoBehaviour
{

    private static int count = 0;
    private static string result = "";
    public void ExecuteSameThread()
    {
        BigForLoop();
    }

    public void ExecuteDifferentThread()
    {
        Thread thread = new Thread(onThreadStart);
        thread.Start();
        //Debug.Log("Execution Ended");
    }

    public void onThreadStart()
    {
        getBigData();
    }


    public void BigForLoop()
    {
        for (int idx = 0; idx < 1000; idx++)
        {
            count++;
            Debug.Log("Debug Output " + count);
        }
    }

    public string getBigData()
    {
        string strReturn = "";

        for (int idx = 0; idx< 10000; idx++)
        {
            strReturn += "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        }
        Debug.Log("Execution Completed");
        ThreadFunctionMapper.result = strReturn;
        return strReturn;
    }
}
