using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public Mesh _DisplayMesh;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawMesh(_DisplayMesh, transform.position, transform.rotation);
        
    }
}
