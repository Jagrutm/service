using EventBus.Messages.Events;
using MassTransit;
using System;

namespace EventBus.Messages.Common
{
    public static class EventBusConstants
    {

        public const int DefaultConcurrentMessageLimit = 8;

        #region Account Events
        public const string AccountCreate = "AccountCreate";
        public const string AccountUpdate = "AccountUpdate";
        public const string AccountDelete = "AccountDelete";
        #endregion

        #region Queues
        #region Accounts
        public const string AccountCreateQueue = "account-create-queue";
        public const string AccountUpdateQueue = "account-upadte-queue";
        public const string AccountDeleteQueue = "account-delete-queue";
        #endregion
        #endregion

        public static string GetMessage(ConsumeContext<MessageEvent> context, string queueName)
        {
            string message = $"{DateTime.Now}: Queue: {queueName}, Event: {context.Message.EventName}, {context.Message.Message}";
            //string message = $"{DateTime.Now}: Queue: {queueName}, Event: {context.Message.EventName}";
            return message;
        }
    }
}
