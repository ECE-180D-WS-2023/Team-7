using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Diagnostics;
using System.IO;

using Mirror;

public class PythonInvoker : NetworkBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            StartCoroutine(WebCam());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    IEnumerator WebCam()
    {
        string topicToSubscribe;

        while (true)
        {
            if (GetComponent<NetworkInfo>().PlayerID != -1)
            {
                topicToSubscribe = "ece180d/team7/player" + GetComponent<NetworkInfo>().PlayerID.ToString();
                break;
            }
        }

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "C:\\Windows\\System32\\cmd.exe",
                RedirectStandardInput = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        process.Start();
        // Pass multiple commands to cmd.exe
        using (var sw = process.StandardInput)
        {
            if (sw.BaseStream.CanWrite)
            {
                UnityEngine.Debug.Log("Writing commands");
                // Vital to activate Anaconda
                sw.WriteLine("C:\\Users\\YSW\\miniconda3\\Scripts\\activate.bat");
                // Activate your environment
                sw.WriteLine("activate ECE180D");
                // run your script. You can also pass in arguments
                UnityEngine.Debug.Log(topicToSubscribe);
                sw.WriteLine("python .\\Assets\\Scripts\\PythonScripts\\detect_object.py -t " + topicToSubscribe);
            }
        }

        yield return null;
    }

}
