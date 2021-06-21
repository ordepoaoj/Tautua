using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLuDataAPI.Models;

namespace BLuDataAPI.Controllers
{
    [Route("api/fornecedor/pj")]
    [ApiController]
    public class CadastroPjController : ControllerBase
    {
        private readonly Contexto _context;

        public CadastroPjController(Contexto context)
        {
            _context = context;
        }

        [HttpGet("{cnpj}")]
        public async Task<ActionResult<Cadastro>> GetCadastro(string cnpj)
        {
            if (Validador.ValidarCnpj(cnpj))
            {
                var cadastro = await _context.Cadastro
                .Include(c => c.CdEmpresaNavigation)
                    .Include(c => c.CdEmpresaNavigation.CdUfNavigation)
                        .Include(c => c.CdFornPessoaNavigation)
                            .OrderBy(c => c.Data)
                                .Where(c => c.CdFornEmpresaNavigation.Cnpj == cnpj).ToListAsync();

                if (cadastro == null)
                {
                    return NotFound("O CNPJ " + cnpj + " não foi localizado.");
                }
                if (cadastro.Count() < 1)
                {
                    return NotFound("O CNPJ " + cnpj + " não fornece nada a nenhuma empresa cadastrada.");
                }

                return Ok(cadastro);
            }

            return NotFound("O CNPJ " + cnpj + " é invalido");
        }
    }
}
