using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Sistema_de_Produtos.Models;

namespace Sistema_de_Produtos.Service
{
    public class UsuarioService
    {
        private Dictionary<int, Produtos> estoque = new Dictionary<int, Produtos>();
        private readonly string caminho = "Data/produtos.json";
        private readonly string caminholog = "Data/log.txt";
        private readonly string pasta = "Data";

        public void Gerarpasta()
        {
            //Criar pasta
            if (!Directory.Exists(pasta))
            {
                Directory.CreateDirectory(pasta);
            }

            //Criar arquivo
            if (!File.Exists(caminho))
            {
                File.WriteAllText(caminho, JsonSerializer.Serialize(new Dictionary<int, Produtos>()));
            }

            //Criar arquivo de log
            if (!File.Exists(caminholog))
            {
                File.WriteAllText(caminholog, "");
                
            }
        }

        public void Carregar()
        {
            try
            {
                Gerarpasta();

                using (StreamReader arquivo = new StreamReader(caminho))
                {
                    string json = arquivo.ReadToEnd();
                    
                    estoque = JsonSerializer.Deserialize<Dictionary<int, Produtos>>(json) ?? new Dictionary<int, Produtos>();
                    
                }
                Escritolog("Estoque carregado com sucesso.");
            }
            catch (Exception ex)
            {
                throw;
                
            }
        }

        public void Salvar()
        {
            try
            {
                using (StreamWriter arquivo = new StreamWriter(caminho))
                {
                    string json = JsonSerializer.Serialize<Dictionary<int, Produtos>>(estoque);
                    arquivo.Write(json);
                }
            }
            catch (Exception ex)
            {
                Escritolog($"Erro ao salvar produto: {ex.Message}");
                throw;
            }
        }
        public void Adicionar(Produtos produtos)
        {
            try
            {
                if(produtos == null)
                {
                    throw new ArgumentNullException("O produto não pode ser nulo.");
                }
                if (produtos.Preco < 0 )
                {
                    throw new ArgumentException("Preço negativo inválido!");
                }
                if (estoque.ContainsKey(produtos.Id))
                {
                    throw new ArgumentException("ID já existente no estoque!");
                }
                Escritolog($"Produto adicionado: ID: {produtos.Id} | Nome: {produtos.Nome} | Preço: R${produtos.Preco}");
                Salvar();
            }
            catch (Exception ex)
            {
                Escritolog($"Erro ao adicionar produto: {ex.Message}");
                throw;

            }
        }
        public Dictionary<int, Produtos> Listar()
        {
            var listaProdutos = estoque.OrderBy(a => a.Key).ToDictionary();
            return listaProdutos;
        }

        public Produtos Consultar(int id)
        {
            estoque.TryGetValue(id, out Produtos? produto);
            if (produto == null)
            {
                
                return null;
            }
            else
            {
                return produto;
            }

        }

        public bool Remover(int id)
        {
            if (!estoque.TryGetValue(id, out Produtos resultado))
            {
               
                return false;
            }
            estoque.Remove(id);
            Escritolog($"Produto removido: ID: {resultado.Id} | Nome: {resultado.Nome} | Preço: R${resultado.Preco}");
            Salvar();
            return true;
        }

        public bool Atualizar(Produtos produtos)
        {
            if (!estoque.ContainsKey(produtos.Id))
            {
                return false;   
            }
            estoque[produtos.Id] = produtos;
            Escritolog($"Produto atualizado: ID: {produtos.Id} | Nome: {produtos.Nome} | Preço: R${produtos.Preco}");
            return true;
        }

        public void Escritolog(string mensagem)
        {
            try
            {
                //Abrir o arquivo de log para escrita
                using (StreamWriter Sw = new StreamWriter(caminholog, true))
                {
                    //Escrever a mensagem de log com data e hora
                    Sw.Write($"{DateTime.Now: dd/MM/yyyy - HH:mm:ss} - {mensagem}");
                }

            }catch (Exception ex)
            {
                throw;
            }
        }

    }                                           
}
