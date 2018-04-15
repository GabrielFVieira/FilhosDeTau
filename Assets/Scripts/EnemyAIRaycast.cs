﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIRaycast : MonoBehaviour {
    #region Public Variables
    public Transform target;
    public Transform raySpawn;
    public float speed;
    public float minRange = 1.5f;
    public float maxRange = 6;
    public float maxDistRay;
    public int lastPath = 10;
    #endregion

    #region Private Variables
    private Rigidbody2D rb2D;
    private Vector2[] directions = new Vector2[8];
    private Vector3 bestPath;
    private RaycastHit2D[] hits = new RaycastHit2D[8];
    private Vector2[] safePaths = new Vector2[8];
    public float offset;
    private float[] ranges = new float[8];
    private bool check;
    private bool[] cols = new bool[8];
    private bool coroutineStarted;
    private int layerMask = ~(1 << 8);
    private Animator anim;
    private float x;
    private float y;
    private int[] alternadPaths = new int[8];
    #endregion

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SetDirection();

        alternadPaths[0] = 4;
        alternadPaths[1] = 5;
        alternadPaths[2] = 6;
        alternadPaths[3] = 7;
        alternadPaths[4] = 0;
        alternadPaths[5] = 1;
        alternadPaths[6] = 2;
        alternadPaths[7] = 3;

    }

    private void FixedUpdate()
    {
        anim.SetFloat("x", x);
        anim.SetFloat("y", y);

        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(raySpawn.position, directions[i], Color.red);
        }

        float range = Vector2.Distance(transform.position, target.position);

        if(range < maxRange && range > minRange)
        {
            if (coroutineStarted == false)
            {
                StartCoroutine("CheckBestPath");
                coroutineStarted = true;
            }
            GetComponent<Animator>().SetBool("isWalking", true);
            transform.position = Vector2.MoveTowards(transform.position, bestPath, speed * Time.deltaTime);

            #region Set Facing Correctly
            if (bestPath.x > transform.position.x && Mathf.Abs(bestPath.x - transform.position.x) > Mathf.Abs(bestPath.y - transform.position.y))
            {
                Debug.Log("Right");
                GetComponent<SpriteRenderer>().flipX = true; // Remove after put a new animation with all 4 way move
                x = 1;
                y = 0;
            }

            else if (bestPath.x < transform.position.x && Mathf.Abs(bestPath.x - transform.position.x) > Mathf.Abs(bestPath.y - transform.position.y))
            {
                Debug.Log("Left");
                GetComponent<SpriteRenderer>().flipX = false; // Remove after put a new animation with all 4 way move
                x = -1;
                y = 0;
            }

            else if (bestPath.y > transform.position.y && Mathf.Abs(bestPath.y - transform.position.y) > Mathf.Abs(bestPath.x - transform.position.x))
            {
                Debug.Log("Up");
                y = 1;
                x = 0;
            }

            else if (bestPath.y < transform.position.y && Mathf.Abs(bestPath.y - transform.position.y) > Mathf.Abs(bestPath.x - transform.position.x))
            {
                Debug.Log("Down");
                y = -1;
                x = 0;
            }
            #endregion
        }

        else
        {
            GetComponent<Animator>().SetBool("isWalking", false);
        }     
    }

    private IEnumerator CheckBestPath()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            hits[i] = Physics2D.Raycast(raySpawn.position, directions[i], maxDistRay, layerMask);

            if (hits[i].collider != null)
            {
                cols[i] = true;
                check = false;
                safePaths[i] = Vector2.zero;
                ranges[i] = 1000;
            }

            else
            {
                cols[i] = false;

                if (lastPath == alternadPaths[i])
                {
                    safePaths[i] = Vector2.zero;
                    ranges[i] = 1000;
                }

                else
                {
                    safePaths[i] = new Vector2(transform.position.x + directions[i].x, transform.position.y + directions[i].y);
                    ranges[i] = Vector2.Distance(safePaths[i], target.position);
                }
            }
        }

        check = IsAllDirectionsSafe();

        if (check)
        {
            bestPath = target.position;
            yield return new WaitForSeconds(0.5f);
            coroutineStarted = false;
            StopCoroutine("CheckBestPath");
            yield return null;
        }
        

        float calcMin = Mathf.Min(ranges[0], ranges[1], ranges[2], ranges[3], ranges[4], ranges[5], ranges[6], ranges[7]);

        for (int z = 0; z < safePaths.Length; z++)
        {
            if (ranges[z] == calcMin)
            {
                bestPath = new Vector3(safePaths[z].x, safePaths[z].y, 0);
                lastPath = z;
                z = safePaths.Length;
            }
        }


        yield return new WaitForSeconds(0.5f);
        coroutineStarted = false;
        check = IsAllDirectionsSafe();
        StopCoroutine("CheckBestPath");
        yield return null;
    }

    private void SetDirection()
    {
        directions[0] = new Vector2(offset, 0);
        directions[1] = new Vector2(offset, offset);
        directions[2] = new Vector2(0, offset);
        directions[3] = new Vector2(-offset, offset);
        directions[4] = new Vector2(-offset, 0);
        directions[5] = new Vector2(-offset, -offset);
        directions[6] = new Vector2(0,-offset);
        directions[7] = new Vector2(offset, -offset);
    }
    
    private bool IsAllDirectionsSafe()
    {
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider != null)
            {
                return false;
            }
        }

        return true;
    }
}
