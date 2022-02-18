using ApplicationService;
using IApplicationService;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Oxygen.Mesh.Dapr.ActorProxyGenerator;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace GoodsActorProxyGenerator
{
    [Generator]
    public class DefaultSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var proxylist = ProxyCodeGeneratorTemplate.GetTemplate<ApiResult, EventHandler>();
            foreach (var item in proxylist)
            {
                context.AddSource(item.sourceName, SourceText.From(item.sourceCode, Encoding.UTF8));
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            //Debugger.Launch();
        }
    }
}
