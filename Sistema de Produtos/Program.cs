using Sistema_de_Produtos.Service;
using Sistema_de_Produtos.Models;

class Program
{
    static UsuarioService service = new UsuarioService();
    static void Main()
    {
        service.Carregar();
        Program.Menu();
    }

    public static void Menu()
    {
        int opcao;
        do
        {
            
        
            Console.WriteLine("=====MENU=====");
            Console.WriteLine("1 - Cadastro");
            Console.WriteLine("2 - Consultar");
            Console.WriteLine("3 - Lista");
            Console.WriteLine("4 - Sair");
            Console.Write("Escolher: ");
            if(!int.TryParse(Console.ReadLine(), out opcao))
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
                continue;
            }

            switch (opcao)
            {
                case 1:
                    Cadastro();
                    break;
                case 2:
                    Consultar();
                    break;
                case 3:
                    Lista();
                    break;
                case 4: Console.WriteLine("Saindo..."); break;
            }
        }
        while (opcao != 4);
    }

    public static void Cadastro()
    {
        Console.Write("ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID inválido. Cadastro cancelado.");
            return;
        }

        Console.Write("Nome: ");
        string nome = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("Nome inválido. Cadastro cancelado.");
            return;
        }

        Console.Write("Preço: ");
        if(!decimal.TryParse(Console.ReadLine(), out decimal preco))
        {
            Console.WriteLine("Preço inválido. Cadastro cancelado.");
            return;
        }
        Produtos produtos = new Produtos
        {
            Id = id,
            Nome = nome,
            Preco = preco
        };
        service.Adicionar(produtos);

    }

    public static void Lista()
    {
        var lista = service.Listar();
        if (lista.Count == 0)
        {
            Console.WriteLine("Lista vazia");
        }
        foreach (var item in lista)
        {
            Console.WriteLine($"ID: {item.Key} | {item.Value.Nome} | {item.Value.Preco}");
        }
    }

    public static void Consultar()
    {
        Console.Write("ID: ");
        if(!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("ID não encontrado;");
            return;
        }
        
        var estoque = service.Consultar(id);
        if (estoque == null)
        {
            Console.WriteLine("Produto  encontrado no estoque");
            return;
        }
        Console.WriteLine($"ID: {estoque.Id} | Nome: {estoque.Nome} | Preço: {estoque.Preco}");
        
        


    }
}
