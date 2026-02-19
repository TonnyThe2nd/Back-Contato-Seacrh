using Teste.Domain.Entities;

namespace Teste.Domain.Contato
{
    public interface IContatoService
    {
        public Entities.Contato AddContato(Entities.Contato contato);
        public Entities.Contato GetContatoById(int id);
        public List<Entities.Contato> GetContatos();
        public Entities.Contato UpdateContato(Entities.Contato contato, int id);
        public bool DeleteContato(int id);
        public List<Domain.Entities.Contato> GetContatoByNome(string nome);
        public List<Domain.Entities.Contato> GetContatoByNumero(string numero);
    }
}
