using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable

namespace BLuDataAPI.Models
{
    public partial class Cadastro
    {
        public int Id { get; set; }
        public int CdEmpresa { get; set; }
        public int? CdFornEmpresa { get; set; }
        public int? CdFornPessoa { get; set; }
        [JsonConverter(typeof(DateFormatConverter), "dd/MM/yyyy")]
        public DateTime Data { get; set; }

        [JsonProperty("FornecedorPF")]
        public virtual Pessoa CdFornPessoaNavigation { get; set; }
        [JsonProperty("Empresa")]
        public virtual Empresa CdEmpresaNavigation { get; set; }
        [JsonProperty("FornecedorPJ ")]
        public virtual Empresa CdFornEmpresaNavigation { get; set; }
        
    }
}
