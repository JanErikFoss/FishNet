﻿using FishNet.Connection;
using UnityEngine;

namespace FishNet.Object
{
    public partial class NetworkObject : MonoBehaviour
    {
        /// <summary>
        /// Called after all data is synchronized with this NetworkObject.
        /// </summary>
        private void InitializeCallbacks(bool asServer)
        {
            /* When invoking OnOwnership here previous owner will
             * always be an empty connection, since the object is just
             * now initializing. */
            //As server.
            if (asServer)
            {
                for (int i = 0; i < NetworkBehaviours.Length; i++)
                    NetworkBehaviours[i].OnStartServer();

                if (OwnerIsValid)
                {
                    for (int i = 0; i < NetworkBehaviours.Length; i++)
                        NetworkBehaviours[i].OnOwnershipServer(NetworkManager.EmptyConnection);
                }
            }
            //As client.
            else
            {
                for (int i = 0; i < NetworkBehaviours.Length; i++)
                    NetworkBehaviours[i].OnStartClient();

                if (IsOwner)
                {
                    for (int i = 0; i < NetworkBehaviours.Length; i++)
                        NetworkBehaviours[i].OnOwnershipClient(NetworkManager.EmptyConnection);
                }
            }
        }

        /// <summary>
        /// Invokes events to be called after OnServerStart.
        /// This is made one method to save instruction calls.
        /// </summary>
        /// <param name=""></param>
        internal void InvokePostOnServerStart(NetworkConnection conn)
        {
            for (int i = 0; i < NetworkBehaviours.Length; i++)
                NetworkBehaviours[i].OnSendBufferedRpcs(conn);

            for (int i = 0; i < NetworkBehaviours.Length; i++)
                NetworkBehaviours[i].OnSpawnServer(conn);
        }

        /// <summary>
        /// Called on the server before it sends a despawn message to a client.
        /// </summary>
        /// <param name="conn">Connection spawn was sent to.</param>
        internal void InvokeOnServerDespawn(NetworkConnection conn)
        {
            for (int i = 0; i < NetworkBehaviours.Length; i++)
                NetworkBehaviours[i].OnDespawnServer(conn);
        }

        /// <summary>
        /// Invokes OnStop callbacks.
        /// </summary>
        /// <param name="asServer"></param>
        private void InvokeStopCallbacks(bool asServer)
        {
            if (asServer)
            {
                for (int i = 0; i < NetworkBehaviours.Length; i++)
                    NetworkBehaviours[i].OnStopServer();
            }
            else
            {
                for (int i = 0; i < NetworkBehaviours.Length; i++)
                    NetworkBehaviours[i].OnStopClient();
            }
        }

        /// <summary>
        /// Invokes OnOwnership callbacks.
        /// </summary>
        /// <param name="prevOwner"></param>
        private void InvokeOwnership(NetworkConnection prevOwner, bool asServer)
        {
            if (asServer)
            {
                for (int i = 0; i < NetworkBehaviours.Length; i++)
                    NetworkBehaviours[i].OnOwnershipServer(prevOwner);
            }
            else
            {
                for (int i = 0; i < NetworkBehaviours.Length; i++)
                    NetworkBehaviours[i].OnOwnershipClient(prevOwner);
            }
        }
    }

}

