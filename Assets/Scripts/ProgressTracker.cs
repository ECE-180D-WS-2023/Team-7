using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ProgressTracker : NetworkBehaviour
{
    [SyncVar]
    public int player1progress = 0;
    [SyncVar]
    public int player2progress = 0;
}
