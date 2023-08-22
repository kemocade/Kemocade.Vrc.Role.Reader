using Kemocade.Vrc.Role.Reader.Extensions;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using static UnityEngine.Debug;
using static VRC.SDKBase.Networking;

namespace Kemocade.Vrc.Role.Reader
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class Reader : UdonSharpBehaviour
    {
        [SerializeField] private VRCUrl _dataUrl;
        private DataDictionary _dictionary;

        // Unity Messages
        protected void Start() =>
            VRCStringDownloader.LoadUrl(_dataUrl, (IUdonEventReceiver)this);

        // Udon Events
        public override void OnStringLoadSuccess(IVRCStringDownload result)
        {
            string json = result.Result;
            if (!VRCJson.TryDeserializeFromJson(json, out DataToken token))
            {
                LogError($"Failed to Deserialize JSON {json} - {result}");
                return;
            }

            if (token.TokenType != TokenType.DataDictionary)
            {
                LogError($"JSON was not a DataDictionary.");
                return;
            }

            _dictionary = token.DataDictionary;
            OnLoaded();
        }

        public override void OnStringLoadError(IVRCStringDownload result) =>
            LogError($"String Load Error ({result.ErrorCode}): {result.Error}");

        public virtual void OnLoaded() { }

        // Accessors
        public bool IsLoaded => _dictionary != null;

        // Safe check to get local player name without edge-case exceptions
        private bool TryGetLocalPlayerName(out string name)
        {
            if
            (
                LocalPlayer == null ||
                !LocalPlayer.IsValid() ||
                string.IsNullOrEmpty(name = LocalPlayer.displayName)
            )
            {
                name = null;
                return false;
            }

            return true;
        }

        // Data Checks
        public bool IsVrcGroupMember(string name, string vrcGroupId) =>
            IsLoaded && _dictionary.IsVrcGroupMember(name, vrcGroupId);

        public bool IsVrcGroupMemberLocal(string vrcGroupId) =>
            TryGetLocalPlayerName(out string name) &&
            IsVrcGroupMember(name, vrcGroupId);

        public bool HasVrcGroupRole(string name, string vrcGroupId, string vrcGroupRoleId) =>
            IsLoaded && _dictionary.HasVrcGroupRole(name, vrcGroupId, vrcGroupRoleId);

        public bool HasVrcGroupRoleLocal(string vrcGroupId, string vrcGroupRoleId) =>
            TryGetLocalPlayerName(out string name) &&
            HasVrcGroupRole(name, vrcGroupId, vrcGroupRoleId);

        public bool IsVrcGroupAdmin(string name, string vrcGroupId) =>
            IsLoaded && _dictionary.IsVrcGroupAdmin(name, vrcGroupId);

        public bool IsVrcGroupAdminLocal(string vrcGroupId) =>
            TryGetLocalPlayerName(out string name) &&
            IsVrcGroupAdmin(name, vrcGroupId);

        public bool IsVrcGroupModerator(string name, string vrcGroupId) =>
            IsLoaded && _dictionary.IsVrcGroupModerator(name, vrcGroupId);

        public bool IsVrcGroupModeratorLocal(string vrcGroupId) =>
            TryGetLocalPlayerName(out string name) &&
            IsVrcGroupModerator(name, vrcGroupId);

        public bool IsDiscordMember(string name, string discordGuildId) =>
            IsLoaded && _dictionary.IsDiscordMember(name, discordGuildId);

        public bool IsDiscordMemberLocal(string discordGuildId) =>
            TryGetLocalPlayerName(out string name) &&
            IsDiscordMember(name, discordGuildId);

        public bool HasDiscordRole(string name, string discordGuildId, string discordRoleId) =>
            IsLoaded && _dictionary.HasDiscordRole(name, discordGuildId, discordRoleId);

        public bool HasDiscordRoleLocal(string discordGuildId, string discordRoleId) =>
            TryGetLocalPlayerName(out string name) &&
            HasDiscordRole(name, discordGuildId, discordRoleId);

        public bool IsDiscordAdmin(string name, string discordGuildId) =>
            IsLoaded && _dictionary.IsDiscordAdmin(name, discordGuildId);

        public bool IsDiscordAdminLocal(string discordGuildId) =>
            TryGetLocalPlayerName(out string name) &&
            IsDiscordAdmin(name, discordGuildId);

        public bool IsDiscordModerator(string name, string discordGuildId) =>
            IsLoaded && _dictionary.IsDiscordModerator(name, discordGuildId);

        public bool IsDiscordModeratorLocal(string discordGuildId) =>
            TryGetLocalPlayerName(out string name) &&
            IsDiscordModerator(name, discordGuildId);

        public bool IsAdminAnywhere(string name) =>
            IsLoaded && _dictionary.IsAdminAnywhere(name);

        public bool IsAdminAnywhereLocal() =>
            TryGetLocalPlayerName(out string name) &&
            IsAdminAnywhere(name);

        public bool IsModeratorAnywhere(string name) =>
            IsLoaded && _dictionary.IsModeratorAnywhere(name);

        public bool IsModeratorAnywhereLocal() =>
            TryGetLocalPlayerName(out string name) &&
            IsModeratorAnywhere(name);
    }
}