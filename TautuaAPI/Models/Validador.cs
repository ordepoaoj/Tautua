using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BLuDataAPI.Models
{
    public class Validador
    {
        private readonly Contexto _contexto ;

        public Validador (Contexto contexto)
        {
            _contexto = contexto;
        }

        public bool Cadastro (Cadastro cadastro)
        {
            int cdParana = 15;
            DbFunctions db = null;
            DateTime hoje = DateTime.Today;
            if (cadastro.CdFornEmpresa != null)
                return true;
            if (cadastro.CdFornPessoa != null)
            {
                var empresa = _contexto.Empresa.Where(e => e.Id == cadastro.CdEmpresa).Include(e => e.CdUfNavigation).FirstOrDefault();
                 var menor = _contexto.Pessoa.Where(p => db.DateDiffYear(p.DtNascimento, hoje) <= 18 && p.Id == cadastro.Id);
                if (empresa.CdUfNavigation.Id == cdParana && menor != null)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

		public static bool ValidarCpf(string cpf)
		{
			int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			string tempCpf;
			string digito;
			int soma;
			int resto;
			cpf = cpf.Trim();
			cpf = cpf.Replace(".", "").Replace("-", "");
			if (cpf.Length != 11)
				return false;
			tempCpf = cpf.Substring(0, 9);
			soma = 0;

			for (int i = 0; i < 9; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
			resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;
			digito = resto.ToString();
			tempCpf = tempCpf + digito;
			soma = 0;
			for (int i = 0; i < 10; i++)
				soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
			resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;
			digito = digito + resto;
			return cpf.EndsWith(digito);
		}

		public static bool ValidarCnpj (string cnpj)
        {
			int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
			int soma;
			int resto;
			string digito;
			string tempCnpj;
			cnpj = cnpj.Trim();
			cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
			if (cnpj.Length != 14)
				return false;
			tempCnpj = cnpj.Substring(0, 12);
			soma = 0;
			for (int i = 0; i < 12; i++)
				soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
			resto = (soma % 11);
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;
			digito = resto.ToString();
			tempCnpj = tempCnpj + digito;
			soma = 0;
			for (int i = 0; i < 13; i++)
				soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
			resto = (soma % 11);
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;
			digito = digito + resto;
			return cnpj.EndsWith(digito);
		}
	}
}
