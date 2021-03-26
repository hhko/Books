using Murmur;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Step_04_MessageIdEnricher
{
    public class MessageIdLogEventEnricher : ILogEventEnricher
    {
        public const string MessageIdPropertyName = "MessasgeId";

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent is null)
                throw new ArgumentNullException(nameof(logEvent));

            if (propertyFactory is null)
                throw new ArgumentNullException(nameof(propertyFactory));

            Murmur32 murmur = MurmurHash.Create32();
            byte[] bytes = Encoding.UTF8.GetBytes(logEvent.MessageTemplate.Text);
            byte[] hash = murmur.ComputeHash(bytes);

            //
            // 예. 2B048590(8자리)
            //
            string hexadecimalHash = BitConverter.ToString(hash).Replace("-", "");
            LogEventProperty eventId = propertyFactory.CreateProperty(MessageIdPropertyName, hexadecimalHash);
            logEvent.AddPropertyIfAbsent(eventId);
        }
    }
}
