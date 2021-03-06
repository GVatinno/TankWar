﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node
{
    public Node( float distance, float aim, float power )
    {
        mDistanceFromTarget = distance;
        mAim = aim;
        mPower = power;
    }

    public float mDistanceFromTarget;
    public float mAim;
    public float mPower;
}

public class AiTankStrategy
{
    public AiTankStrategy(TankData data)
    {
        mTankData = data;
        mOrderedNodes = new List<Node>();
        mBestShotIndex = -1;
    }

    TankData mTankData = null;
    List<Node> mOrderedNodes = null;
    int mBestShotIndex = -1;


    private int FindBestShotIndex()
    {
        int best = 0;
        for ( int i = 1; i < mOrderedNodes.Count; ++i)
        {
            if ( Mathf.Abs(mOrderedNodes[best].mDistanceFromTarget) > Mathf.Abs(mOrderedNodes[i].mDistanceFromTarget))
            {
                best = i;
            }
        }
        return best;
    }

    public void ImproveStrategy( float distanceFromTarget, float aim, float power)
    {
        if (mOrderedNodes.Count == 0)
        {
            mOrderedNodes.Add(new Node(distanceFromTarget, aim, power));
            mBestShotIndex = 0;
        }
        else
        {
            mOrderedNodes.Add(new Node(distanceFromTarget, aim, power));
            mOrderedNodes = mOrderedNodes.OrderBy(o => o.mDistanceFromTarget).ToList();
            mBestShotIndex = FindBestShotIndex();
        }
   }

    public void GetNextStrategy(out float aim, out float power)
    {
        if (Empty())
        {
            GetRandomStrategy(out aim, out power);
            return;
        }


        if (mOrderedNodes[mBestShotIndex].mDistanceFromTarget < 0)
        {
            if (mOrderedNodes.Count > mBestShotIndex + 1)
            {
                // are more node after the best
                aim = mOrderedNodes[mBestShotIndex].mAim + (Math.Abs(mOrderedNodes[mBestShotIndex + 1].mAim - mOrderedNodes[mBestShotIndex].mAim)) * 0.5f;
                power = mOrderedNodes[mBestShotIndex].mPower + (Math.Abs(mOrderedNodes[mBestShotIndex + 1].mPower - mOrderedNodes[mBestShotIndex].mPower)) * 0.5f;
            }
            else
            {
                aim = mOrderedNodes[mBestShotIndex].mAim + (mTankData.mMaxAim - mOrderedNodes[mBestShotIndex].mAim) * 0.5f;
                power = mOrderedNodes[mBestShotIndex].mPower + (mTankData.mMaxPower - mOrderedNodes[mBestShotIndex].mPower) * 0.5f;
            }
        }
        else
        {
            if (mOrderedNodes.Count > 1 && mBestShotIndex > 0)
            {
                // are more node before the best
                aim = mOrderedNodes[mBestShotIndex].mAim - (Math.Abs(mOrderedNodes[mBestShotIndex - 1].mAim - mOrderedNodes[mBestShotIndex].mAim)) * 0.5f;
                power = mOrderedNodes[mBestShotIndex].mPower - (Math.Abs(mOrderedNodes[mBestShotIndex - 1].mPower - mOrderedNodes[mBestShotIndex].mPower)) * 0.5f;
            }
            else
            {
                aim = mOrderedNodes[mBestShotIndex].mAim - (mOrderedNodes[mBestShotIndex].mAim - mTankData.mMinAim) * 0.5f;
                power = mOrderedNodes[mBestShotIndex].mPower - (mOrderedNodes[mBestShotIndex].mPower - mTankData.mMinPower) * 0.5f;
            }
        }
    }

    private bool Empty()
    {
        return mOrderedNodes.Count == 0;
    }

    private void GetRandomStrategy(out float aim, out float power)
    {
        // both ranges are [0, 1] so we can use the power
        float n = UnityEngine.Random.Range(mTankData.mMinPower, mTankData.mMaxPower);
        aim = n;
        power = n;
    }

   
}