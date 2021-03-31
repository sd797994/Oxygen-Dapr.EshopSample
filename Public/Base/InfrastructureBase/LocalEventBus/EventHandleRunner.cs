using Autofac;
using Autofac.Core.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace InfrastructureBase
{
    public static class EventHandleRunner
    {
        public static void RegisterAndRun<Tsrv, Tin>(ILifetimeScope lifetimeScope, string topic, string methodName)
        {
            Channel<Tin> channel = Channel.CreateUnbounded<Tin>();
            var eventdelegate = CreateMethodDelegate<Tsrv, Tin>(typeof(Tsrv).GetMethod(methodName));
            _ = SubscribeHandleInvoke((lifetimeScope as LifetimeScope).RootLifetimeScope, channel, eventdelegate);
            ChannelDictionary.SetChannel(topic, channel);
        }
        static async Task SubscribeHandleInvoke<Tobj, Tin>(ILifetimeScope lifetimeScope, Channel<Tin> pipline, Func<Tobj, Tin, Task> eventHandler)
        {
            while (await pipline.Reader.WaitToReadAsync())
            {
                try
                {
                    if (pipline.Reader.TryRead(out Tin data))
                        await eventHandler(lifetimeScope.Resolve<Tobj>(), data);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException?.Message ?? e.Message);
                }
            }
        }
        static Func<Tobj, Tin, Task> CreateMethodDelegate<Tobj, Tin>(MethodInfo method)
        {
            var mParameter = Expression.Parameter(typeof(Tobj), "m");
            var pParameter = Expression.Parameter(typeof(Tin), "p");
            var mcExpression = Expression.Call(mParameter, method, Expression.Convert(pParameter, typeof(Tin)));
            var reExpression = Expression.Convert(mcExpression, typeof(Task));
            return Expression.Lambda<Func<Tobj, Tin, Task>>(reExpression, mParameter, pParameter).Compile();
        }
    }
}
