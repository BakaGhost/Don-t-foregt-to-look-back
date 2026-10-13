using UnityEngine;

public abstract class MonsterState : MonoBehaviour
{
    public abstract void Init();
    public abstract void Enter();
    public abstract void UpdateState();
    public abstract void Exit();
}
