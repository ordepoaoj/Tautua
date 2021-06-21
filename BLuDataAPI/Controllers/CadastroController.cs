using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLuDataAPI.Models;

namespace BLuDataAPI.Controllers
{
    [Route("api/cliente")]
    [ApiController]
    public class CadastroController : ControllerBase
    {
        private readonly Contexto _context;

        public CadastroController(Contexto context)
        {
            _context = context;
        }

        [HttpGet("{cnpj}")]
        public async Task<ActionResult<Cadastro>> GetCadastro(string cnpj)
        {
            if(Validador.ValidarCnpj(cnpj))
            {
                var cadastro = await _context.Cadastro
                                .Include(c => c.CdFornEmpresaNavigation)
                                    .Where(c => c.CdEmpresaNavigation.Cnpj == cnpj).ToListAsync();
                if (cadastro == null)
                {
                    return NotFound();
                }

                if (cadastro.Count() < 1)
                    return NotFound("Nenhuma empresa fornece algo ao CNPJ" + cnpj);

                return Ok(cadastro);
            }
            return NotFound("O CNPJ" + cnpj +"é invalido");
        }


        // PUT: api/Cadastro/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCadastro(int id, Cadastro cadastro)
        {
            if (id != cadastro.Id)
            {
                return BadRequest();
            }

            _context.Entry(cadastro).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CadastroExists(id))
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
        public async Task<ActionResult<Cadastro>> PostCadastro(Cadastro cadastro)
        {
            Validador validar = new Validador(_context);

                if (validar.Cadastro(cadastro))
            {

                _context.Cadastro.Add(cadastro);
                await _context.SaveChangesAsync();
                return Ok(cadastro);
            }

            return NoContent();           
        }

        // DELETE: api/Cadastro/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCadastro(int id)
        {
            var cadastro = await _context.Cadastro.FindAsync(id);
            if (cadastro == null)
            {
                return NotFound();
            }

            _context.Cadastro.Remove(cadastro);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CadastroExists(int id)
        {
            return _context.Cadastro.Any(e => e.Id == id);
        }
    }
}
