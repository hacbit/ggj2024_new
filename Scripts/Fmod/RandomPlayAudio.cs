using System.Collections;
using UnityEngine;

public class RandomPlayAudio : MonoBehaviour
{
    private FMOD.Studio.EventInstance stageInstance;
    public FMODUnity.EventReference[] stageEvents;

    private FMOD.Studio.EventInstance fallInstance;
    public FMODUnity.EventReference fallEvent;

    private FMOD.Studio.EventInstance winInstance;
    public FMODUnity.EventReference winEvent;

    public static bool isStageDown = false;
    public static bool isWin = false;
    public static bool isFall = false;

    public void Start()
    {
        int index = Random.Range(0, stageEvents.Length);
        stageInstance = FMODUnity.RuntimeManager.CreateInstance(stageEvents[index]);
        stageInstance.start();

        fallInstance = FMODUnity.RuntimeManager.CreateInstance(fallEvent);
        winInstance = FMODUnity.RuntimeManager.CreateInstance(winEvent);
    }

    public void Update()
    {
        if (isStageDown)
        {
            stageInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            isStageDown = false;
        }

        if (isFall)
        {
            OnFall();
        }

        if (isWin)
        {
            OnWin();
        }
    }

    public void OnFall()
    {
        fallInstance.start();
    }

    public void OnWin()
    {
        winInstance.start();
    }
}