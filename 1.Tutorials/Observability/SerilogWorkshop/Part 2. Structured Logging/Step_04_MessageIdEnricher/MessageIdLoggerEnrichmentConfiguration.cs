using Serilog;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Step_04_MessageIdEnricher
{
    public static class MessageIdLoggerEnrichmentConfiguration
    {
        public static LoggerConfiguration WithMessasgeId(this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)
                throw new ArgumentNullException(nameof(enrich));

            return enrich.With<MessageIdLogEventEnricher>();
        }
    }
}
