using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureBase
{
    public class ApplicationServiceException : Exception
    {
        public ApplicationServiceException(string message) : base(message)
        {

        }
    }
}
