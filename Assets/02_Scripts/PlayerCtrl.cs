using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl instance;
    private float h = 0.0f, v = 0.0f;
    private Transform tr;
    private float moveSpeed;
    private GameObject joystick;
    private Animator animator;
    private Rigidbody rb;
    private bool isSkill = false;
    private bool isComboAttack = false;
    private bool isDashAttack = false;
    private AudioSource source;
    public AudioClip skillClip;
    

    IEnumerator Start()
    {
        instance = this;
        tr = GetComponent<Transform>();
        moveSpeed = 10.0f;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        source = GetComponent<AudioSource>();
        yield return null;
    }

    void Update()
    {
        Move();
    }

    public void SkillAttackOn()
    {
        if (!isSkill)
        {
            isSkill = true;
            animator.SetTrigger("SkillAttackTrigger");
            Invoke("SkillAttackEnd", 2.5f);
        }
    }
    private void SkillAttackEnd()
    {
        isSkill = false;
    }
    public void SkillSound()
    {
        source.clip = skillClip;
        source.Play();
    }

    public void ComboAttackOn()
    {
        if (!isComboAttack)
        {
            isComboAttack = true;
            animator.SetBool("IsComboAttack", true);
        }
    }
    public void ComboAttackOff()
    {
        isComboAttack = false;
        animator.SetBool("IsComboAttack", false);
    }

    public void DashAttackOn()
    {
        if (!isDashAttack)
        {
            isDashAttack = true;
            animator.applyRootMotion = true;
            animator.SetTrigger("DashAttackTrigger");
            Invoke("DashAttackEnd", 1.75f);
        }
    }
    private void DashAttackEnd()
    {
        animator.applyRootMotion = false;
        isDashAttack = false;
    }

    public void Move()
    {
        if (animator != null && !isSkill && !isComboAttack && !isDashAttack)
        {
            animator.SetFloat("Speed", h * h + v * v);
            if (rb != null)
            {
                Vector3 speed = rb.velocity;
                speed.x = h * moveSpeed;
                speed.z = v * moveSpeed;
                if (animator.GetFloat("Speed") > 0.05f)
                    rb.velocity = speed;
                if (h != 0.0f || v != 0.0f)
                {
                    transform.rotation = Quaternion.LookRotation(speed);
                }
            }
        }
    }
    public void GetStickPos(Vector3 input)
    {
        h = input.x;
        v = input.y;
    }

}
