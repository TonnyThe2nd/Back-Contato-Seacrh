using Teste.Domain.Entities;

namespace Teste.Domain.Repositories
{
    public interface IContatoRepository
    {
        Entities.Contato AddContato(Entities.Contato contato);
        Entities.Contato GetContatoById(int id);
        List<Entities.Contato> GetContatos();
        Entities.Contato UpdateContato(Entities.Contato contato, int id);
        bool DeleteContato(int id);
        public List<Domain.Entities.Contato> GetContatoByNome(string nome);
        public List<Domain.Entities.Contato> GetContatoByNumero(string numero);
    }
}
