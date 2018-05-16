﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Daylily.Common.Models.CQRequest.Api
{
    public class SendPrivateMsg
    {
        [JsonProperty(PropertyName = "user_id")]
        public long UserId { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "auto_escape")]
        public bool AutoEscape { get; set; }

        public SendPrivateMsg(string userId, string message, bool autoEscape = false)
        {
            UserId = long.Parse(userId);
            Message = message;
            AutoEscape = autoEscape;
        }
    }
}
