using System.Text;
using System.Transactions;
using Dapper;
using Teste.Domain.Entities;
using Teste.Domain.Repositories;
using Teste.Infra.Data;

namespace Teste.Infra.Repositories
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly IUnityOfWork _uow;

        public ContatoRepository(IUnityOfWork uow) { _uow = uow; }

        public Domain.Entities.Contato AddContato(Domain.Entities.Contato contato)
        {
            try
            {
                const string sqlContato = @"
                        insert into dbo.contato (nome, idade) 
                        values (@nome, @idade); 
                        select cast(scope_identity() as bigint);";

                long contatoId = _uow.GetConnection().QuerySingle<long>(
                    sqlContato,
                    new { nome = contato.Nome, idade = contato.Idade },
                    _uow.GetTransaction()
                );

                foreach (var numero in contato.Telefone)
                {
                    _uow.GetConnection().Execute(
                        "insert into dbo.Telefone (IdContato, Numero) VALUES (@IdContato, @Numero)",
                        new { IdContato = contatoId, Numero = numero },
                        _uow.GetTransaction()
                    );
                }

                _uow.CommitTransaction();

                var contatoCriado = new Domain.Entities.Contato
                {
                    Id = contatoId,
                    Nome = contato.Nome,
                    Idade = contato.Idade,
                    Telefone = contato.Telefone
                };

                return contatoCriado;
            }
            catch
            {
                _uow.RollBack();
                return null;
            }
        }


        public Contato GetContatoById(int id)
        {
            var contato = _uow.GetConnection().QuerySingleOrDefault<Contato>(
                "SELECT Id, Nome, Idade FROM Contato WHERE Id = @Id",
                new { Id = id }
            );

            if (contato == null)
                return null;

            var telefones = _uow.GetConnection().Query<string>(
                "SELECT Numero FROM Telefone WHERE IdContato = @Id",
                new { Id = id }
            ).ToList();

            contato.Telefone = telefones;

            return contato;
        }

        public List<Domain.Entities.Contato> GetContatos()
        {
            try
            {
                const string sql = @"
            select 
                c.Id, 
                c.Nome, 
                c.Idade,
                t.Id as TelefoneId,
                t.Numero
            from dbo.contato c
            left join dbo.telefone t ON t.idContato = c.id
            order by c.Id";

                var contatoDict = new Dictionary<long, Domain.Entities.Contato>();

                var lista = _uow.GetConnection().Query<Domain.Entities.Contato, TelefoneAux, Domain.Entities.Contato>(
                    sql,
                    (contato, telefone) =>
                    {
                        if (!contatoDict.TryGetValue(contato.Id, out var contatoEntry))
                        {
                            contatoEntry = contato;
                            contatoEntry.Telefone = new List<string>();
                            contatoDict.Add(contatoEntry.Id, contatoEntry);
                        }

                        if (telefone != null && !string.IsNullOrEmpty(telefone.Numero))
                        {
                            contatoEntry.Telefone.Add(telefone.Numero);
                        }

                        return contatoEntry;
                    },
                    splitOn: "TelefoneId"
                );

                return contatoDict.Values.ToList();
            }
            catch
            {
                return null;
            }
        }


        public Domain.Entities.Contato UpdateContato(Domain.Entities.Contato contato, int id)
        {
            try
            {
                var sql = new StringBuilder();
                sql.Append("UPDATE Contato SET ");

                var parametros = new DynamicParameters();
                parametros.Add("Id", id);

                var updates = new List<string>();

                if (contato.Idade != null)
                {
                    updates.Add("Idade = @Idade");
                    parametros.Add("Idade", contato.Idade);
                }

                if (!string.IsNullOrEmpty(contato.Nome))
                {
                    updates.Add("Nome = @Nome");
                    parametros.Add("Nome", contato.Nome);
                }

                if (updates.Any())
                {
                    sql.Append(string.Join(", ", updates));
                    sql.Append(" WHERE Id = @Id");

                    _uow.GetConnection().Execute(sql.ToString(), parametros, _uow.GetTransaction());
                }

                _uow.GetConnection().Execute(
                    "DELETE FROM Telefone WHERE IdContato = @Id",
                    new { Id = id },
                    _uow.GetTransaction()
                );

                if (contato.Telefone != null && contato.Telefone.Any())
                {
                    foreach (var numero in contato.Telefone)
                    {
                        _uow.GetConnection().Execute(
                            "INSERT INTO Telefone (IdContato, Numero) VALUES (@Id, @Numero)",
                            new { Id = id, Numero = numero },
                            _uow.GetTransaction()
                        );
                    }
                }

                _uow.CommitTransaction();

                return GetContatoById(id);
            }
            catch
            {
                _uow.RollBack();
                return null;
            }
        }


        public bool DeleteContato(int id)
        {
            try
            {
                _uow.GetConnection().Execute(
                    "DELETE FROM Telefone WHERE IdContato = @Id",
                    new { Id = id },
                    _uow.GetTransaction()
                );

                _uow.GetConnection().Execute(
                    "DELETE FROM Contato WHERE Id = @Id",
                    new { Id = id },
                    _uow.GetTransaction()
                );

                _uow.CommitTransaction();
                return true;
            }
            catch
            {
                _uow.RollBack();
                return false;
            }
        }


        public List<Domain.Entities.Contato> GetContatoByNome(string nome)
        {
            try
            {
                var sql = @"
                    SELECT c.Id, c.Nome, c.Idade,
                           t.Numero
                    FROM Contato c
                    LEFT JOIN Telefone t ON t.IdContato = c.Id
                    WHERE c.Nome LIKE @Nome";

                var contatoDict = new Dictionary<long, Domain.Entities.Contato>();

                var lista = _uow.GetConnection().Query<Domain.Entities.Contato, string, Domain.Entities.Contato>(
                    sql,
                    (contato, numero) =>
                    {
                        if (!contatoDict.TryGetValue(contato.Id, out var contatoEntry))
                        {
                            contatoEntry = contato;
                            contatoEntry.Telefone = new List<string>();
                            contatoDict.Add(contato.Id, contatoEntry);
                        }

                        if (!string.IsNullOrEmpty(numero))
                            contatoEntry.Telefone.Add(numero);

                        return contatoEntry;
                    },
                    new { Nome = $"%{nome}%" },
                    splitOn: "Numero"
                );

                return contatoDict.Values.ToList();
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
                var sql = @"
                    SELECT c.Id, c.Nome, c.Idade,
                           t.Numero
                    FROM Contato c
                    LEFT JOIN Telefone t ON t.IdContato = c.Id
                    WHERE t.Numero LIKE @numero";

                var contatoDict = new Dictionary<long, Domain.Entities.Contato>();

                var lista = _uow.GetConnection().Query<Domain.Entities.Contato, string, Domain.Entities.Contato>(
                    sql,
                    (contato, numero) =>
                    {
                        if (!contatoDict.TryGetValue(contato.Id, out var contatoEntry))
                        {
                            contatoEntry = contato;
                            contatoEntry.Telefone = new List<string>();
                            contatoDict.Add(contato.Id, contatoEntry);
                        }

                        if (!string.IsNullOrEmpty(numero))
                            contatoEntry.Telefone.Add(numero);

                        return contatoEntry;
                    },
                    new { Numero = $"%{numero}%" },
                    splitOn: "Numero"
                );

                return contatoDict.Values.ToList();
            }
            catch
            {
                return null;
            }
        }
        public class TelefoneAux
        {
            public long TelefoneId { get; set; }
            public string Numero { get; set; }
        }

    }
}
