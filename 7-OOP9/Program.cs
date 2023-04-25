namespace OOP9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string CommandGoSupermarket = "1";
            const string CommandExit = "2";

            Supermarket supermarket = new Supermarket();

            Console.WriteLine($"{CommandGoSupermarket} - ЗАЙТИ В СУПЕРМАРКЕТ" + $"\n{CommandExit} - ВЫХОД");

            bool isWorking = true;

            while (isWorking)
            {
                Console.Write("\nВведите команду: ");
                string userInput = Console.ReadLine();

                if (CommandGoSupermarket == userInput)
                {
                    supermarket.Work();
                }
                else if (CommandExit == userInput)
                {
                    isWorking = false;
                }
                else
                {
                    Console.WriteLine($"Ошибка. Введите {CommandGoSupermarket} или {CommandExit}");
                }
            }
        }
    }

    class Supermarket
    {
        private List<Product> _products = new List<Product>();
        private Queue<Client> _clients = new Queue<Client>();
        private int _moneyShop = 0;

        public Supermarket()
        {
            AddProducts();
        }

        public void Work()
        {
            CreateQueueCustomers();

            while (_clients.TryDequeue(out Client client))
            {
                Console.Clear();
                Console.WriteLine($"Прибыль магазина {_moneyShop} рублей");
                Console.WriteLine($"Около супермаркета гуляют {_clients.Count} человек(а).");
                Console.WriteLine($"Зашёл покупатель: {client.Name}");

                ChooseProducts(client);

                client.Buy();

                Console.WriteLine($"Денег осталось - {client.Money} рублей.");
                Console.WriteLine($"{client.Name} забирает с собой: ");

                client.ShowProductsBasket();

                _moneyShop += client.CalculateTotalPrice();

                Console.ReadKey(); 
            }
        }

        private void ChooseProducts(Client client)
        {
            bool isEnoughProducts = false;
            string exitWord = "касса";

            while (isEnoughProducts == false)
            {
                ShowProductsStore();

                Console.WriteLine($"\nДеньги клиента - {client.Money} рублей.");

                Console.Write($"Введите название продукта или {exitWord} для выхода: ");
                string userInput = Console.ReadLine();

                if (userInput == exitWord)
                {
                    isEnoughProducts = true;
                }
                else
                {
                    if (TryGetProduct(out Product product, userInput))
                    {
                        client.AddProductBasket(product);

                        Console.WriteLine("\nВы положили в тележку - " + product.Name);
                        Console.WriteLine("\nВ тележке у клиента лежит: ");

                        client.ShowProductsBasket();

                        Console.Write("\nДля того чтобы продолжить покупки нажмите любую клавишу...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Ошибка. Такого продукта нет в списке.");
                    }
                }

                Console.ReadKey();
            }
        }

        private bool TryGetProduct(out Product product, string userInput)
        {
            product = null;

            for (int i = 0; i < _products.Count; i++)
            {
                if (userInput.ToLower() == _products[i].Name.ToLower())
                {
                    product = _products[i];
                    return true;
                }
            }

            return false;
        }

        private void ShowProductsStore()
        {
            Console.WriteLine("\nПродукты супермаркета: ");

            for (int i = 0; i < _products.Count; i++)
            {
                Console.WriteLine("Название - " + _products[i].Name + ", Цена - " + _products[i].Price);
            }
        }

        private void AddProducts()
        {
            _products.Add(new Product("Хлеб", 50));
            _products.Add(new Product("Колбаса", 120));
            _products.Add(new Product("Молоко", 70));
            _products.Add(new Product("Творог", 90));
            _products.Add(new Product("Шоколад", 55));
            _products.Add(new Product("Чай", 35));
            _products.Add(new Product("Вода", 20));
        }

        private void CreateQueueCustomers()
        {
            _clients.Enqueue(new Client("Вася", 100));
            _clients.Enqueue(new Client("Ваня", 233));
            _clients.Enqueue(new Client("Алёна", 341));
            _clients.Enqueue(new Client("Катя", 555));
            _clients.Enqueue(new Client("Женя", 1000));
        }
    }

    class Client
    {
        private List<Product> _productsBasket = new List<Product>();

        public Client(string customer, int cash)
        {
            Name = customer;
            Money = cash;
        }

        public string Name { get; private set; }

        public int Money { get; private set; }


        public int CalculateTotalPrice()
        {
            int bill = 0;

            for (int i = 0; i < _productsBasket.Count; i++)
            {
                bill += _productsBasket[i].Price;
            }

            return bill;
        }

        public void Buy()
        {
            while (CanPay() == false)
            {
                RemoveProduct();
            }

            Money -= CalculateTotalPrice();
        }

        public void AddProductBasket(Product product)
        {
            _productsBasket.Add(product);
        }

        public void ShowProductsBasket()
        {
            for (int i = 0; i < _productsBasket.Count; i++)
            {
                Console.WriteLine("Название - " + _productsBasket[i].Name + ", Цена - " + _productsBasket[i].Price);
            }
        }
        private bool CanPay()
        {
            int bill = CalculateTotalPrice();

            return Money >= bill;
        }

        private void RemoveProduct()
        {
            int index = UserUtils.GenerateRandomNumber(_productsBasket.Count);

            Console.WriteLine($"{_productsBasket[index].Name} удалён.");
            _productsBasket.RemoveAt(index);
        }        
    }

    class Product
    {
        public Product(string commodityName, int commodityPrice)
        {
            Name = commodityName;
            Price = commodityPrice;
        }

        public string Name { get; private set; }

        public int Price { get; private set; }
    }

    class UserUtils
    {
        private static Random _random = new Random();

        public static int GenerateRandomNumber(int maх)
        {
            int min = 0;

            return _random.Next(min, maх);
        }
    }
}
