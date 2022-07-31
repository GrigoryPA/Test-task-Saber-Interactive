using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace ListRandSerialization
{
    class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand; // произвольный элемент внутри списка
        public string Data;
    }

    class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count; // количество элементов списка

        private static Random rand = new Random();

        public void Serialize(FileStream s)
        {
            if (Head != null) // если нет головы ранд-списка, то функцию не выполняем
            {
                Dictionary<ListNode, int> dict = new Dictionary<ListNode, int>(Count); // создаем словарь нужного размера
                int i = 0; // инициализируем словарь
                for (ListNode temp = Head; !(temp is null); temp = temp.Next, i++)                                  // N
                {
                    dict.Add(temp, i);                                                                              // 1
                }

                using (BinaryWriter bin = new BinaryWriter(s)) // начинаем запись в файл
                {
                    bin.Write(Count); // записываем в первую очередь размер ранд-списка
                    for (ListNode temp = Head; !(temp is null); temp = temp.Next)                                       // N
                    {
                        bin.Write(temp.Data); // записываем данные узла
                        bin.Write((temp.Rand is null) ? -1 : dict[temp.Rand]); // записываем уникальный индекс ранд-узла
                    }
                }
            }
        }

        public void Deserialize(FileStream s)
        {
            try
            {
                using BinaryReader bin = new BinaryReader(s); // начинаем работы с файлом

                int len = bin.PeekChar() > -1 ? bin.ReadInt32() : -1; // считываем размер списка
                if (len < 0) // если размер списка не задан
                {
                    throw new Exception("File is empty"); // файл пуст
                }

                List<Tuple<ListNode, int>> arr = new List<Tuple<ListNode, int>>(len); // создаем список элементов ранд-списка и индексов рандомных элементов
                ListNode temp = new ListNode(); // создаем первый пустой узел
                Count = 0; // размер ранд-списка = 0
                Head = temp; // голова ранд-списка равна первому узлу

                while (bin.PeekChar() > -1) // пока есть что считывать                                          // N
                {
                    String data = bin.ReadString(); // считали данные
                    int randomId = bin.ReadInt32(); // считали индекс рандомного эл-та
                    Count++; // увеличили количество элементов в ранд-списке

                    temp.Data = data; // инициализируем узел ранд-списка
                    ListNode next = new ListNode(); // создаем следующий узел
                    temp.Next = next;
                    next.Prev = temp;
                    arr.Add(new Tuple<ListNode, int>(temp, randomId)); // добавляем узел и ранд-индекс в список // 1

                    temp = next; // итерируемся на следующий узел
                }

                Tail = temp.Prev; // хвост ранд-списка
                Tail.Next = null; // занулили указатель хвоста

                foreach (var n in arr) // итерируемся по созданному списку                                      // N
                {
                    n.Item1.Rand = n.Item2 < 0 ? null : arr[n.Item2].Item1; // берем из списка по индексу нужный ранд-узел
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR IN DESERIALIZATION: ");
                Console.WriteLine(e.Message);
                Console.Read();
                Environment.Exit(0);
            }
        }



        //пустой конструктор списка
        public ListRand() { }

        // конструктор списка по длине
        public ListRand(int length)
        {
            if (length > 0)
            {
                ListNode head = new ListNode(); // основные ноды для генерация списка 
                Count = 1;
                Head = head;
                Tail = head;
                head.Prev = null;
                head.Next = null;
                head.Rand = null;
                //head.Data = Count.ToString();
                head.Data = rand.Next(0, 99).ToString(); // рандомим дату для первого нода

                for (int i = 1; i < length; i++) // добавляем нужное количество нодов в список
                {
                    AddNodeToTail(); // добавление ноды в списокы
                }

                ListNode temp = Head;
                for (int i = 0; i < Count; i++)
                {
                    temp.Rand = GetRandomNode(); // добавляем элементу ссылку на рандомный элемент списка
                    temp = temp.Next; // перемещаем указатель текущего эелемнта
                }
            }
        }

        // добавляение ноды в конец списка
        private void AddNodeToTail()
        {
            ListNode newNode = new ListNode();
            Count++;
            newNode.Prev = Tail;
            newNode.Next = null;
            newNode.Rand = null;
            //newNode.Data = Count.ToString();
            newNode.Data = rand.Next(0, 99).ToString();

            Tail.Next = newNode;
            Tail = newNode;
        }

        // вернуть рандомный нод из списка
        private ListNode GetRandomNode()
        {
            int x = rand.Next(-1, Count);

            return x < 0 ? null : GetNodeFromIndex(x);
        }

        // вернуть нод списка по индексу от головы
        private ListNode GetNodeFromIndex(int index)
        {
            ListNode temp = Head;
            for (int i = 0; i < index; i++)
            {
                temp = temp.Next;
            }
            return temp;
        }

        // вывод списка в консоль
        public void Print()
        {
            if (Head != null)
            {
                ListNode temp = Head;
                Console.WriteLine("ListRand: " + Count.ToString() + ", Head - " + Head.Data + ", Tail - " + Tail.Data);
                for (int i = 0; i < Count; i++)
                {
                    string str = "";
                    str += "DATA: " + temp.Data;
                    str += ", Prev: " + ((temp.Prev is null) ? "null" : temp.Prev.Data);
                    str += ", Next: " + ((temp.Next is null) ? "null" : temp.Next.Data);
                    str += ", Rand: " + ((temp.Rand is null) ? "null" : temp.Rand.Data);
                    Console.WriteLine(str);
                    temp = temp.Next; // перемещаем указатель текущего эелемнта
                }
            }
            else
            {
                Console.WriteLine("ListRand is empty");
            }
        }
    }


    class Program
    {
        const int LENGTH = 5; // длина генерируемого для теста списка

        static void Main(string[] args)
        {
            ListRand first = new ListRand(LENGTH); // инициализируем ранд-лист
                                                   //first.Print();


            // СЕРИАЛИЗАЦИЯ
            FileStream fs = new FileStream("dat.dat", FileMode.OpenOrCreate); //инициализируем поток файла
            first.Serialize(fs); // сереализуем полученный лист в файл
            fs.Close();


            // ДЕСЕРИАЛИЗАЦИЯ
            ListRand second = new ListRand(); // создаем новый лист
            try
            {
                fs = new FileStream("dat.dat", FileMode.Open); // пытаемся открыть поток заданного файла, если он есть
            }
            catch (Exception e) // если файла нет
            {
                Console.WriteLine("ERROR: ");
                Console.WriteLine(e.Message); // выводим сообщение ошибки
                Console.Read();
            }
            second.Deserialize(fs); // десерализуем лист из файла
            fs.Close();


            // СРАВНЕНИЕ СПИСКОВ
            first.Print();
            Console.WriteLine();
            second.Print();



            Console.Read();
        }
    }
}
