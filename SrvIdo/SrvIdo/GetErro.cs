namespace SrvIdo
{
    public class GetErro
    {
        private readonly IHostEnvironment _hostingEnvironment;

        public GetErro(IHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        public void LogErro(Exception ex)
        {
            string Folder = _hostingEnvironment.ContentRootPath + @"\Erro\erro " + (DateTime.Now).ToString().Replace("/", "_").Replace(":", ".") + ".txt";
            string erro = ex.ToString() + " " + DateTime.Now;
            if (!File.Exists(Folder))
            {
                File.WriteAllText(Folder, erro);
            }
        }
    }
}
