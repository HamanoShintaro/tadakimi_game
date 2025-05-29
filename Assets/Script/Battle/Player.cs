using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーキャラクターのステータス & 移動処理
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 1000f)]
    private float speed = 2.0f;

    [SerializeField]
    [Range(0f, 1000f)]
    private float limitZoneDistance = 300;

    [SerializeField]
    [Range(-5000f, 0f)]
    private float minLimitMovePosition = -2600f;

    [SerializeField]
    [Range(0f, 5000f)]
    private float maxLimitMovePosition = 2600f;

    [SerializeField]
    public bool isMove = true;

    [SerializeField]
    public bool isRight;

    private RectTransform player;
    private CharacterCore characterCore;

    private Animator animator;

    private void Start()
    {
        player = GetComponent<RectTransform>();
        characterCore = GetComponent<CharacterCore>();
        animator = GetComponent<Animator>();
    }

    private bool wasKnockBackActive = false; // KnockBackの状態を追跡

    private void Update()
    {
        bool isKnockBack = animator.GetBool("KnockBack");

        // KnockBackがtrueならRoot Motionを無効にし、falseなら有効にする
        if (isKnockBack && !wasKnockBackActive)
        {
            animator.applyRootMotion = false;
            wasKnockBackActive = true;
        }
        else if (!isKnockBack && wasKnockBackActive)
        {
            animator.applyRootMotion = true;
            wasKnockBackActive = false;
        }

        // KnockBackがtrueの場合、移動を停止
        if (isKnockBack) return;

        // 移動入力がある場合は背景を動かす&歩きアニメーション再生 / ない場合はアニメーションを停止
        if (isRight && isMove)
        {
            MoveRight();
        }
        else if (!isRight && isMove)
        {
            MoveLeft();
        }
        else
        {
            isMove = false;
        }
    }


    private void MoveRight()
    {
        // スケールで前向き（右向き）に
        var scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x); // xを正にする
        transform.localScale = scale;

        if (player.anchoredPosition.x > maxLimitMovePosition) return;

        transform.localPosition = new Vector3(transform.localPosition.x + speed * Time.deltaTime, -315, 0);
        isMove = true;
    }

    private void MoveLeft()
    {
        // スケールで後向き（左向き）に
        var scale = transform.localScale;
        scale.x = -Mathf.Abs(scale.x); // xを負にする
        transform.localScale = scale;

        if (player.anchoredPosition.x < minLimitMovePosition) return;

        transform.localPosition = new Vector3(transform.localPosition.x - speed * Time.deltaTime, -315, 0);
        isMove = true;
    }

    /// <summary>
    /// 右のパネルが押された時
    /// </summary>
    public void MoveButtonDownRight()
    {
        
        isRight = true;
        isMove = true;
    }

    /// <summary>
    /// 左のパネルが押された時
    /// </summary>
    public void MoveButtonDownLeft()
    {
        
        isRight = false;
        isMove = true;
    }

    /// <summary>
    /// パネルから指が離された時
    /// </summary>
    public void MoveButtonUp()
    {
        isMove = false;
        animator.SetBool("Long", false);
        animator.SetBool("Attack", false);
        animator.SetBool("Skill", false);
        animator.SetBool("Special", false);
        characterCore.canState = true;
    }

    private void OnGUI()
    {
        // HPを強制的に100回復するボタン
        if (GUI.Button(new Rect(10, 10, 200, 30), "HPを100回復"))
        {
            characterCore.hp += 100;
        }
    }
}
