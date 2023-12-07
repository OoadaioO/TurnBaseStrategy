using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    public event EventHandler OnShoot;

    private enum State
    {
        Aming,
        Shooting,
        Cooloff,
    }

    private State state;
    private int maxShootDistance = 7;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;


    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aming:

                Vector3 animDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, animDir, rotateSpeed * Time.deltaTime);

                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }

    }
    private void Shoot()
    {
        OnShoot?.Invoke(this,EventArgs.Empty);
        targetUnit.Damage();

    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aming:
                state = State.Shooting;
                float shootingStateTime = .1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float cooloffStateTime = .5f;
                stateTimer = cooloffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxShootDistance)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridGridPosition(testGridPosition))
                {
                    // Grid position is empty , no unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both Units on Same 'team'
                    continue;
                }
                validGridPositionList.Add(testGridPosition);

            }
        }

        return validGridPositionList;
    }


    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aming;
        float amingStateTime = 1f;
        stateTimer = amingStateTime;
        canShootBullet = true;
    }
}
