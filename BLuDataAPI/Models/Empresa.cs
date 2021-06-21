using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable

namespace BLuDataAPI.Models
{

    public partial class Empresa
    {
        public Empresa()
        {
            CadastroCdEmpresaNavigations = new HashSet<Cadastro>();
            CadastroCdFornEmpresaNavigations = new HashSet<Cadastro>();
            Telefones = new HashSet<Telefone>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public int CdUf { get; set; }

        [JsonProperty("Estado")]
        public virtual Uf CdUfNavigation { get; set; }

        [JsonIgnore]
        public virtual ICollection<Cadastro> CadastroCdEmpresaNavigations { get; set; }
        [JsonIgnore]
        public virtual ICollection<Cadastro> CadastroCdFornEmpresaNavigations { get; set; }

        public virtual ICollection<Telefone> Telefones { get; set; }

        public bool validar(string nome, string cnpj, string uf)
        {
            if (nome.Length > 3 && nome.Length < 70 && cnpj.Length == 14 && uf.Length == 2)
                return true;

            return false;
        }
    }
}
