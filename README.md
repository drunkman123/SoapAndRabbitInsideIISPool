# SoapAndRabbitInsideIISPool
## Descrição do Projeto
Uma API que faz requisições via SOAP, envia para fila num rabbitMq local e depois insere dados num SQL
### Tecnologias utilizadas
C# .NET 6, SQL, SOAP, RabbitMQ
### Desafios e Futuro
O uso de uma API em loop para "ativar" o handler do RabbitMQ foi motivado pois o servidor de hospedagem não possui docker e só permite inclusão no IIS numa AppPool.
Este projeto meu primeiro usando RabbitMQ, que vale dizer que tudo que fiz foi aprendido em apenas 1 dia.



Esta parte do código serve para limitar 1 request apenas por endpoint, bastando usar o decorado [ExclusiveAction] em cada método da controller (visto que os 2 entrarão em loop, ninguém mais conseguirá chamar outra instância)
```
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
```



A instância do RabbitMQ está configurada com exchange de dead-letter para receber as mensagens com falha (Nack).



### Modo de Uso
Basta fazer 2 requests, um cada endpoint, que a aplicação entrará em 2 loops distintos.
