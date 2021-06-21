using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable

namespace BLuDataAPI.Models
{
    public partial class Uf
    {
        public Uf()
        {
            Empresas = new HashSet<Empresa>();
            Pessoas = new HashSet<Pessoa>();
        }

        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }

        [JsonIgnore]
        public virtual ICollection<Empresa> Empresas { get; set; }
        [JsonIgnore]
        public virtual ICollection<Pessoa> Pessoas { get; set; }
    }
}
