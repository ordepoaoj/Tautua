using BLuDataAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BLuDataAPI.Controllers
{
    [Route("api/pessoa")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly Contexto _context;

        public PessoaController(Contexto context)
        {
            _context = context;
        }

        [HttpGet("{cpf}")]
        public async Task<ActionResult<Pessoa>> GetPessoa(string cpf)
        {
            if (Validador.ValidarCpf(cpf))
            {
                var pessoa = await _context.Pessoa.Where(p => p.Cpf == cpf).Include(e => e.CdUfNavigation).Include(p => p.Telefones).FirstOrDefaultAsync();

                if (pessoa == null)
                {
                    return NotFound();
                }

                return pessoa;
            }
            return Ok("CPF no formato invalido");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(int id, Pessoa pessoa)
        {
            if (id != pessoa.Id)
            {
                return BadRequest();
            }

            _context.Entry(pessoa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PessoaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Pessoa>> PostPessoa(Pessoa pessoa)
        {
            if (Validador.ValidarCpf(pessoa.Cpf))
            {
                _context.Pessoa.Add(pessoa);
                await _context.SaveChangesAsync();
                return Ok(pessoa);
            }
            return NotFound("CPF no formato incorreto");
            
        }

        // DELETE: api/Pessoa/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _context.Pessoa.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            _context.Pessoa.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PessoaExists(int id)
        {
            return _context.Pessoa.Any(e => e.Id == id);
        }
    }
}
