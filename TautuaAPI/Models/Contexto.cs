using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace BLuDataAPI.Models
{
    public partial class Contexto : DbContext
    {
        public Contexto()
        {
        }

        public Contexto(DbContextOptions<Contexto> options)
            : base(options)
        {
        }

        public virtual DbSet<Cadastro> Cadastro { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<Pessoa> Pessoa { get; set; }
        public virtual DbSet<Telefone> Telefone { get; set; }
        public virtual DbSet<Uf> Uf { get; set; }

        public IConfiguration Configuration { get;  }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnextion"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Cadastro>(entity =>
            {
                entity.ToTable("Cadastro");

                entity.Property(e => e.CdEmpresa).HasColumnName("cdEmpresa");

                entity.Property(e => e.CdFornEmpresa).HasColumnName("cdFornEmpresa");

                entity.Property(e => e.CdFornPessoa).HasColumnName("cdFornPessoa");

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.HasOne(d => d.CdEmpresaNavigation)
                .WithMany(p => p.CadastroCdEmpresaNavigations)
                .HasForeignKey(d => d.CdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cadastro_ToEmpresa");

                entity.HasOne(d => d.CdFornEmpresaNavigation)
                    .WithMany(p => p.CadastroCdFornEmpresaNavigations)
                    .HasForeignKey(d => d.CdFornEmpresa)
                    .HasConstraintName("FK_ToFornEmpresa");

                entity.HasOne(d => d.CdFornPessoaNavigation)
                    .WithMany(p => p.Cadastros)
                    .HasForeignKey(d => d.CdFornPessoa)
                    .HasConstraintName("FK_ToFornPessoa");
            });

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.ToTable("Empresa");

                entity.Property(e => e.CdUf).HasColumnName("cdUF");

                entity.Property(e => e.Cnpj)
                    .IsRequired()
                    .HasMaxLength(14)
                    .HasColumnName("CNPJ")
                    .IsFixedLength(true);

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.CdUfNavigation)
                    .WithMany(p => p.Empresas)
                    .HasForeignKey(d => d.CdUf)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Empresa_ToUF");
            });

            modelBuilder.Entity<Pessoa>(entity =>
            {
                entity.ToTable("Pessoa");

                entity.Property(e => e.CdUf).HasColumnName("cdUF");

                entity.Property(e => e.Cpf)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnName("CPF")
                    .IsFixedLength(true);

                entity.Property(e => e.DtNascimento).HasColumnType("date");

                entity.Property(e => e.Nome)
                    .IsRequired()
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.HasOne(d => d.CdUfNavigation)
                    .WithMany(p => p.Pessoas)
                    .HasForeignKey(d => d.CdUf)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pessoa_ToUF");
            });

            modelBuilder.Entity<Telefone>(entity =>
            {
                entity.ToTable("Telefone");

                entity.HasOne(d => d.CdEmpresaNavigation)
                    .WithMany(p => p.Telefones)
                    .HasForeignKey(d => d.CdEmpresa)
                    .HasConstraintName("FK_Telefone_ToEmpresa");

                entity.HasOne(d => d.CdPessoaNavigation)
                    .WithMany(p => p.Telefones)
                    .HasForeignKey(d => d.CdPessoa)
                    .HasConstraintName("FK_Telefone_ToPessoa");

                entity.Property(e => e.Numero)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Uf>(entity =>
            {
                entity.ToTable("UF");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Sigla)
                    .HasMaxLength(2)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
