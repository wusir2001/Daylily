﻿using Daylily.Bot.Models;
using Daylily.CoolQ.Models.CqResponse;
using System;
using System.Collections.Generic;

namespace Daylily.Bot
{
    public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs args);
    public delegate void SessionReceivedEventHandler(object sender, SessionReceivedEventArgs args);

    public class SessionReceivedEventArgs : EventArgs
    {
        public CommonMessage MessageObj { get; set; }
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public Msg MessageObj { get; set; }
    }

    public delegate void MessageEventHandler(object sender, MessageEventArgs args);
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(object parsedObject)
        {
            ParsedObject = parsedObject;
        }

        public object ParsedObject { get; }
    }
    public delegate void NoticeEventHandler(object sender, NoticeEventArgs args);
    public class NoticeEventArgs : EventArgs
    {
        public NoticeEventArgs(object parsedObject)
        {
            ParsedObject = parsedObject;
        }
        public object ParsedObject { get; }
    }
    public delegate void RequestEventHandler(object sender, RequestEventArgs args);
    public class RequestEventArgs : EventArgs
    {
        public RequestEventArgs(object parsedObject)
        {
            ParsedObject = parsedObject;
        }

        public object ParsedObject { get; }
    }
    public delegate void ExceptionEventHandler(object sender, ExceptionEventArgs args);
    public class ExceptionEventArgs : EventArgs
    {
        public ExceptionEventArgs(Exception e)
        {
            Exception = e;
        }
        public Exception Exception { get; }
    }

    public class BindingFailedEventArgs : EventArgs
    {
        public BindingFailedEventArgs(List<BindingFailedItem> failedItems)
        {
            FailedItems = failedItems;
        }
        public List<BindingFailedItem> FailedItems { get; }

        public struct BindingFailedItem
        {
            public string Parameter { get; set; }
            public string Value { get; set; }
            public string Reason { get; set; }
        }
    }
}
