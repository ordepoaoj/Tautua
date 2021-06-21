using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLuDataAPI.Models;

namespace BLuDataAPI.Controllers
{
    [Route("api/empresa")]
    [ApiController]
    public class EmpresasController : ControllerBase
    {
        private readonly Contexto _context;

        public EmpresasController(Contexto context)
        {
            _context = context;
        }


        // GET: api/Empresas/5
        [HttpGet("{cnpj}")]
        public async  Task<ActionResult<Empresa>> GetEmpresa(string cnpj)
        {
            if (cnpj.Length ==14)
            {
                var empresa = await _context.Empresa.Include(e => e.CdUfNavigation).Include(e => e.Telefones).Where(e => e.Cnpj == cnpj).FirstOrDefaultAsync();
                if (empresa == null)
                {
                    return NotFound();
                }
                return empresa;
            }
            return Ok("Dado no formato invalido");
        }

              
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpresa(int id, Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return BadRequest();
            }

            _context.Entry(empresa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Ok("Erro na base");
            }

            return NoContent();
        }

        // POST: api/Empresas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Empresa>> PostEmpresa(Empresa empresa)
        {
            Empresa cadastro = new Empresa();
            if (cadastro.validar(empresa.Nome.ToString(), empresa.Cnpj.ToString(), empresa.CdUf.ToString()) == true)
            {
                if(!EmpresaExiste(empresa.Cnpj.ToString()))
                {
                    _context.Empresa.Add(empresa);
                    await _context.SaveChangesAsync();
                    return Ok(empresa);
                }
                return Ok("Empresa já cadastrada no cnpj " + empresa.Cnpj.ToString());
            }

            return NotFound("Dados incorretos");
            
        }

        // DELETE: api/Empresas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            var empresa = await _context.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }

            _context.Empresa.Remove(empresa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpresaExiste(string cnpj)
        {
            return _context.Empresa.Any(e => e.Cnpj == cnpj);
        }
    }
}
