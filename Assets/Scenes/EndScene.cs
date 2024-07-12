using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{
    public void End()
    {
        SceneMgr.Instance.End();
    }
}
