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
                    supermarket.Trade(supermarket);
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
        private List<Product> _listProducts = new List<Product>();
        private Queue<Client> _customerQueue = new Queue<Client>();

        public Supermarket()
        {
            AddProducts();

            AddQueueCustomers();
        }

        public void Trade(Supermarket supermarket)
        {
            Client client = _customerQueue.Peek();

            string goCheckOut = "касса";

            while (_customerQueue.Count > 0)
            {
                ShowProductsMagazine();

                Console.WriteLine("\nЧтобы подойти к кассе введите - " + goCheckOut);
                Console.WriteLine("\nВ супермаркет зашёл(а) - " + _customerQueue.Peek().Name);
                Console.WriteLine("У него в кармане - " + _customerQueue.Peek().Money + " рублей.");

                Console.Write("\nВведите название продуктов: ");
                string userInput = Console.ReadLine();

                if (TryGetProduct(out Product product, userInput))
                {
                    client.AddProductBasket(product);

                    Console.Clear();
                }
                else if (userInput.ToLower() == goCheckOut.ToLower())
                {
                    Console.Clear();
                    Console.WriteLine("К кассе подошёл - " + _customerQueue.Peek().Name);
                    Console.WriteLine("У него в кармане - " + _customerQueue.Peek().Money + " рублей.");

                    client.ShowProductsBasket();

                    client.TryPaidProduct(supermarket);
                }
                else
                {
                    Console.WriteLine("\nОшибка. Попробуйте ещё раз.");
                    Console.Write("Для продолжения нажмите любую клавишу...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        public Client ShowFirstItemBeginningQueue()
        {
            var customer = _customerQueue.Peek();
            return customer;
        }

        public string DequeueFirstItemList()
        {
            string customer = _customerQueue.Dequeue().Name;
            return customer;
        }

        private bool TryGetProduct(out Product product, string userInput)
        {
            product = null;

            for (int i = 0; i < _listProducts.Count; i++)
            {
                if (userInput.ToLower() == _listProducts[i].СommodityName.ToLower())
                {
                    product = _listProducts[i];
                    return true;
                }
            }

            return false;
        }

        private void ShowProductsMagazine()
        {
            Console.WriteLine("\nПродукты супермаркета: ");

            for (int i = 0; i < _listProducts.Count; i++)
            {
                Console.WriteLine("Название - " + _listProducts[i].СommodityName + ", Цена - " + _listProducts[i].СommodityPrice);
            }
        }

        private void AddProducts()
        {
            _listProducts.Add(new Product("Хлеб", 50));
            _listProducts.Add(new Product("Колбаса", 120));
            _listProducts.Add(new Product("Молоко", 70));
            _listProducts.Add(new Product("Творог", 90));
            _listProducts.Add(new Product("Шоколад", 55));
            _listProducts.Add(new Product("Чай", 35));
            _listProducts.Add(new Product("Вода", 20));
        }

        private void AddQueueCustomers()
        {
            _customerQueue.Enqueue(new Client("Вася", 100));
            _customerQueue.Enqueue(new Client("Ваня", 233));
            _customerQueue.Enqueue(new Client("Алёна", 341));
            _customerQueue.Enqueue(new Client("Катя", 555));
            _customerQueue.Enqueue(new Client("Женя", 1000));
        }
    }

    class Client
    {
        private List<Product> _productsBasket = new List<Product>();
        private int _moneyMustPaid = 0;
        private int _moneyPay;

        public Client(string customer, int cash)
        {
            Name = customer;
            Money = cash;
        }

        public string Name { get; private set; }

        public int Money { get; private set; }

        public bool TryPaidProduct(Supermarket supermarket)
        {
            NecessaryPayCustomer();

            _moneyPay = supermarket.ShowFirstItemBeginningQueue().Money;

            if (_moneyPay >= _moneyMustPaid)
            {
                Buy(supermarket);

                return true;
            }
            else
            {
                PayProducts();

                Buy(supermarket);

                return false;
            }
        }

        public void AddProductBasket(Product product)
        {
            _productsBasket.Add(product);

            Console.WriteLine("\nВы положили в тележку - " + product.СommodityName);
            Console.Write("\nДля того чтобы продолжить покупки нажмите любую клавишу...");
            Console.ReadKey();
            Console.Clear();
        }

        public void ShowProductsBasket()
        {
            Console.WriteLine("\nПродукты в тележке: ");

            for (int i = 0; i < _productsBasket.Count; i++)
            {
                Console.WriteLine("Название - " + _productsBasket[i].СommodityName + ", Цена - " + _productsBasket[i].СommodityPrice);
            }
        }

        private void Buy(Supermarket supermarket)
        {
            var customer = supermarket.ShowFirstItemBeginningQueue().Name;

            if (_moneyMustPaid <= 0)
            {
                Console.WriteLine("Ошибка. Вы ничего не взяли. Приходите в следующий раз...");
            }
            else
            {
                Console.WriteLine("\n\nПокупатель " + customer + " заплатил " + _moneyMustPaid + " рублей.");
                _moneyPay -= _moneyMustPaid;

                Console.WriteLine("\n" + customer + " забирает с собой: ");

                for (int i = 0; i < _productsBasket.Count; i++)
                {
                    Console.WriteLine("Товар - " + _productsBasket[i].СommodityName);
                }

                Console.WriteLine("\nУ покупателя - " + customer + " осталось " + _moneyPay + " рублей.");
            }

            _moneyMustPaid = 0;
            _productsBasket.Clear();

            customer = supermarket.DequeueFirstItemList();

            Console.Write("\nЧтобы зашёл следующий покупатель нажмите любую клавишу...");
            Console.ReadKey();
            Console.Clear();
        }

        private void PayProducts()
        {
            ShuffleListProducts();

            while (_moneyPay < _moneyMustPaid)
            {
                int index = 0;

                if (index >= 0 && index < _productsBasket.Count)
                {
                    _moneyMustPaid -= _productsBasket[index].СommodityPrice;

                    Console.WriteLine("\nВам не хватает денег для оплаты, давайте что нибудь выложим.");
                    Console.WriteLine("Товар - " + _productsBasket[index].СommodityName + " удалён из корзины.");

                    _productsBasket.RemoveAt(index);
                }

                Console.ReadKey();
            }
        }

        private void ShuffleListProducts()
        {
            Random random = new();

            for (int product = _productsBasket.Count - 1; product >= 1; product--)
            {
                int tempListIndex = random.Next(product + 1);
                (_productsBasket[product], _productsBasket[tempListIndex]) = (_productsBasket[tempListIndex], _productsBasket[product]);
            }
        }

        private void NecessaryPayCustomer()
        {
            for (int i = 0; i < _productsBasket.Count; i++)
            {
                _moneyMustPaid += _productsBasket[i].СommodityPrice;
            }

            Console.WriteLine("\nЗдравствуйте. С вас - " + _moneyMustPaid + " рублей.");
            Console.Write("\nДля оплаты нажмите любую клавишу...");
            Console.ReadKey();
        }
    }

    class Product
    {
        public Product(string productName, int productPrice)
        {
            СommodityName = productName;
            СommodityPrice = productPrice;
        }

        public string СommodityName { get; private set; }

        public int СommodityPrice { get; private set; }
    }
}
