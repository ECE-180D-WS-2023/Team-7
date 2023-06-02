using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class StatusTracker : NetworkBehaviour
{
    [SyncVar]
    public bool player1_speedup = false;
    [SyncVar]
    public bool player1_slowdown = false;
    [SyncVar]
    public bool player1_inverse = false;
    [SyncVar]
    public bool player2_speedup = false;
    [SyncVar]
    public bool player2_slowdown = false;
    [SyncVar]
    public bool player2_inverse = false;
}