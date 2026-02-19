namespace Teste.Domain.Entities
{
    public class Contato
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public List<string>? Telefone { get; set; }
    }
}
