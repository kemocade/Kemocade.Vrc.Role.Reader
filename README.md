# Kemocade VRC Role Reader

[![VPM Package Version](https://img.shields.io/vpm/v/com.kemocade.vrc.role.reader?repository_url=https%3A%2F%2Fkemocade.github.io%2FKemocade.Vrc.Role.Reader%2Findex.json)](https://kemocade.github.io/Kemocade.Vrc.Role.Reader)
[![Code Coverage](https://kemocade.github.io/Kemocade.Vrc.Role.Reader/coverage/badge_shieldsio_linecoverage_brightgreen.svg)](https://kemocade.github.io/Kemocade.Vrc.Role.Reader/coverage)

-----
# Introduction

Video Introduction on YouTube:

[![Intro Video](http://img.youtube.com/vi/YedZa5gvlZg/0.jpg)](http://www.youtube.com/watch?v=YedZa5gvlZg "Introduction")

This package exposes user membership and roles from VRChat Groups and/or Discord Servers to Udon, which are not otherwise possible to access.
It accomplishes this by pairing with a companion GitHub Actions workflow, which uses the [VRChat API](https://vrchatapi.github.io) and/or a [Discord Bot](https://discord.com/developers/docs/intro) to gather the desired data.
That information is then published to [GitHub Pages](https://pages.github.com/), where it can then be consumed by this package via [Remote String Loading](https://creators.vrchat.com/worlds/udon/string-loading).

# Prerequisites
Before using this package, you must configure your own instance of the [Kemocade VRC Role Tracker](https://github.com/kemocade/Kemocade.Vrc.Role.Tracker) system.
After configuring it to track your desired group(s), you can use this package to access the tracked data.

# Installation
Install via the [VCC Package Listing](https://kemocade.github.io/Kemocade.Vrc.Role.Reader).

# Usage
Add the provided [Reader](Packages/com.kemocade.vrc.role.reader/Runtime/Reader.cs) `Component` to any `GameObject` in your scene.
Connect to your tracked data by using the inspector to set the `Data Url` property to your GitHub Pages `data.json` Url from the [final step of configuring your Kemocade VRC Role Tracker](https://github.com/kemocade/Kemocade.Vrc.Role.Tracker#7-get-the-results) instance.

`Reader` provides a `bool` property named `IsLoaded` which determines if the remote string loading process has been completed.
All of the following methods will return `false` until this process has finished, so you should always check `IsLoaded` before using them.

If you want to execute code immediately upon this loading process completing, see [Inheritance](#inheritance).

You can use the following methods to check information about users by passing in their name:

| **Method** | **Parameters** | **Returns** | **Description** |
| - | - | - | - |
| `IsVrcGroupMember` | `string name`, `string vrcGroupId` | `bool` | If the given user is a member of the given VRC Group |
| `HasVrcGroupRole` | `string name`, `string vrcGroupId`, `string vrcGroupRoleId` | `bool` | If the given user has the given role in the given VRC Group |
| `IsVrcGroupAdmin` | `string name`, `string vrcGroupId` | `bool` | If the given user is the owner of the given VRC Group |
| `IsVrcGroupModerator` | `string name`, `string vrcGroupId` | `bool` | If the given user has any role with instance moderation permissions in the VRC Group |
| `IsDiscordMember` | `string name`, `string discordGuildId` | `bool` | If the given user is a member of the given Discord Server |
| `HasDiscordRole` | `string name`, `string discordGuildId`, `string discordRoleId` | `bool` | If the given user has the given role in the given Discord Server |
| `IsDiscordAdmin` | `string name`, `string discordGuildId` | `bool` | If the given user has any role with Admin permissions in the given Discord Server |
| `IsDiscordModerator` | `string name`, `string discordGuildId` | `bool` | If the given user has any role with Moderation permissions in the given Discord Server |
| `IsAdminAnywhere` | `string name` | `bool` | If the given user has any role with Admin permissions across any tracked VRC Groups or Discord Servers |
| `IsModeratorAnywhere` | `string name` | `bool` | If the given user has any role with Moderation permissions across any tracked VRC Groups or Discord Servers |

These methods also come in local variants that check the local user directly:

| **Method** | **Parameters** | **Returns** | **Description** |
| - | - | - | - |
| `IsVrcGroupMemberLocal` | `string vrcGroupId` | `bool` | If the local user is a member of the given VRC Group |
| `HasVrcGroupRoleLocal` | `string vrcGroupId`, `string vrcGroupRoleId` | `bool` | If the local user has the given role in the given VRC Group |
| `IsVrcGroupAdminLocal` | `string vrcGroupId` | `bool` | If the local user is the owner of the given VRC Group |
| `IsVrcGroupModeratorLocal` | `string vrcGroupId` | `bool` | If the local user has any role with instance moderation permissions in the VRC Group |
| `IsDiscordMemberLocal` | `string discordGuildId` | `bool` | If the local user is a member of the given Discord Server |
| `HasDiscordRoleLocal` | `string discordGuildId`, `string discordRoleId` | `bool` | If the local user has the given role in the given Discord Server |
| `IsDiscordAdminLocal` | `string discordGuildId` | `bool` | If the local user has any role with Admin permissions in the given Discord Server |
| `IsDiscordModeratorLocal` | `string discordGuildId` | `bool` | If the local user has any role with Moderation permissions in the given Discord Server |
| `IsAdminAnywhereLocal` | | `bool` | If the local user has any role with Admin permissions across any tracked VRC Groups or Discord Servers |
| `IsModeratorAnywhereLocal` | | `bool` | If the local user has any role with Moderation permissions across any tracked VRC Groups or Discord Servers |

# Inheritance

You can make your own [subclass](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/tutorials/inheritance) of the `Reader` component to hook into the remote string loading process and execute code as soon as the data is ready.
This is useful for quickly cacheing data to be used later, such as whether or not the local user is a Moderator.

Here is an example of a `Reader` subclass named `ReaderTest` which logs whether or not the local user is a Moderator as soon as the `Reader` is ready:

```csharp
using Kemocade.Vrc.Role.Reader;
using UnityEngine;

public class ReaderTest : Reader
{
    public override void OnLoaded() => Debug.Log(IsModeratorAnywhereLocal());
}
```
