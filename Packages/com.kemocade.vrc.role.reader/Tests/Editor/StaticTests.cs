using NUnit.Framework;
using static NUnit.Framework.Assert;
using VRC.SDK3.Data;
using Kemocade.Vrc.Role.Reader.Extensions;

namespace Kemocade.Vrc.Role.Reader.Tests
{
    public class StaticTests
    {
        const string SAMPLE_JSON =
            @"{
            ""vrcUserDisplayNames"": [
                ""Lion"",
                ""Tiger"",
                ""Cat"",
                ""Wolf"",
                ""Cow"",
                ""Frog"",
                ""Salamander""
            ],
            ""vrcGroupsById"": {
                ""grp_aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"": {
                    ""name"": ""Mammals"",
                    ""vrcUsers"":[0,1,2,3,4],
                    ""roles"": {
                        ""grol_aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"": {
                            ""name"": ""King"",
                            ""isAdmin"": true,
                            ""isModerator"": true,
                            ""vrcUsers"": [0]
                        },
                        ""grol_bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"": {
                            ""name"": ""Prince"",
                            ""isModerator"": true,
                            ""vrcUsers"": [3]
                        },
                        ""grol_cccccccc-cccc-cccc-cccc-cccccccccccc"": {
                            ""name"": ""Carnivore"",
                            ""vrcUsers"": [0,1,2,3]
                        }
                    }
                },
                ""grp_bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"": {
                    ""name"": ""Amphibians"",
                    ""vrcUsers"":[5,6],
                    ""roles"": {
                        ""grol_dddddddd-dddd-dddd-dddd-dddddddddddd"": {
                            ""name"": ""Big"",
                            ""isAdmin"": true,
                            ""isModerator"": true,
                            ""vrcUsers"": [5]
                        }
                    }
                }
            },
            ""discordServersById"": {
                ""111111111111111111"": {
                    ""name"": ""Felines"",
                    ""vrcUsers"":[0,1,2],
                    ""roles"": {
                        ""222222222222222222"": {
                            ""name"": ""Admin"",
                            ""vrcUsers"": [0],
                            ""isAdmin"": true,
                            ""isModerator"": true
                        },
                        ""333333333333333333"": {
                            ""name"": ""Stripes"",
                            ""vrcUsers"": [1],
                            ""isModerator"": true
                        }
                    }
                },
                ""444444444444444444"": {
                    ""name"": ""Everyone"",
                    ""vrcUsers"":[0,1,2,3,4,5,6],
                    ""roles"": {}
                }
            }
        }";

        const string SAMPLE_JSON_BAD =
            @"{
                ""qwertyuiop"": [""aaaaa"",],
                ""asdfghjkl"": {
                    ""bbbbb"": {
                        ""ccccc"": ""ddddd"",
                        ""eeeee"": ""fffff"",
                    }
                }
            }";

        const string LION = "Lion";
        const string TIGER = "Tiger";
        const string CAT = "Cat";
        const string WOLF = "Wolf";
        const string COW = "Cow";
        const string FROG = "Frog";
        const string SALAMANDER = "Salamander";
        const string GROUP_MAMMALS = "grp_aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
        const string ROLE_KING = "grol_aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
        const string ROLE_PRINCE = "grol_bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb";
        const string ROLE_CARNIVORE = "grol_cccccccc-cccc-cccc-cccc-cccccccccccc";
        const string GROUP_AMPHIBIANS = "grp_bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb";
        const string ROLE_BIG = "grol_dddddddd-dddd-dddd-dddd-dddddddddddd";
        const string GUILD_FELINES = "111111111111111111";
        const string ROLE_ADMIN = "222222222222222222";
        const string ROLE_STRIPES = "333333333333333333";
        const string GUILD_EVERYONE = "444444444444444444";

        private static DataDictionary Data =>
            VRCJson.TryDeserializeFromJson(SAMPLE_JSON, out DataToken token) ?
            token.DataDictionary : null;

        private static DataDictionary DataBad =>
            VRCJson.TryDeserializeFromJson(SAMPLE_JSON_BAD, out DataToken token) ?
            token.DataDictionary : null;

        [Test]
        public void IsVrcGroupMember() =>
            True(Data.IsVrcGroupMember(LION, GROUP_MAMMALS));

        [Test]
        public void IsVrcGroupMemberFail() =>
            False(Data.IsVrcGroupMember(FROG, GROUP_MAMMALS));

        [Test]
        public void IsVrcGroupMemberFailBad() =>
            False(DataBad.IsVrcGroupMember(FROG, GROUP_MAMMALS));

        [Test]
        public void HasVrcGroupRole() =>
            True(Data.HasVrcGroupRole(LION, GROUP_MAMMALS, ROLE_KING));

        [Test]
        public void HasVrcGroupRole2() =>
            True(Data.HasVrcGroupRole(WOLF, GROUP_MAMMALS, ROLE_PRINCE));

        [Test]
        public void HasVrcGroupRoleFail() =>
            False(Data.HasVrcGroupRole(SALAMANDER, GROUP_AMPHIBIANS, ROLE_BIG));

        [Test]
        public void HasVrcGroupRoleFail2() =>
            False(Data.HasVrcGroupRole(COW, GROUP_MAMMALS, ROLE_CARNIVORE));

        [Test]
        public void HasVrcGroupRoleFailBad() =>
            False(DataBad.HasVrcGroupRole(COW, GROUP_MAMMALS, ROLE_CARNIVORE));

        [Test]
        public void IsVrcGroupAdmin() =>
            True(Data.IsVrcGroupAdmin(FROG, GROUP_AMPHIBIANS));

        [Test]
        public void IsVrcGroupAdminFail() =>
            False(Data.IsVrcGroupAdmin(CAT, GROUP_AMPHIBIANS));

        [Test]
        public void IsVrcGroupAdminFailBad() =>
            False(DataBad.IsVrcGroupAdmin(CAT, GROUP_AMPHIBIANS));

        [Test]
        public void IsVrcGroupModerator() =>
            True(Data.IsVrcGroupModerator(WOLF, GROUP_MAMMALS));

        [Test]
        public void IsVrcGroupModeratorFail() =>
            False(Data.IsVrcGroupModerator(CAT, GROUP_MAMMALS));

        [Test]
        public void IsVrcGroupModeratorFailBad() =>
            False(DataBad.IsVrcGroupModerator(CAT, GROUP_MAMMALS));

        [Test]
        public void IsDiscordMember() =>
            True(Data.IsDiscordMember(COW, GUILD_EVERYONE));

        [Test]
        public void IsDiscordMemberFail() =>
            False(Data.IsDiscordMember(SALAMANDER, GUILD_FELINES));

        [Test]
        public void IsDiscordMemberFailBad() =>
            False(DataBad.IsDiscordMember(SALAMANDER, GUILD_FELINES));

        [Test]
        public void HasDiscordRole() =>
            True(Data.HasDiscordRole(TIGER, GUILD_FELINES, ROLE_STRIPES));

        [Test]
        public void HasDiscordRoleFail() =>
            False(Data.HasDiscordRole(CAT, GUILD_FELINES, ROLE_ADMIN));

        [Test]
        public void HasDiscordRoleFailBad() =>
            False(DataBad.HasDiscordRole(CAT, GUILD_FELINES, ROLE_ADMIN));

        [Test]
        public void IsDiscordAdmin() =>
            True(Data.IsDiscordAdmin(LION, GUILD_FELINES));

        [Test]
        public void IsDiscordAdminFail() =>
            False(Data.IsDiscordAdmin(WOLF, GUILD_FELINES));

        [Test]
        public void IsDiscordAdminFailBad() =>
            False(DataBad.IsDiscordAdmin(WOLF, GUILD_FELINES));

        [Test]
        public void IsDiscordModerator() =>
            True(Data.IsDiscordModerator(LION, GUILD_FELINES));

        [Test]
        public void IsDiscordModeratorFail() =>
            False(Data.IsDiscordModerator(WOLF, GUILD_FELINES));

        [Test]
        public void IsDiscordModeratorFailBad() =>
            False(DataBad.IsDiscordModerator(WOLF, GUILD_FELINES));

        [Test]
        public void IsAdminAnywhere() =>
            True(Data.IsAdminAnywhere(LION));

        [Test]
        public void IsAdminAnywhereFail() =>
            False(Data.IsAdminAnywhere(COW));

        [Test]
        public void IsAdminAnywhereFailBad() =>
            False(DataBad.IsAdminAnywhere(COW));

        [Test]
        public void IsModeratorAnywhere() =>
            True(Data.IsModeratorAnywhere(TIGER));

        [Test]
        public void IsModeratorAnywhereFail() =>
            False(Data.IsModeratorAnywhere(SALAMANDER));

        [Test]
        public void IsModeratorAnywhereFailBad() =>
            False(DataBad.IsModeratorAnywhere(SALAMANDER));

        // Bad Json Data Navigation Tests
        [Test]
        public void TryGetVrcUserDisplayNamesFailBad() =>
            False(DataBad.TryGetVrcUserDisplayNames(out _));

        [Test]
        public void TryGetVrcGroupsByIdFailBad() =>
            False(DataBad.TryGetVrcGroupsById(out _));

        [Test]
        public void TryGetVrcGroupFailBad() =>
            False(DataBad.TryGetVrcGroup(out _, GROUP_AMPHIBIANS));

        [Test]
        public void TryGetVrcGroupName() =>
            True
            (
                Data.TryGetVrcGroupName(out string name, GROUP_AMPHIBIANS) &&
                name == "Amphibians"
            );

        [Test]
        public void TryGetVrcGroupNameFailBad() =>
            False(DataBad.TryGetVrcGroupName(out _, GROUP_AMPHIBIANS));

        [Test]
        public void TryGetVrcGroupVrcUsersFailBad() =>
            False(DataBad.TryGetVrcGroupVrcUsers(out _, GROUP_AMPHIBIANS));

        [Test]
        public void TryGetVrcGroupRolesFailBad() =>
            False(DataBad.TryGetVrcGroupRoles(out _, GROUP_AMPHIBIANS));

        [Test]
        public void TryGetVrcGroupRoleFailBad() =>
            False(DataBad.TryGetVrcGroupRole(out _, GROUP_AMPHIBIANS, ROLE_BIG));

        [Test]
        public void TryGetVrcGroupRoleNameFailBad() =>
            False(DataBad.TryGetVrcGroupRoleName(out _, GROUP_AMPHIBIANS, ROLE_BIG));

        [Test]
        public void TryGetVrcGroupRoleName() =>
            True
            (
                Data.TryGetVrcGroupRoleName(out string name, GROUP_AMPHIBIANS, ROLE_BIG) &&
                name == "Big"
            );

        [Test]
        public void TryGetVrcGroupRoleIsAdminFailBad() =>
            False(DataBad.TryGetVrcGroupRoleIsAdmin(out _, GROUP_AMPHIBIANS, ROLE_BIG));

        [Test]
        public void TryGetVrcGroupRoleIsModeratorFailBad() =>
            False(DataBad.TryGetVrcGroupRoleIsModerator(out _, GROUP_AMPHIBIANS, ROLE_BIG));

        [Test]
        public void TryGetVrcGroupRoleVrcUsersFailBad() =>
            False(DataBad.TryGetVrcGroupRoleVrcUsers(out _, GROUP_AMPHIBIANS, ROLE_BIG));

        [Test]
        public void TryGetDiscordServersByIdFailBad() =>
            False(DataBad.TryGetDiscordServersById(out _));

        [Test]
        public void TryGetDiscordServerFailBad() =>
            False(DataBad.TryGetDiscordServer(out _, GUILD_FELINES));

        [Test]
        public void TryGetDiscordServerNameFailBad() =>
            False(DataBad.TryGetDiscordServerName(out _, GUILD_FELINES));

        [Test]
        public void TryGetDiscordServerName() =>
            True
            (
                Data.TryGetDiscordServerName(out string name, GUILD_EVERYONE) &&
                name == "Everyone"
            );

        [Test]
        public void TryGetDiscordServerVrcUsersFailBad() =>
            False(DataBad.TryGetDiscordServerVrcUsers(out _, GUILD_FELINES));

        [Test]
        public void TryGetDiscordServerRolesFailBad() =>
            False(DataBad.TryGetDiscordServerRoles(out _, GUILD_FELINES));

        [Test]
        public void TryGetDiscordServerRoleFailBad() =>
            False(DataBad.TryGetDiscordServerRole(out _, GUILD_FELINES, ROLE_STRIPES));

        [Test]
        public void TryGetDiscordServerRoleNameFailBad() =>
            False(DataBad.TryGetDiscordServerRoleName(out _, GUILD_FELINES, ROLE_STRIPES));

        [Test]
        public void TryGetDiscordServerRoleName() =>
            True
            (
                Data.TryGetDiscordServerRoleName(out string name, GUILD_FELINES, ROLE_STRIPES) &&
                name == "Stripes"
            );

        [Test]
        public void TryGetDiscordServerRoleIsAdminFailBad() =>
            False(DataBad.TryGetDiscordServerRoleIsAdmin(out _, GUILD_FELINES, ROLE_STRIPES));

        [Test]
        public void TryGetDiscordServerRoleIsModeratorFailBad() =>
            False(DataBad.TryGetDiscordServerRoleIsModerator(out _, GUILD_FELINES, ROLE_STRIPES));

        [Test]
        public void TryGetDiscordServerRoleVrcUsersFailBad() =>
            False(DataBad.TryGetDiscordServerRoleVrcUsers(out _, GUILD_FELINES, ROLE_STRIPES));
    }
}
