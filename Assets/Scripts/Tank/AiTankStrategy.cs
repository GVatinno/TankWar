using System;
using System.Collections.Generic;
using UnityEngine;

public enum StrategyType : int
{
    IncreaseAim = 1,
    DecreaseAim = 2,
    IncreasePower = 3,
    DecreasePower = 4
}

public class Node
{
    public Node( float distance, float aim, float power, StrategyType strategy, Node rhs, Node lhs, Node mPreviousNode)
    {
        mSqrdDistanceFromTarget = distance;
        mAim = aim;
        mPower = power;
        mStrategy = strategy;
        mLhs = lhs;
        mRhs = rhs;
        mPreviousNode = null;
    }

    public float mSqrdDistanceFromTarget;
    public float mAim;
    public float mPower;
    public StrategyType mStrategy;
    public Node mRhs;
    public Node mLhs;
    public Node mPreviousNode;
}

public class AiTankStrategy
{
    public AiTankStrategy(TankData data)
    {
        mTankData = data;
    }

    Node mRoot = null;
    Node mLastNode = null;
    TankData mTankData = null;


    public bool Empty()
    {
        return mRoot == null;
    }


    public void ImproveStrategy(float distance, float aim, float power, StrategyType strategy)
    {
        Node nodeToCreate = null;
        if (mRoot == null)
        {
            nodeToCreate = new Node(distance, aim, power, strategy, null, null, mLastNode);
            mRoot = nodeToCreate;
        }
        else
        {
            nodeToCreate = InsertRecursively(mRoot, distance, aim, power, strategy);
        }
        mLastNode = nodeToCreate;
    }

    Node InsertRecursively(Node parent, float distance, float aim, float power, StrategyType strategy)
    {
        Node newNode = null;
        if (distance <= parent.mSqrdDistanceFromTarget)
        {
            if (parent.mLhs == null)
            {
                parent.mLhs = new Node(distance, aim, power, strategy, null, null, mLastNode);
                newNode = parent.mLhs;
                return newNode;
            }
            else
            {
                return InsertRecursively(parent.mLhs, distance, aim, power, strategy);
            }
        }
        else
        {
            if (parent.mRhs == null)
            {
                parent.mRhs = new Node(distance, aim, power, strategy, null, null, mLastNode);
                newNode = parent.mRhs;
                return newNode;
            }
            else
            {
                return InsertRecursively(parent.mRhs, distance, aim, power, strategy);
            }
        }
    }

    private Node FindLastBestStrategy( Node node)
    {
        if (node.mLhs == null)
        {
            return node;
        }
        else
        {
            return FindLastBestStrategy(node.mLhs);
        }
    }

    private void ComputeNewStrategyValues(StrategyType strategy, float previousAim, float previousPower, out float aim, out float power)
    {
        float newAim = previousAim;
        float newPower = previousPower;
        switch (strategy)
        {
            case StrategyType.DecreaseAim:
                newAim = UnityEngine.Random.Range(mTankData.mMinAim, previousAim - mTankData.mIncrement);
                break;
            case StrategyType.IncreaseAim:
                newAim = UnityEngine.Random.Range(previousAim + mTankData.mIncrement, mTankData.mMaxAim);
                break;
            case StrategyType.DecreasePower:
                newPower = UnityEngine.Random.Range(mTankData.mMinPower, previousPower - mTankData.mIncrement);
                break;
            case StrategyType.IncreasePower:
                newPower = UnityEngine.Random.Range(previousPower + mTankData.mIncrement, mTankData.mMaxPower);
                break;
        }
        aim = newAim;
        power = newPower;
    }

    private StrategyType ChangeStrategy(Node lastNode, Node previousNode)
    {
        StrategyType newStrategy = lastNode.mStrategy;
        switch (newStrategy)
        {
            case StrategyType.DecreaseAim:
                if (lastNode.mSqrdDistanceFromTarget < previousNode.mSqrdDistanceFromTarget)
                    newStrategy = StrategyType.DecreasePower;
                else
                    newStrategy = StrategyType.IncreasePower;
                break;
            case StrategyType.IncreaseAim:
                if (lastNode.mSqrdDistanceFromTarget < previousNode.mSqrdDistanceFromTarget)
                    newStrategy = StrategyType.IncreasePower;
                else
                    newStrategy = StrategyType.DecreasePower;
                break;
            case StrategyType.DecreasePower:
                if (lastNode.mSqrdDistanceFromTarget < previousNode.mSqrdDistanceFromTarget)
                    newStrategy = StrategyType.DecreaseAim;
                else
                    newStrategy = StrategyType.IncreaseAim;
                break;
            case StrategyType.IncreasePower:
                if (lastNode.mSqrdDistanceFromTarget < previousNode.mSqrdDistanceFromTarget)
                    newStrategy = StrategyType.IncreaseAim;
                else
                    newStrategy = StrategyType.DecreaseAim;
                break;
        }
        return newStrategy;
    }

    public StrategyType ChangeStrategyIfValueAreAtLimit( Node node)
    {
        StrategyType newStrategy = node.mStrategy;
        switch (newStrategy)
        {
            case StrategyType.DecreaseAim:
                if ( node.mAim <= mTankData.mMinAim )
                    newStrategy = StrategyType.DecreasePower;
                break;
            case StrategyType.IncreaseAim:
                if (node.mAim >= mTankData.mMaxAim)
                    newStrategy = StrategyType.IncreasePower;
                break;
            case StrategyType.DecreasePower:
                if (node.mPower <= mTankData.mMinPower)
                    newStrategy = StrategyType.DecreaseAim;
                break;
            case StrategyType.IncreasePower:
                if (node.mPower >= mTankData.mMaxPower)
                    newStrategy = StrategyType.IncreaseAim;
                break;
        }
        return newStrategy;
    }

    public StrategyType GetNextStrategy(out float aim, out float power)
    {
        Node node = FindLastBestStrategy(mRoot);
        StrategyType newStrategy = node.mStrategy;

        if (node != mLastNode)
        {
            newStrategy = ChangeStrategy(node, mLastNode);
        }
        else
        {
            newStrategy = ChangeStrategyIfValueAreAtLimit(node);
        }
        ComputeNewStrategyValues(newStrategy, node.mAim, node.mPower, out aim, out power);
        return newStrategy;
    }

    public StrategyType GetRandomStrategy(out float aim, out float power)
    {
        aim = mTankData.mMinAim;
        power = UnityEngine.Random.Range(mTankData.mMinPower * 0.5f, mTankData.mMaxPower);
        return StrategyType.IncreasePower;
    }

    public void ResetStrategy()
    {
        mRoot = null;
        mLastNode = null;
    }
}