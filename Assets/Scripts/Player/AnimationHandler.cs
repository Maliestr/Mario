using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void UpdateMovement(bool isRunning, bool isGrounded, float verticalSpeed)
    {
        if (animator == null) return;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("verticalSpeed", verticalSpeed);
    }
    
    public void TriggerAttack(int comboStep)
    {
        if (animator == null) return;
        
        switch (comboStep)
        {
            case 0: animator.SetTrigger("isAttacking1"); break;
            case 1: animator.SetTrigger("isAttacking2"); break;
            case 2: animator.SetTrigger("isAttacking3"); break;
        }
    }
    
    public void TriggerHurt()
    {
        animator?.SetTrigger("isHurt");
    }
    
    public void TriggerDeath()
    {
        animator?.SetBool("isDead", true);
    }
    
    public void SetBig(bool isBig)
    {
        if (animator != null)
            animator.SetBool("isBig", isBig);
    }
}