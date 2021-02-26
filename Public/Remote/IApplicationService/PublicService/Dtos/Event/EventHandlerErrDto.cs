using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IApplicationService.PublicService.Dtos.Event
{
    public class EventHandlerErrDto
    { 
        public EventHandlerErrDto() { }
        public EventHandlerErrDto(string handlerName, string eventData, string errMessage, string errStackTrace, bool IsSystemErr)
        {
            this.HandlerName = handlerName;
            this.EventData = eventData;
            this.ErrMessage = errMessage;
            this.ErrStackTrace = errStackTrace;
            this.IsSystemErr = IsSystemErr;
        }
        public string HandlerName { get; set; }
        public string ErrMessage { get; set; }
        public string ErrStackTrace { get; set; }
        public bool IsSystemErr { get; set; }
        public string EventData { get; set; }
    }
}
