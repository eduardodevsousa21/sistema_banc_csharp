using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    static string Menu()
    {
        string menuTexto = @"
        ================ MENU ================
        [d]\tDepósito
        [s]\tSaque
        [e]\tExtrato
        [nc]\tNova conta
        [lc]\tListar contas
        [nu]\tNovo usuário
        [q]\tSair
        => ";
        Console.Write(menuTexto);
        return Console.ReadLine();
    }

    static (decimal, string) Depositar(decimal saldo, decimal valor, string extrato)
    {
        if (valor > 0)
        {
            saldo += valor;
            extrato += $"Depósito:\t\tR$ {valor:F2}\n";
            Console.WriteLine("\n=== Depósito realizado com sucesso! ===");
        }
        else
        {
            Console.WriteLine("\n@@@ Operação falhou! O valor informado é inválido. @@@");
        }

        return (saldo, extrato);
    }

    static (decimal, string) Sacar(decimal saldo, decimal valor, string extrato, decimal limite, int numeroSaques, int limiteSaques)
    {
        bool excedeuSaldo = valor > saldo;
        bool excedeuLimite = valor > limite;
        bool excedeuSaques = numeroSaques >= limiteSaques;

        if (excedeuSaldo)
        {
            Console.WriteLine("\n@@@ Operação falhou! Você não tem saldo suficiente. @@@");
        }
        else if (excedeuLimite)
        {
            Console.WriteLine("\n@@@ Operação falhou! O valor do saque excede o limite. @@@");
        }
        else if (excedeuSaques)
        {
            Console.WriteLine("\n@@@ Operação falhou! Número máximo de saques excedido. @@@");
        }
        else if (valor > 0)
        {
            saldo -= valor;
            extrato += $"Saque:\t\tR$ {valor:F2}\n";
            numeroSaques++;
            Console.WriteLine("\n=== Saque realizado com sucesso! ===");
        }
        else
        {
            Console.WriteLine("\n@@@ Operação falhou! O valor informado é inválido. @@@");
        }

        return (saldo, extrato);
    }

    static void ExibirExtrato(decimal saldo, string extrato)
    {
        Console.WriteLine("\n================ EXTRATO ================");
        Console.WriteLine(!string.IsNullOrEmpty(extrato) ? extrato : "Não foram realizadas movimentações.");
        Console.WriteLine($"\nSaldo:\t\tR$ {saldo:F2}");
        Console.WriteLine("=========================================");
    }

    static void CriarUsuario(List<Dictionary<string, string>> usuarios)
    {
        Console.Write("Informe o CPF (somente números): ");
        string cpf = Console.ReadLine();
        Dictionary<string, string> usuario = FiltrarUsuario(cpf, usuarios);

        if (usuario != null)
        {
            Console.WriteLine("\n@@@ Já existe um usuário com esse CPF! @@@");
            return;
        }

        Console.Write("Informe o nome completo: ");
        string nome = Console.ReadLine();
        Console.Write("Informe a data de nascimento (dd-mm-aaaa): ");
        string dataNascimento = Console.ReadLine();
        Console.Write("Informe o endereço (logradouro, número - bairro - cidade/estado): ");
        string endereco = Console.ReadLine();

        usuarios.Add(new Dictionary<string, string> { { "nome", nome }, { "data_nascimento", dataNascimento }, { "cpf", cpf }, { "endereco", endereco } });

        Console.WriteLine("=== Usuário criado com sucesso! ===");
    }

    static Dictionary<string, string> FiltrarUsuario(string cpf, List<Dictionary<string, string>> usuarios)
    {
        foreach (var usuario in usuarios)
        {
            if (usuario["cpf"] == cpf)
            {
                return usuario;
            }
        }
        return null;
    }

    static Dictionary<string, string> CriarConta(string agencia, int numeroConta, List<Dictionary<string, string>> usuarios)
    {
        Console.Write("Informe o CPF do usuário: ");
        string cpf = Console.ReadLine();
        Dictionary<string, string> usuario = FiltrarUsuario(cpf, usuarios);

        if (usuario != null)
        {
            Console.WriteLine("\n=== Conta criada com sucesso! ===");
            return new Dictionary<string, string> { { "agencia", agencia }, { "numero_conta", numeroConta.ToString() }, { "usuario", usuario.ToString() } };
        }

        Console.WriteLine("\n@@@ Usuário não encontrado, processo de criação de conta encerrado! @@@");
        return null;
    }

    static void ListarContas(List<Dictionary<string, string>> contas)
    {
        foreach (var conta in contas)
        {
            StringBuilder linha = new StringBuilder();
            linha.AppendLine("Agência:\t" + conta["agencia"]);
            linha.AppendLine("C/C:\t" + conta["numero_conta"]);
            linha.AppendLine("Titular:\t" + conta["usuario"]);
            Console.WriteLine("==================================================================");
            Console.WriteLine(linha.ToString());
        }
    }

    static void Main(string[] args)
    {
        const int LimiteSaques = 3;
        const string Agencia = "0001";

        decimal saldo = 0;
        decimal limite = 500;
        string extrato = "";
        int numeroSaques = 0;
        List<Dictionary<string, string>> usuarios = new List<Dictionary<string, string>>();
        List<Dictionary<string, string>> contas = new List<Dictionary<string, string>>();

        while (true)
        {
            string opcao = Menu();

            if (opcao == "d")
            {
                Console.Write("Informe o valor do depósito: ");
                decimal valor = Convert.ToDecimal(Console.ReadLine());

                (saldo, extrato) = Depositar(saldo, valor, extrato);
            }
            else if (opcao == "s")
            {
                Console.Write("Informe o valor do saque: ");
                decimal valor = Convert.ToDecimal(Console.ReadLine());

                (saldo, extrato) = Sacar(saldo, valor, extrato, limite, numeroSaques, LimiteSaques);
            }
            else if (opcao == "e")
            {
                ExibirExtrato(saldo, extrato);
            }
            else if (opcao == "nu")
            {
                CriarUsuario(usuarios);
            }
            else if (opcao == "nc")
            {
                int numeroConta = contas.Count + 1;
                Dictionary<string, string> conta = CriarConta(Agencia, numeroConta, usuarios);

                if (conta != null)
                {
                    contas.Add(conta);
                }
            }
            else if (opcao == "lc")
            {
                ListarContas(contas);
            }
            else if (opcao == "q")
            {
                break;
            }
            else
            {
                Console.WriteLine("Operação inválida, por favor selecione uma opção valida");
            }
        }
    }
}