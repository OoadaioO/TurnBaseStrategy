using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestoryed;

    [SerializeField] private Transform crateDestoryedPrefab;

    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public void Damage()
    {
        Transform crateDestoryedTransform =  Instantiate(crateDestoryedPrefab,transform.position,transform.rotation);
        ApplyExplotionToChildren(crateDestoryedTransform,150f,transform.position,10f);

        Destroy(gameObject);

        OnAnyDestoryed?.Invoke(this, EventArgs.Empty);
    }


       private void ApplyExplotionToChildren(Transform root, float explotionForce, Vector3 explotionPosition, float explotionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explotionForce, explotionPosition, explotionRange);
            }

            ApplyExplotionToChildren(child, explotionForce, explotionPosition, explotionRange);
        }
    }
}
