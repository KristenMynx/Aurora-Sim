/*
 * Copyright (c) Contributors, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the OpenSimulator Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System.Net;
using OpenSim.Framework;
using OpenMetaverse;
using OpenMetaverse.Packets;
using OpenMetaverse.Messages.Linden;
using OpenMetaverse.StructuredData;

namespace OpenSim.Services.Interfaces
{
    public interface IEventQueueService
    {
        /// <summary>
        /// This adds a EventQueueMessage to the user's CAPS handler at the given region handle
        /// </summary>
        /// <param name="o"></param>
        /// <param name="avatarID"></param>
        /// <param name="RegionHandle"></param>
        /// <returns>Whether it was added successfully</returns>
        bool Enqueue(OSD o, UUID avatarID, ulong RegionHandle);

        /// <summary>
        /// This adds a EventQueueMessage to the user's CAPS handler at the given region handle
        /// Returns the result of the enqueue of the event, not just whether it was posted to the service
        /// </summary>
        /// <param name="ev"></param>
        /// <param name="avatarID"></param>
        /// <param name="regionHandle"></param>
        /// <returns></returns>
        bool TryEnqueue(OSD ev, UUID avatarID, ulong regionHandle);

        // These are required to decouple Scenes from EventQueueHelper
        void DisableSimulator(UUID avatarID, ulong RegionHandle);
        void EnableSimulator(ulong handle, byte[] IPAddress, int Port, UUID avatarID, ulong RegionHandle);
        void EstablishAgentCommunication(UUID avatarID, ulong regionHandle, byte[] IPAddress, int Port, string CapsUrl, ulong RegionHandle);
        void TeleportFinishEvent(ulong regionHandle, byte simAccess, 
                                 IPEndPoint regionExternalEndPoint,
                                 uint locationID, uint flags,
                                 UUID agentID, uint teleportFlags, ulong RegionHandle);
        void CrossRegion(ulong handle, Vector3 pos, Vector3 lookAt,
                         IPEndPoint newRegionExternalEndPoint,
                         UUID avatarID, UUID sessionID, ulong RegionHandle);
        void ChatterBoxSessionStartReply(string groupName, UUID groupID, UUID AgentID, ulong RegionHandle);
        void ChatterboxInvitation(UUID sessionID, string sessionName,
                                  UUID fromAgent, string message, UUID toAgent, string fromName, byte dialog,
                                  uint timeStamp, bool offline, int parentEstateID, Vector3 position,
                                  uint ttl, UUID transactionID, bool fromGroup, byte[] binaryBucket, ulong RegionHandle);
        void ChatterBoxSessionAgentListUpdates(UUID sessionID, UUID fromAgent, UUID toAgent, bool canVoiceChat,
                                               bool isModerator, bool textMute, ulong RegionHandle);
        void ParcelProperties(ParcelPropertiesMessage parcelPropertiesMessage, UUID avatarID, ulong RegionHandle);
        void ChatterBoxSessionAgentListUpdates(UUID sessionID, OpenMetaverse.Messages.Linden.ChatterBoxSessionAgentListUpdatesMessage.AgentUpdatesBlock[] message, UUID toAgent, string Transition, ulong RegionHandle);
        void GroupMembership(AgentGroupDataUpdatePacket groupUpdate, UUID avatarID, ulong RegionHandle);
        void QueryReply(PlacesReplyPacket placesReply, UUID avatarID, string[] RegionTypes, ulong RegionHandle);
        void ScriptRunningReply(UUID objectID, UUID itemID, bool running, bool mono,
            UUID avatarID, ulong RegionHandle);


        /// <summary>
        /// This is a region > CapsService message ONLY, this should never be sent to the client.
        /// </summary>
        /// <param name="avatarID"></param>
        /// <param name="RegionHandle"></param>
        /// <param name="DrawDistance"></param>
        /// <param name="neighbors"></param>
        /// <param name="circuit"></param>
        void EnableChildAgentsReply(UUID avatarID, ulong RegionHandle,
            int DrawDistance, GridRegion[] neighbors, AgentCircuitData circuit, 
            AgentData data, uint TeleportFlags);

        /// <summary>
        /// This is a region > CapsService message ONLY, this should never be sent to the client.
        /// </summary>
        /// <param name="avatarID"></param>
        /// <param name="RegionHandle"></param>
        /// <param name="DrawDistance"></param>
        /// <param name="neighbors"></param>
        /// <param name="circuit"></param>
        bool TryEnableChildAgents(UUID avatarID, ulong RegionHandle,
            int DrawDistance, GridRegion region, AgentCircuitData circuit,
            AgentData data, uint TeleportFlags, byte[] IPAddress, int Port);
    }
}
