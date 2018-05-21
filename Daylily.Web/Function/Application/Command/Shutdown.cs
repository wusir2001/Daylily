﻿using Daylily.Common.Models;
using System;

namespace Daylily.Web.Function.Application.Command
{
    public class Shutdown : AppConstruct
    {
        public override CommonMessageResponse Execute(CommonMessage message)
        {
            if (message.PermissionLevel != PermissionLevel.Root)
                return new CommonMessageResponse("不存在的", message, true);
            Environment.Exit(0);
            return null;
        }
    }
}
