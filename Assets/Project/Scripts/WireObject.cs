using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class WireObject : MonoBehaviour
{
    private List<WireNode> wireNodes = new List<WireNode>();
    [SerializeField]
    private LineRenderer wireRenderer = null;
    [SerializeField]
    private WireObject endNode = null;

    [SerializeField]
    private GameObject wireNodePrefab = null;

    [SerializeField]
    private GameObject drawnNode = null;

    [SerializeField]
    LayerMask layer = (1 << 0);

    public UnityEvent _OnSignalRecieved;

    bool isPlacing = false;
#if UNITY_EDITOR // debug only
    [SerializeField]
    private bool IgnoreInput = false;
#endif
    private void RenderConnection()
    {
        // loop from this > foreach (wire nodes) > end node and draw line

        if (this.wireNodes.Count == 0)
        {
            wireRenderer.enabled = false;
            return;
        }
        else
            wireRenderer.enabled = true;
        List<Vector3> positions = new List<Vector3>();
        positions.Add(this.transform.position);

        foreach(WireNode connection in wireNodes)
        {
            positions.Add(connection.transform.position);
        }

        if (isPlacing)
        {
            positions.Add(drawnNode.transform.position);
        }

        if (endNode != null)
        positions.Add(endNode.transform.position);



        this.wireRenderer.positionCount = positions.Count;
        this.wireRenderer.SetPositions(positions.ToArray());
    }
#if UNITY_EDITOR // debug only
    private void Update()
    {
        RenderConnection();
        if (IgnoreInput)
            return;
        if (Input.GetKey(KeyCode.P))
        {
            DrawNode(GameObject.FindObjectOfType<Camera>().gameObject);
            isPlacing = true;
        }
        else
            isPlacing = false;
        if (Input.GetKeyDown(KeyCode.I))
        {
            CreateNode(GameObject.FindObjectOfType<Camera>().gameObject);
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            TryDeleteNode(GameObject.FindObjectOfType<Camera>().gameObject);
        }

    }
#endif
    /// <summary>
    /// Draws a node from the player camera, forward. 
    /// </summary>
    /// <param name="playerCamera">Player Camera reference</param>
    private void DrawNode(GameObject playerCamera)
    {
      
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 10f, layer))
        {
            WireObject other = hit.collider.GetComponent<WireObject>();
            if (other)
                return;

            drawnNode.GetComponent<Renderer>().enabled = true;
            drawnNode.transform.position = hit.point;
            drawnNode.transform.up = hit.normal;
        }else
        {
            drawnNode.GetComponent<Renderer>().enabled = false;
        }
    }

    /// <summary>
    /// Raycasts infront of the player camera, and tries to delete the node.
    /// </summary>
    /// <param name="playerCamera"></param>
    private void TryDeleteNode(GameObject playerCamera)
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 10f, layer))
        {
            WireNode other = hit.collider.GetComponent<WireNode>();
            if (other)
            {
                RemoveWireNode(other);
            }
        }
    }

    /// <summary>
    /// Creates a node, and adds it to the array, and connects it.
    /// </summary>
    /// <param name="playerCamera"></param>
    private void CreateNode(GameObject playerCamera)
    {
        // creates game object at target (and parents) unless the object clicked is a WireObject..

        // cast from camera foward.. 
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 10f, layer))
        {
            WireObject other = hit.collider.GetComponent<WireObject>();
            if (other)
            {
                this.endNode = other;
            }
            else
            {
                WireNode newNode = Instantiate(wireNodePrefab).GetComponent<WireNode>();
                newNode.parent = this;
                newNode.transform.position = hit.point;
                newNode.transform.up = hit.normal;
                newNode.transform.parent = hit.transform;
                wireNodes.Add(newNode);
            }
        }
    }
    /// <summary>
    /// Attempts to find the node in the array, and returns the index if found, or -1 if not found.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private int FindWireNodeIndex(WireNode node)
    {
        int i = 0;
        foreach(WireNode wireNode in wireNodes)
        {
            if (wireNode == node)
                return i;
            i++;
        }
        return -1;
    }

    /// <summary>
    /// Removes the node/deletes the object if it exists in the array, and will then resize the array.
    /// </summary>
    /// <param name="node"></param>
    private void RemoveWireNode(WireNode node)
    {
        int index = FindWireNodeIndex(node);
        if (index < 0)
            return;
        for(int i = (wireNodes.Count - 1); index < wireNodes.Count;i--)
        {
            Destroy(wireNodes[i].gameObject);
            wireNodes.RemoveAt(i);
        }
        endNode = null;

    }

    /// <summary>
    /// Moves the selected node to the provided position
    /// </summary>
    /// <param name="node"></param>
    /// <param name="nodePos"></param>
    public void MoveNode(WireNode node, Vector3 nodePos)
    {
        if (node)
        {
            node.transform.position = nodePos;
        }
    }

    /// <summary>
    /// Used to invoke WireObjects delegates assigned in the inspector, from both this object, and the reciever.
    /// </summary>
    public void OnSignalRecieved()
    {
        _OnSignalRecieved?.Invoke();
        if (endNode)
        {
            endNode._OnSignalRecieved?.Invoke();
        }
    }
}
