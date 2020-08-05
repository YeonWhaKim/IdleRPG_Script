using UnityEngine;

public class BGState : MonoBehaviour
{
    public Animator bganimator;

    // Start is called before the first frame update
    private void Start()
    {
        StateManager.instance.idle += Idle;
        StateManager.instance.move += Move;
        StateManager.instance.action += Stop;
        StateManager.instance.bossDieMove += Stop;
        Idle();
    }

    public void Idle()
    {
        bganimator.speed = 0;
    }

    public void Move()
    {
        //bganimator.Rebind();
        bganimator.speed = 1;
    }

    public void Stop()
    {
        bganimator.speed = 0;
    }
}