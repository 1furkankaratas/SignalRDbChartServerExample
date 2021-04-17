using Microsoft.AspNetCore.Builder;

namespace SignalRDbChartServerExample.Subscription.Middleware
{
    public static class DatabaseSubscriptionMiddleware
    {
        public static void UseDatabaseSubcripton<T>(this IApplicationBuilder builder,string tableName) where T:class,IDatabaseSubscription
        {
            var subscription = (T)builder.ApplicationServices.GetService(typeof(T));
            subscription.Configure(tableName);
        }
    }
}