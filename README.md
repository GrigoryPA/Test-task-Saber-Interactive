# Test-task-Saber-Interactive
 
## Тестовое задание в компанию Saber Interactive на должность программиста игровой логики (Junior).

----------
**1. Реализация функций сериализации и десериализации**

Реализуйте функции сериализации и десериализации двусвязного списка, заданного следующим образом:

``` c#
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
        public int Count;

        public void Serialize(FileStream s)
        {
        }

        public void Deserialize(FileStream s)
        {
        }
    }
```  

Примечание: сериализация подразумевает сохранение и восстановление полной структуры списка, включая взаимное соотношение его элементов между собой — в том числе ссылок на Rand элементы.
Алгоритмическая сложность решения должна быть меньше квадратичной.
Нельзя добавлять новые поля в исходные классы ListNode, ListRand
Для выполнения задания можно использовать любой общеиспользуемый язык.
Тест нужно выполнить без использования библиотек/стандартных средств сериализации.

--------------
**2. Проектирование дерева поведения ИИ.**

Напишите ИИ  для противника используя BhvTree (достаточно нарисовать схему, реализация в каком-либо из движков не требуется).
Солдат - сущность, которая может стрелять, перезаряжаться, отправиться в указанную точку и ждать.
Солдат занимается патрулем из точки А в точку Б
Когда он прошел патруль, он сразу же повторяет маршрут
Если солдат замечает врага, он подходит к нему, пока не окажется на дистанции в 35 метров
Достигнув дистанции 35 метров, солдат начинает стрелять по врагу, пока не потеряет его из виду.

Реализованнное дерево поведения:
![Soldier`s Behavior Tree drawio](https://user-images.githubusercontent.com/62347783/182025558-e86eaba9-b779-4955-a6d9-4d938c3bfde9.png)



