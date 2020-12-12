using src.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace src
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Resolução do exercício proposto de combinações da seção 7");

            List<Order> listaPedidos = new List<Order>();
            Client cliente = new Client();
            List<OrderItem> items = new List<OrderItem>();
            string status = "PendingPayment";

            int caseSwitch;
            bool loop = true;
            do
            {
                Console.WriteLine(Environment.NewLine + "Menu de opções");
                Console.WriteLine("1) Cadastro de pedido");
                Console.WriteLine("2) Listar pedidos cadastrados");
                Console.WriteLine("3) Sair");
                Console.Write("Digite o número da opção desejada: ");
                caseSwitch = int.Parse(Console.ReadLine());
                switch (caseSwitch)
                {
                    case 1:
                        Console.WriteLine(Environment.NewLine + "Cadastro de pedido");

                        Console.WriteLine("Entre com os dados do cliente");

                        Console.Write("Nome: ");
                        string name = Console.ReadLine().Trim();
                        bool loopClient = true;
                        do
                        {
                            if (String.IsNullOrEmpty(name) || name.Length < 5 || String.IsNullOrWhiteSpace(name))
                            {
                                Console.Write(Environment.NewLine
                                    + "O nome do cliente deve ter um formato válido e mais de 5 letras. "
                                    + Environment.NewLine
                                    + "Digite o nome do cliente: ");
                                name = Console.ReadLine();
                            }
                            else
                            {
                                loopClient = false;
                            }
                        } while (loopClient);

                        Console.Write("Email: ");
                        string email = Console.ReadLine().Trim();
                        loopClient = true;
                        do
                        {
                            if (String.IsNullOrEmpty(email) || !email.Contains('@') || email.Contains(' ') || !email.Contains('.') || String.IsNullOrWhiteSpace(email))
                            {
                                Console.Write(Environment.NewLine
                                    + "O email do cliente deve ter um formato válido."
                                    + Environment.NewLine
                                    + "Digite o email do cliente: ");
                                email = Console.ReadLine();
                            }
                            else
                            {
                                loopClient = false;
                            }
                        } while (loopClient);

                        Console.Write("Data de nascimento dd/mm/aaaa: ");
                        string birthDate = Console.ReadLine().Trim();
                        loopClient = true;
                        do
                        {
                            if (String.IsNullOrEmpty(birthDate) || birthDate.Length != 10 || !birthDate.Contains('/') || birthDate.Contains('.') || birthDate.Contains('-') || String.IsNullOrWhiteSpace(birthDate))
                            {
                                Console.Write(Environment.NewLine
                                    + "A data de nascimento deve ter um formato válido dd/mm/aaaa."
                                    + Environment.NewLine
                                    + "Digite a data de nascimento do cliente: ");
                                birthDate = Console.ReadLine();
                            }
                            else if (int.Parse(birthDate.Substring(6, 4)) > DateTime.Now.Year)
                            {
                                Console.Write(Environment.NewLine
                                    + $"O ano deve ser anterior a {DateTime.Now.Year}."
                                    + Environment.NewLine
                                    + "Digite a data de nascimento do cliente: ");
                                birthDate = Console.ReadLine();
                            }
                            else
                            {
                                loopClient = false;
                            }
                        } while (loopClient);

                        Console.WriteLine(Environment.NewLine + "Cliente: "
                            + name
                            + " ("
                            + birthDate
                            + ") - "
                            + email);

                        //Instancia do objeto cliente
                        cliente = new Client(name, email, DateTime.Parse(birthDate));

                        Console.WriteLine(Environment.NewLine + "Dados do pedido");

                        loopClient = true;
                        do
                        {
                            Console.WriteLine(Environment.NewLine + "Escolha a situação do pedido");
                            Console.WriteLine("1) Pagamento pendente");
                            Console.WriteLine("2) Processando");
                            Console.WriteLine("3) Enviado");
                            Console.WriteLine("4) Entregue");
                            Console.Write("Digite o número da opção desejada: ");
                            int switchStatus = int.Parse(Console.ReadLine());
                            switch (switchStatus)
                            {
                                case 1:
                                    status = "PendingPayment";
                                    Console.WriteLine("Situação: Pagamento pendente");
                                    loopClient = false;
                                    break;

                                case 2:
                                    status = "Processing";
                                    Console.WriteLine("Situação: Pedido em processamento");
                                    loopClient = false;
                                    break;

                                case 3:
                                    status = "Shipped";
                                    Console.WriteLine("Situação: Pedido enviado");
                                    loopClient = false;
                                    break;

                                case 4:
                                    status = "Delivered";
                                    Console.WriteLine("Situação: Entregue.");
                                    loopClient = false;
                                    break;

                                default:
                                    Console.WriteLine("Opção inválida" + Environment.NewLine);
                                    break;
                            }
                        } while (loopClient);

                        bool itemLoop = true;

                        string itemName;
                        int itemQtd;
                        double itemPrice;

                        do
                        {
                            Console.WriteLine(Environment.NewLine + $"Itens cadastrados: {items.Count}");
                            double sum = 0.0;
                            foreach (OrderItem obj in items)
                            {
                                Console.WriteLine(obj.ToString());
                                sum += obj.SubTotal();
                            }
                            Console.WriteLine($"Total do pedido: $ {sum.ToString("F2", CultureInfo.InvariantCulture)}");

                            Console.Write("Deseja cadastrar um novo item (s/n)? ");
                            string addItem = Console.ReadLine();
                            if (addItem[0] == 's' || addItem[0] == 'S' || addItem[0] == 'y' || addItem[0] == 'Y')
                            {
                                Console.WriteLine(Environment.NewLine + $"Entre com os dados do item #{items.Count + 1}");
                                Console.Write("Nome do item: ");
                                itemName = Console.ReadLine();
                                Console.Write("Preço do item: $ ");
                                itemPrice = double.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);
                                Console.Write("Quantidade: ");
                                itemQtd = int.Parse(Console.ReadLine());

                                items.Add(new OrderItem(itemQtd, new Product(itemName, itemPrice)));
                            }
                            else if (addItem[0] == 'n' || addItem[0] == 'N')
                            {
                                if (items.Count > 0)
                                {
                                    Order pedido = new Order(DateTime.Now, Enum.Parse<OrderStatus>(status), cliente);
                                    
                                    foreach (OrderItem obj in items)
                                    {
                                        pedido.AddItem(obj);
                                    }

                                    Console.WriteLine();

                                    Console.WriteLine("Resumo do pedido: ");
                                    Console.WriteLine("Momento do pedido: " + pedido.Moment.ToString("dd/MM/yyyy"));
                                    Console.WriteLine("Situação do pedido: " + pedido.Status.ToString());
                                    Console.WriteLine("Cliente: " + pedido.Client.ToString());
                                    Console.WriteLine("Lista de itens: ");
                                    foreach (OrderItem obj in pedido.Items)
                                    {
                                        Console.WriteLine(obj.ToString());
                                    }
                                    listaPedidos.Add(pedido);

                                    itemLoop = false;


                                }
                                else
                                {
                                    Console.WriteLine("Pelo menos um item deve ser cadastrado no pedido");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Opção inválida");
                            }

                        } while (itemLoop);

                        break;

                    case 2:
                        if(listaPedidos.Count < 1)
                        {
                            Console.WriteLine(Environment.NewLine + "Nenhum pedido cadastrato");
                        }
                        else
                        {
                            Console.WriteLine(Environment.NewLine + "Pedidos cadastrados:");

                            foreach (Order obj in listaPedidos)
                            {
                                Console.WriteLine(Environment.NewLine + obj.ToString());
                            }
                        }

                        break;

                    case 3:
                        Console.WriteLine(Environment.NewLine + "Sair");
                        loop = false;
                        break;

                    default:
                        break;
                }
            } while (loop);
        }
    }
}