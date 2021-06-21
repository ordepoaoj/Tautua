using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BLuDataAPI.Models
{
    public partial class Pessoa
    {
        public Pessoa()
        {
            Cadastros = new HashSet<Cadastro>();
            Telefones = new HashSet<Telefone>();
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public int CdUf { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd/MM/yyyy")]
        public DateTime DtNascimento { get; set; }

        [JsonProperty("Estado")]
        public virtual Uf CdUfNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<Cadastro> Cadastros { get; set; }      
        public virtual ICollection<Telefone> Telefones { get; set; }
    }
}
