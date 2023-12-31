﻿using VRC.SDK3.Data;

namespace Kemocade.Vrc.Role.Reader.Extensions
{
    public static class DataDictionaryExtensions
    {
        public static bool IsVrcGroupMember(this DataDictionary dictionary, string name, string vrcGroupId)
        {
            if
            (
                !dictionary.TryGetVrcUserIndex(out double index, name) ||
                !dictionary.TryGetVrcGroupVrcUsers(out DataList vrcUsers, vrcGroupId)
            )
            { return false; }

            return vrcUsers.Contains(index);
        }

        public static bool HasVrcGroupRole(this DataDictionary dictionary, string name, string vrcGroupId, string vrcGroupRoleId)
        {
            if
            (
                !dictionary.TryGetVrcUserIndex(out double index, name) ||
                !dictionary.TryGetVrcGroupRoleVrcUsers(out DataList vrcUsers, vrcGroupId, vrcGroupRoleId)
            )
            { return false; }

            return vrcUsers.Contains(index);
        }

        public static bool IsVrcGroupAdmin(this DataDictionary dictionary, string name, string vrcGroupId)
        {
            if
            (
                !dictionary.TryGetVrcUserIndex(out double index, name) ||
                !dictionary.TryGetVrcGroupRoles(out DataDictionary roles, vrcGroupId)
            )
            { return false; }

            DataList roleIds = roles.GetKeys();
            for (int i = 0; i < roleIds.Count; i++)
            {
                string roleId = roleIds[i].String;

                dictionary.TryGetVrcGroupRoleIsAdmin(out bool isAdmin, vrcGroupId, roleId);
                if (!isAdmin) { continue; }

                if (!dictionary.TryGetVrcGroupRoleVrcUsers(out DataList vrcUsers, vrcGroupId, roleId)) { return false; }
                if (vrcUsers.Contains(index)) { return true; }
            }

            return false;
        }

        public static bool IsVrcGroupModerator(this DataDictionary dictionary, string name, string vrcGroupId)
        {
            if
            (
                !dictionary.TryGetVrcUserIndex(out double index, name) ||
                !dictionary.TryGetVrcGroupRoles(out DataDictionary roles, vrcGroupId)
            )
            { return false; }

            DataList roleIds = roles.GetKeys();
            for (int i = 0; i < roleIds.Count; i++)
            {
                string roleId = roleIds[i].String;

                dictionary.TryGetVrcGroupRoleIsModerator(out bool isModerator, vrcGroupId, roleId);
                if (!isModerator) { continue; }

                if (!dictionary.TryGetVrcGroupRoleVrcUsers(out DataList vrcUsers, vrcGroupId, roleId)) { return false; }
                if (vrcUsers.Contains(index)) { return true; }
            }

            return false;
        }

        public static bool IsDiscordMember(this DataDictionary dictionary, string name, string discordGuildId)
        {
            if
            (
                !dictionary.TryGetVrcUserIndex(out double index, name) ||
                !dictionary.TryGetDiscordServerVrcUsers(out DataList vrcUsers, discordGuildId)
            )
            { return false; }

            return vrcUsers.Contains(index);
        }

        public static bool HasDiscordRole(this DataDictionary dictionary, string name, string discordGuildId, string discordRoleId)
        {
            if
            (
                !dictionary.TryGetVrcUserIndex(out double index, name) ||
                !dictionary.TryGetDiscordServerRoleVrcUsers(out DataList vrcUsers, discordGuildId, discordRoleId)
            )
            { return false; }

            return vrcUsers.Contains(index);
        }

        public static bool IsDiscordAdmin(this DataDictionary dictionary, string name, string discordGuildId)
        {
            if
            (
                !dictionary.TryGetVrcUserIndex(out double index, name) ||
                !dictionary.TryGetDiscordServerRoles(out DataDictionary roles, discordGuildId)
            )
            { return false; }

            DataList roleIds = roles.GetKeys();
            for (int i = 0; i < roleIds.Count; i++)
            {
                string roleId = roleIds[i].String;

                dictionary.TryGetDiscordServerRoleIsAdmin(out bool isAdmin, discordGuildId, roleId);
                if (!isAdmin) { continue; }

                if (!dictionary.TryGetDiscordServerRoleVrcUsers(out DataList vrcUsers, discordGuildId, roleId)) { return false; }
                if (vrcUsers.Contains(index)) { return true; }
            }

            return false;
        }

        public static bool IsDiscordModerator(this DataDictionary dictionary, string name, string discordGuildId)
        {
            if
            (
                !dictionary.TryGetVrcUserIndex(out double index, name) ||
                !dictionary.TryGetDiscordServerRoles(out DataDictionary roles, discordGuildId)
            )
            { return false; }

            DataList roleIds = roles.GetKeys();
            for (int i = 0; i < roleIds.Count; i++)
            {
                string roleId = roleIds[i].String;

                dictionary.TryGetDiscordServerRoleIsModerator(out bool isModerator, discordGuildId, roleId);
                if (!isModerator) { continue; }

                if (!dictionary.TryGetDiscordServerRoleVrcUsers(out DataList vrcUsers, discordGuildId, roleId)) { return false; }
                if (vrcUsers.Contains(index)) { return true; }
            }

            return false;
        }

        public static bool IsAdminAnywhere(this DataDictionary dictionary, string name)
        {
            if
            (
                !dictionary.TryGetVrcGroupsById(out DataDictionary vrcGroupsById) ||
                !dictionary.TryGetDiscordServersById(out DataDictionary discordServersById)
            )
            { return false; }

            DataList vrcGroupIds = vrcGroupsById.GetKeys();
            for (int i = 0; i < vrcGroupIds.Count; i++)
            {
                string vrcGroupId = vrcGroupIds[i].String;
                if (dictionary.IsVrcGroupAdmin(name, vrcGroupId)) { return true; }
            }

            DataList discordGuildIds = discordServersById.GetKeys();
            for (int i = 0; i < discordGuildIds.Count; i++)
            {
                string discordGuildId = discordGuildIds[i].String;
                if (dictionary.IsDiscordAdmin(name, discordGuildId)) { return true; }
            }

            return false;
        }

        public static bool IsModeratorAnywhere(this DataDictionary dictionary, string name)
        {
            if
            (
                !dictionary.TryGetVrcGroupsById(out DataDictionary vrcGroupsById) ||
                !dictionary.TryGetDiscordServersById(out DataDictionary discordServersById)
            )
            { return false; }

            DataList vrcGroupIds = vrcGroupsById.GetKeys();
            for (int i = 0; i < vrcGroupIds.Count; i++)
            {
                string vrcGroupId = vrcGroupIds[i].String;
                if (dictionary.IsVrcGroupModerator(name, vrcGroupId)) { return true; }
            }

            DataList discordGuildIds = discordServersById.GetKeys();
            for (int i = 0; i < discordGuildIds.Count; i++)
            {
                string discordGuildId = discordGuildIds[i].String;
                if (dictionary.IsDiscordModerator(name, discordGuildId)) { return true; }
            }

            return false;
        }

        private static bool TryGetVrcUserIndex(this DataDictionary dictionary, out double index, string name)
        {
            if (!dictionary.TryGetVrcUserDisplayNames(out DataList vrcUserDisplayNames))
            {
                index = default;
                return false;
            }

            index = vrcUserDisplayNames.IndexOf(name);
            return index != -1;
        }

        // Json Navigator Methods
        // vrcUserDisplayNames
        public static bool TryGetVrcUserDisplayNames(this DataDictionary dictionary, out DataList vrcUserDisplayNames)
        {
            if (!dictionary.TryGetValue("vrcUserDisplayNames", TokenType.DataList, out DataToken vrcUserDisplayNamesToken))
            {
                vrcUserDisplayNames = default;
                return false;
            }
            vrcUserDisplayNames = vrcUserDisplayNamesToken.DataList;
            return true;
        }

        // vrcGroupsById
        public static bool TryGetVrcGroupsById(this DataDictionary dictionary, out DataDictionary vrcGroupsById)
        {
            if (!dictionary.TryGetValue("vrcGroupsById", TokenType.DataDictionary, out DataToken vrcGroupsByIdToken))
            {
                vrcGroupsById = default;
                return false;
            }

            vrcGroupsById = vrcGroupsByIdToken.DataDictionary;
            return true;
        }

        // vrcGroupsById/{vrcGroupId}
        public static bool TryGetVrcGroup(this DataDictionary dictionary, out DataDictionary vrcGroup, string vrcGroupId)
        {
            if
            (
                !dictionary.TryGetVrcGroupsById(out DataDictionary vrcGroupsById) ||
                !vrcGroupsById.TryGetValue(vrcGroupId, TokenType.DataDictionary, out DataToken vrcGroupToken)
            )
            {
                vrcGroup = default;
                return false;
            }

            vrcGroup = vrcGroupToken.DataDictionary;
            return true;
        }

        // vrcGroupsById/{vrcGroupId}/name
        public static bool TryGetVrcGroupName(this DataDictionary dictionary, out string name, string vrcGroupId)
        {
            if
            (
                !dictionary.TryGetVrcGroup(out DataDictionary vrcGroup, vrcGroupId) ||
                !vrcGroup.TryGetValue("name", TokenType.String, out DataToken nameToken)
            )
            {
                name = default;
                return false;
            }

            name = nameToken.String;
            return true;
        }

        // vrcGroupsById/{vrcGroupId}/vrcUsers
        public static bool TryGetVrcGroupVrcUsers(this DataDictionary dictionary, out DataList vrcUsers, string vrcGroupId)
        {
            if
            (
                !dictionary.TryGetVrcGroup(out DataDictionary vrcGroup, vrcGroupId) ||
                !vrcGroup.TryGetValue("vrcUsers", TokenType.DataList, out DataToken vrcUsersToken)
            )
            {
                vrcUsers = default;
                return false;
            }

            vrcUsers = vrcUsersToken.DataList;
            return true;
        }

        // vrcGroupsById/{vrcGroupId}/roles
        public static bool TryGetVrcGroupRoles(this DataDictionary dictionary, out DataDictionary roles, string vrcGroupId)
        {
            if
            (
                !dictionary.TryGetVrcGroup(out DataDictionary vrcGroup, vrcGroupId) ||
                !vrcGroup.TryGetValue("roles", TokenType.DataDictionary, out DataToken rolesToken)
            )
            {
                roles = default;
                return false;
            }

            roles = rolesToken.DataDictionary;
            return true;
        }

        // vrcGroupsById/{vrcGroupId}/roles/{vrcGroupRoleId}
        public static bool TryGetVrcGroupRole(this DataDictionary dictionary, out DataDictionary role, string vrcGroupId, string vrcGroupRoleId)
        {
            if
            (
                !dictionary.TryGetVrcGroupRoles(out DataDictionary roles, vrcGroupId) ||
                !roles.TryGetValue(vrcGroupRoleId, TokenType.DataDictionary, out DataToken roleToken)
            )
            {
                role = default;
                return false;
            }

            role = roleToken.DataDictionary;
            return true;
        }

        // vrcGroupsById/{vrcGroupId}/roles/{vrcGroupRoleId}/name
        public static bool TryGetVrcGroupRoleName(this DataDictionary dictionary, out string name, string vrcGroupId, string vrcGroupRoleId)
        {
            if
            (
                !dictionary.TryGetVrcGroupRole(out DataDictionary role, vrcGroupId, vrcGroupRoleId) ||
                !role.TryGetValue("name", TokenType.String, out DataToken nameToken)
            )
            {
                name = default;
                return false;
            }

            name = nameToken.String;
            return true;
        }

        // vrcGroupsById/{vrcGroupId}/roles/{vrcGroupRoleId}/isAdmin
        public static bool TryGetVrcGroupRoleIsAdmin(this DataDictionary dictionary, out bool isAdmin, string vrcGroupId, string vrcGroupRoleId)
        {
            if
            (
                !dictionary.TryGetVrcGroupRole(out DataDictionary role, vrcGroupId, vrcGroupRoleId) ||
                !role.TryGetValue("isAdmin", TokenType.Boolean, out DataToken isAdminToken)
            )
            {
                isAdmin = default;
                return false;
            }

            isAdmin = isAdminToken.Boolean;
            return true;
        }

        // vrcGroupsById/{vrcGroupId}/roles/{vrcGroupRoleId}/isModerator
        public static bool TryGetVrcGroupRoleIsModerator(this DataDictionary dictionary, out bool isModerator, string vrcGroupId, string vrcGroupRoleId)
        {
            if
            (
                !dictionary.TryGetVrcGroupRole(out DataDictionary role, vrcGroupId, vrcGroupRoleId) ||
                !role.TryGetValue("isModerator", TokenType.Boolean, out DataToken isModeratorToken)
            )
            {
                isModerator = default;
                return false;
            }

            isModerator = isModeratorToken.Boolean;
            return true;
        }

        // vrcGroupsById/{vrcGroupId}/roles/{vrcGroupRoleId}/vrcUsers
        public static bool TryGetVrcGroupRoleVrcUsers(this DataDictionary dictionary, out DataList vrcUsers, string vrcGroupId, string vrcGroupRoleId)
        {
            if
            (
                !dictionary.TryGetVrcGroupRole(out DataDictionary role, vrcGroupId, vrcGroupRoleId) ||
                !role.TryGetValue("vrcUsers", TokenType.DataList, out DataToken vrcUsersToken)
            )
            {
                vrcUsers = default;
                return false;
            }

            vrcUsers = vrcUsersToken.DataList;
            return true;
        }

        // discordServersById
        public static bool TryGetDiscordServersById(this DataDictionary dictionary, out DataDictionary discordServersById)
        {
            if (!dictionary.TryGetValue("discordServersById", TokenType.DataDictionary, out DataToken discordServersByIdToken))
            {
                discordServersById = default;
                return false;
            }

            discordServersById = discordServersByIdToken.DataDictionary;
            return true;
        }

        // discordServersById/{discordGuildId}
        public static bool TryGetDiscordServer(this DataDictionary dictionary, out DataDictionary discordServer, string discordGuildId)
        {
            if
            (
                !dictionary.TryGetDiscordServersById(out DataDictionary discordServersById) ||
                !discordServersById.TryGetValue(discordGuildId, TokenType.DataDictionary, out DataToken discordServerToken)
            )
            {
                discordServer = default;
                return false;
            }

            discordServer = discordServerToken.DataDictionary;
            return true;
        }

        // discordServersById/{discordGuildId}/name
        public static bool TryGetDiscordServerName(this DataDictionary dictionary, out string name, string discordGuildId)
        {
            if
            (
                !dictionary.TryGetDiscordServer(out DataDictionary discordServer, discordGuildId) ||
                !discordServer.TryGetValue("name", TokenType.String, out DataToken nameToken)
            )
            {
                name = default;
                return false;
            }

            name = nameToken.String;
            return true;
        }

        // discordServersById/{discordGuildId}/vrcUsers
        public static bool TryGetDiscordServerVrcUsers(this DataDictionary dictionary, out DataList vrcUsers, string discordGuildId)
        {
            if
            (
                !dictionary.TryGetDiscordServer(out DataDictionary discordServer, discordGuildId) ||
                !discordServer.TryGetValue("vrcUsers", TokenType.DataList, out DataToken vrcUsersToken)
            )
            {
                vrcUsers = default;
                return false;
            }

            vrcUsers = vrcUsersToken.DataList;
            return true;
        }

        // discordServersById/{discordGuildId}/roles
        public static bool TryGetDiscordServerRoles(this DataDictionary dictionary, out DataDictionary roles, string discordGuildId)
        {
            if
            (
                !dictionary.TryGetDiscordServer(out DataDictionary discordServer, discordGuildId) ||
                !discordServer.TryGetValue("roles", TokenType.DataDictionary, out DataToken rolesToken)
            )
            {
                roles = default;
                return false;
            }

            roles = rolesToken.DataDictionary;
            return true;
        }

        // discordServersById/{discordGuildId}/roles/{discordRoleId}
        public static bool TryGetDiscordServerRole(this DataDictionary dictionary, out DataDictionary role, string discordGuildId, string discordRoleId)
        {
            if
            (
                !dictionary.TryGetDiscordServerRoles(out DataDictionary roles, discordGuildId) ||
                !roles.TryGetValue(discordRoleId, TokenType.DataDictionary, out DataToken roleToken)
            )
            {
                role = default;
                return false;
            }

            role = roleToken.DataDictionary;
            return true;
        }

        // discordServersById/{discordGuildId}/roles/{discordRoleId}/name
        public static bool TryGetDiscordServerRoleName(this DataDictionary dictionary, out string name, string discordGuildId, string discordRoleId)
        {
            if
            (
                !dictionary.TryGetDiscordServerRole(out DataDictionary role, discordGuildId, discordRoleId) ||
                !role.TryGetValue("name", TokenType.String, out DataToken nameToken)
            )
            {
                name = default;
                return false;
            }

            name = nameToken.String;
            return true;
        }

        // discordServersById/{discordGuildId}/roles/{discordRoleId}/isAdmin
        public static bool TryGetDiscordServerRoleIsAdmin(this DataDictionary dictionary, out bool isAdmin, string discordGuildId, string discordRoleId)
        {
            if
            (
                !dictionary.TryGetDiscordServerRole(out DataDictionary role, discordGuildId, discordRoleId) ||
                !role.TryGetValue("isAdmin", TokenType.Boolean, out DataToken isAdminToken)
            )
            {
                isAdmin = default;
                return false;
            }

            isAdmin = isAdminToken.Boolean;
            return true;
        }

        // discordServersById/{discordGuildId}/roles/{discordRoleId}/isModerator
        public static bool TryGetDiscordServerRoleIsModerator(this DataDictionary dictionary, out bool isModerator, string discordGuildId, string discordRoleId)
        {
            if
            (
                !dictionary.TryGetDiscordServerRole(out DataDictionary role, discordGuildId, discordRoleId) ||
                !role.TryGetValue("isModerator", TokenType.Boolean, out DataToken isModeratorToken)
            )
            {
                isModerator = default;
                return false;
            }

            isModerator = isModeratorToken.Boolean;
            return true;
        }

        // discordServersById/{discordGuildId}/roles/{discordRoleId}/vrcUsers
        public static bool TryGetDiscordServerRoleVrcUsers(this DataDictionary dictionary, out DataList vrcUsers, string discordGuildId, string discordRoleId)
        {
            if
            (
                !dictionary.TryGetDiscordServerRole(out DataDictionary role, discordGuildId, discordRoleId) ||
                !role.TryGetValue("vrcUsers", TokenType.DataList, out DataToken vrcUsersToken)
            )
            {
                vrcUsers = default;
                return false;
            }

            vrcUsers = vrcUsersToken.DataList;
            return true;
        }
    }
}