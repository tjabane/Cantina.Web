using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Infrastructure.Metrics
{
    public class ReviewsMeter
    {
        private Counter<int> ReviewsAddedCounter { get; }
        public ReviewsMeter(IMeterFactory meterFactory, IConfiguration configuration)
        {
            var applicationName = configuration["ApplicationName"] ?? "The Cantina";
            var meter = meterFactory.Create($"{applicationName}.Metrics.ReviewsMeter");
            ReviewsAddedCounter = meter.CreateCounter<int>("reviews-added", "count", "The number of reviews added");
        }
        public void IncrementReviewsAddedCounter()
        {
            ReviewsAddedCounter.Add(1);
        }
    }
}
