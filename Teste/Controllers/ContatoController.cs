using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teste.Domain.Contato;
using Teste.Domain.Entities;

namespace Teste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContatoController : ControllerBase
    {
        private readonly IContatoService _contatoService;

        public ContatoController(IContatoService contatoService)
        {
            _contatoService = contatoService;
        }
        [HttpPost]
        public IActionResult AddContato(Contato contato)
        {
            try
            {
                return Ok(_contatoService.AddContato(contato));
            }
            catch
            {
                return BadRequest(new { erro = "Erro de alguma coisa ai q eu tenho q ver" });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetContato(int id)
        {
            try
            {
                return Ok(_contatoService.GetContatoById(id));
            }
            catch
            {
                return BadRequest(new { erro = "Erro de alguma coisa ai q eu tenho q ver" });
            }
        }
        [HttpGet]
        public IActionResult GetContatos()
        {
            try
            {
                return Ok(_contatoService.GetContatos());
            }
            catch
            {
                return BadRequest(new { erro = "Erro de alguma coisa ai q eu tenho q ver" });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateContato(int id, [fromBody]Contato contato)
        {
            try
            {
                var existe = _contatoService.GetContatoById(id);
                if (existe == null)
                    return NotFound();

                var result = _contatoService.UpdateContato(contato, id);
                return Ok(result);
            }
            catch
            {
                return BadRequest(new { erro = "Erro ao atualizar contato" });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContato(int id)
        {
            try
            {
                return Ok(_contatoService.DeleteContato(id));
            }
            catch
            {
                return BadRequest(new { erro = "Erro de alguma coisa ai q eu tenho q ver" });
            }
        }
        [HttpGet("nome/{nome}")]
        public IActionResult GetContatoByNome(string nome)
        {
            try
            {
                return Ok(_contatoService.GetContatoByNome(nome));
            }
            catch
            {
                return BadRequest(new { erro = "Erro de alguma coisa ai q eu tenho q ver" });
            }
        }
        [HttpGet("numero/{numero}")]
        public IActionResult GetContatoByNumero(string numero)
        {
            try
            {
                return Ok(_contatoService.GetContatoByNumero(numero));
            }
            catch
            {
                return BadRequest(new { erro = "Erro de alguma coisa ai q eu tenho q ver" });
            }
        }
    }
}
