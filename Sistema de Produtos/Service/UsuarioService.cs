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

        public void Carregar()
        {
            try
            {
                string pasta = Path.GetDirectoryName(caminho);
                if (!Directory.Exists(pasta))
                {
                    Directory.CreateDirectory(pasta);
                }

                if (!File.Exists(caminho))
                {
                    File.WriteAllText(caminho, "{}");
                    estoque = new Dictionary<int, Produtos>();
                    return;
                }

                using (StreamReader arquivo = new StreamReader(caminho))
                {
                    string json = arquivo.ReadToEnd();
                    
                    estoque = JsonSerializer.Deserialize<Dictionary<int, Produtos>>(json) ?? new Dictionary<int, Produtos>();
                }
            }
            catch (Exception ex)
            {
                estoque = new Dictionary<int, Produtos>();
                return;
              
                
            }
        }

        public void Salvar()
        {
            try
            {
                using (StreamWriter arquivo = new StreamWriter(caminho))
                {
                    string json = JsonSerializer.Serialize<Dictionary<int, Produtos>>(estoque);
                    arquivo.WriteLine(json);
                }
            }
            catch (Exception ex)
            {
                estoque = new Dictionary<int, Produtos>();
            }
        }
        public void Adicionar(Produtos produtos)
        {
            if (estoque.TryGetValue(produtos.Id, out Produtos Resultado))
            {
                throw new Exception("ID já cadastrado");    
            }
            
            
            estoque.Add(produtos.Id, produtos);
            Salvar();


        }
        public Dictionary<int, Produtos> Listar()
        {
            var listaProdutos = estoque.OrderBy(a => a.Key).ToDictionary();
            return listaProdutos;
        }

        public Produtos Consultar(int id)
        {
            var produtos = estoque.TryGetValue(id, out Produtos? produto);
            if (produto == null)
            {
                
                return null;
            }
            else
            {
                return produto;
            }

        }

    }                                           
}
