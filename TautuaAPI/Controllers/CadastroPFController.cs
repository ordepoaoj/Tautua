using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLuDataAPI.Models;

namespace BLuDataAPI.Controllers
{
    [Route("api/fornecedor/pf")]
    [ApiController]
    public class CadastroPFController : ControllerBase
    {
        private readonly Contexto _context;

        public CadastroPFController(Contexto context)
        {
            _context = context;
        }

        [HttpGet("{cpf}")]
        public async Task<ActionResult<Cadastro>> GetCadastro(string cpf)
        {
            if(Validador.ValidarCpf(cpf))
            {
                var cadastro = await _context.Cadastro
                .Include(c => c.CdEmpresaNavigation)
                    .Include(c => c.CdEmpresaNavigation.CdUfNavigation)
                        .Include(c => c.CdFornPessoaNavigation)
                            .OrderBy(c => c.Data)
                                .Where(c => c.CdFornPessoaNavigation.Cpf == cpf).ToListAsync();

                if (cadastro == null)
                {
                    return NotFound("O ­cpf " + cpf + " não foi localizado.");
                }

                return Ok(cadastro);
            }

            return NotFound("O ­cpf " + cpf + " é invalido");
        }
    }
}
