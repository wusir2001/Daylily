﻿using Daylily.Bot.Backend;
using Daylily.CoolQ;
using Daylily.CoolQ.Messaging;
using Daylily.Osu;
using Daylily.Osu.Cabbage;
using Daylily.Osu.Elo;
using OSharp.Api.V1.User;
using System;
using System.Collections.Generic;
using Daylily.Bot.Messaging;
using Daylily.CoolQ.Plugin;

namespace Daylily.Plugin.Osu
{
    [Name("ELO查询")]
    [Author("yf_extension")]
    [Version(2, 0, 0, PluginVersion.Stable)]
    [Help("获取玩家的ELO信息。", "Mapping ELO Project 是以 ELO 等级分制度 来对选手在作图竞赛中的表现进行量化衡量以及排名的一个项目。", "详情：t/728158")]
    [Command("elo")]
    class Elo : CoolQCommandPlugin
    {
        public override Guid Guid => new Guid("774c4420-5a5c-4240-a742-9b030a1e989e");

        [FreeArg]
        [Help("查询指定的osu用户名。若带空格，请使用引号。")]
        public string UserName { get; set; }

        public override CoolQRouteMessage OnMessageReceived(CoolQScopeEventArgs scope)
        {
            var routeMsg = scope.RouteMessage;
            string id;
            string uname;
            OldSiteApiClient client = new OldSiteApiClient();
            if (UserName == null)
            {
                BllUserRole bllUserRole = new BllUserRole();
                List<TableUserRole> userInfo = bllUserRole.GetUserRoleByQq(long.Parse(routeMsg.UserId));
                if (userInfo.Count == 0)
                    return routeMsg.ToSource(DefaultReply.IdNotBound, true);

                id = userInfo[0].UserId.ToString();
                uname = userInfo[0].CurrentUname;
            }
            else
            {
                int userNum = client.GetUser(UserComponent.FromUserName(UserName), out var userObj);
                if (userNum == 0)
                    return routeMsg.ToSource(DefaultReply.IdNotFound, true);
                if (userNum > 1)
                {
                    // ignored
                }

                id = userObj.UserId.ToString();
                uname = userObj.UserName;
            }

            var eloInfo = EloApiClient.GetEloByUid(long.Parse(id));
            switch (eloInfo.Result.ToLower())
            {
                case "fail" when eloInfo.Message.ToLower() == "unranked":
                    return routeMsg.ToSource(uname + "大概没有参加什么mapping赛事..所以没有数据..",
                        true);
                case "fail":
                    return routeMsg.ToSource("未知错误..查询不到..", true);
                default:
                    return routeMsg.ToSource(
                        $"{eloInfo.User.Name}，有elo点{Math.Round(eloInfo.User.Elo, 2)}，当前#{eloInfo.User.Ranking}.", true);
            }
        }
    }
}
