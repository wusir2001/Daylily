﻿using Daylily.Bot.Backend;
using Daylily.Common.Logging;
using Daylily.CoolQ;
using Daylily.CoolQ.Messaging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Daylily.Bot.Messaging;
using Daylily.CoolQ.Plugin;

namespace Daylily.Plugin.Fun
{
    [Name("复读")]
    [Author("yf_extension")]
    [Version(2, 0, 1, PluginVersion.Beta)]
    [Help("按一定条件触发复读。")]
    public class RepeatApp : CoolQApplicationPlugin
    {
        public override Guid Guid => new Guid("c96921e9-7f05-4c15-ba92-e0cc3932d2e3");

        private static readonly ConcurrentDictionary<string, GroupSettings> GroupDic = new ConcurrentDictionary<string, GroupSettings>();
        private const int MaxNum = 10;

        public override CoolQRouteMessage OnMessageReceived(CoolQScopeEventArgs scope)
        {
            var routeMsg = scope.RouteMessage;
            if (routeMsg.MessageType == MessageType.Private)
                return null;
            string groupId = routeMsg.GroupId ?? routeMsg.DiscussId;

            if (!GroupDic.ContainsKey(groupId))
            {
                GroupDic.GetOrAdd(groupId, new GroupSettings
                {
                    GroupId = groupId,
                });

                GroupDic[groupId].Task = Task.Run(DecreaseQueue);
                //GroupDic[groupId].Thread.Start();
            }

            if (GroupDic[groupId].IntQueue >= MaxNum && !GroupDic[groupId].Locked)
            {
                GroupDic[groupId].Locked = true;
                Logger.Debug(groupId + " locked");
                Logger.Success(groupId + "的" + routeMsg.UserId + "触发了复读");
                Thread.Sleep(StaticRandom.Next(1000, 8000));
                return routeMsg.ToSource(routeMsg.RawMessage);
            }

            GroupDic[groupId].IntQueue++;
            //Logger.Debug(groupId + " incresed to " + GroupDic[groupId].IntQueue);
            return null;

            Task DecreaseQueue()
            {
                while (true)
                {
                    Thread.Sleep(StaticRandom.Next(1000, 10000));
                    if (GroupDic[groupId].IntQueue <= 0)
                    {
                        if (GroupDic[groupId].Locked)
                        {
                            GroupDic[groupId].Locked = false;
                            Logger.Debug(groupId + " unlocked");
                        }

                        continue;
                    }

                    if (StaticRandom.NextDouble() < 0.02) Thread.Sleep(StaticRandom.Next(30000, 45000));

                    GroupDic[groupId].IntQueue--;
                    //Logger.Debug(groupId + " decresed to " + GroupDic[groupId].IntQueue);
                }
            }
        }

        private class GroupSettings
        {
            public string GroupId { get; set; }
            public int IntQueue { get; set; }
            public Task Task { get; set; }
            public bool Locked { get; set; } = false;
        }

    }
}
