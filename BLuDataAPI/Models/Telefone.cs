using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable

namespace BLuDataAPI.Models
{
    public partial class Telefone
    {
        public int Id { get; set; }
        public string Numero { get; set; }
        public int? CdPessoa { get; set; }
        public int? CdEmpresa { get; set; }


        [JsonIgnore]
        public virtual Empresa CdEmpresaNavigation { get; set; }
        [JsonIgnore]
        public virtual Pessoa CdPessoaNavigation { get; set; }
    }
}
