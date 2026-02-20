namespace Teste.Infra.Services
{
    public class LogService
    {

        public static void RegistrarExclusão(int id, string nome)
        {
            string pastaDocumentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string pastaLog = Path.Combine(pastaDocumentos, "AgendaLogs");
            if (!Directory.Exists(pastaLog))
            {
                Directory.CreateDirectory(pastaLog);
            }
            string caminhoArquivo = Path.Combine(pastaLog, "contatos_log.txt");
            string mensagem = $"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] CONTATO EXCLUÍDO - ID: {id} - Nome: {nome}";
            File.AppendAllText(caminhoArquivo, mensagem + Environment.NewLine);

        }
    }
}
