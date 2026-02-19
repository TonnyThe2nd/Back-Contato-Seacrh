using Teste.Domain.Entities;
using Teste.Domain.Repositories;

namespace Teste.Domain.Contato
{
    public class ContatoService : IContatoService
    {
        private readonly IContatoRepository _contatoRepository;
        public ContatoService(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        public Entities.Contato AddContato(Entities.Contato contato)
        {
            try
            {
                return _contatoRepository.AddContato(contato);
                
            }
            catch
            {
                return null;
            }
        }

        public Entities.Contato GetContatoById(int id)
        {
            try
            {
                return _contatoRepository.GetContatoById(id);

            }
            catch
            {
                return null;
            }
        }
        public List<Entities.Contato> GetContatos()
        {
            try
            {
                return _contatoRepository.GetContatos();

            }
            catch
            {
                return null;
            }
        }
        public Entities.Contato UpdateContato(Entities.Contato contato, int id)
        {
            try
            {
                return _contatoRepository.UpdateContato(contato, id);

            }
            catch
            {
                return null;
            }
        }
        public bool DeleteContato(int id)
        {
            try
            {
                return _contatoRepository.DeleteContato(id);

            }
            catch
            {
                return false;
            }
        }

        public List<Domain.Entities.Contato> GetContatoByNome(string nome)
        {
            try
            {
                return _contatoRepository.GetContatoByNome(nome);
            }
            catch
            {
                return null;
            }
        }
        public List<Domain.Entities.Contato> GetContatoByNumero(string numero)
        {
            try
            {
                return _contatoRepository.GetContatoByNumero(numero);
            }
            catch
            {
                return null;
            }
        }
    }
}
