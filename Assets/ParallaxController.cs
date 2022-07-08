using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public GameObject referenceObj;
    public Vector3 parralaxValues, anchor, offset;

    private void Start()
    {
        anchor = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rPos = referenceObj.transform.position;
        offset = new Vector3((anchor.x - rPos.x) * parralaxValues.x, (anchor.y - rPos.y) * parralaxValues.y, 0);
        transform.position = anchor + offset;
    }
}
