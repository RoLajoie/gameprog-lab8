using UnityEngine;
using UnityEngine.SceneManagement;

public class AttackState : IState
{
    private AIController aiController;

    public StateType Type => StateType.Attack;

    public AttackState(AIController aiController)
    {
        this.aiController = aiController;
    }

    public void Enter()
    {
        aiController.Animator.SetBool("isAttacking", true);
        aiController.Agent.isStopped = true; // Stop AI movement
    }

    public void Execute()
    {
        if (Vector3.Distance(aiController.transform.position, aiController.Player.position) > aiController.AttackRange)
        {
            aiController.StateMachine.TransitionToState(StateType.Chase);
            return;
        }

        // If in attack range, restart the level as a "hit" action
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        aiController.Animator.SetBool("isAttacking", false);
        aiController.Agent.isStopped = false; // Resume AI movement
    }
}
