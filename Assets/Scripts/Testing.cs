using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private Vector3 rotation;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(rotation);

        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     GridSystemVisual.Instance.HideAllGridPositions();
        //     GridSystemVisual.Instance.ShowGridPositionList(unit.GetMoveAction().GetValidActionGridPositionList());
        // }
    }


}
