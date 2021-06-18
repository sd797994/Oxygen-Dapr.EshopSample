using System.Threading.Tasks;

namespace InfrastructureBase.AopFilter
{
    public interface IAopMethodFilter
    {
        Task OnMethodExecuting(object param);
        Task OnMethodExecuted(object result);
    }
}
