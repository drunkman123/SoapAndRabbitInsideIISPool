using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SrvIDO.DATA.Interfaces;
using System.Net;

namespace SrvIdo.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExclusiveActionAttribute : ActionFilterAttribute
    {
        private static int _isExecuting = 0;
        private static int _isDuplicated = 0;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Interlocked.CompareExchange(ref _isExecuting, 1, 0) == 0)
            {
                base.OnActionExecuting(filterContext);
                return;
            }
            Interlocked.Exchange(ref _isDuplicated, 1);
            filterContext.Result = new StatusCodeResult((int)HttpStatusCode.ServiceUnavailable);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            if (_isDuplicated == 1)
            {
                Interlocked.Exchange(ref _isDuplicated, 0);
                return;
            }
            Interlocked.Exchange(ref _isExecuting, 0);
        }
    }

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IDOController : ControllerBase
    {
        #region CONSTRUTOR

        private readonly IIDOService _IDOService;
        private readonly GetErro _getErro;


        public IDOController(IIDOService IDOService, GetErro getErro)
        {
            _IDOService = IDOService;
            _getErro = getErro;
        }
        #endregion



        [ExclusiveAction]
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> EnviaIDO()
        {
            while (1 > 0)
            {
                try
                {                
                    await _IDOService.EnviaIDO();
                    new ManualResetEvent(false).WaitOne(TimeSpan.FromMinutes(60));
                }
                catch (Exception ex)
                {
                    _getErro.LogErro(ex);
                }
            }
        }

        [ExclusiveAction]
        [HttpGet]
        public void UpdateIDO()
        {
            _IDOService.UpdateIDO();
        }
    }
}
