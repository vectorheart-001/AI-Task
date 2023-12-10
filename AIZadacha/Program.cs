//редовете в таблицата.Отговарят на броя на децата.
string[] dete5 = new string[2] {"0","0"};
string[] dete7 = new string[2] {"0", "0" };
string[] dete8 = new string[2] {"0", "0" };
string[] dete4 = new string[2] {"0", "0"};
string[] dete6 = new string[2] { "0", "0" };
string remainingInterest = "Piano";
string remainingName = "Nevena";
//Речниците за отрицания(Пример:Йона не е на 8,Иванка не харесва математика)
Dictionary<string, string> NegativeBacklogName = new Dictionary<string, string>();
Dictionary<string, string> NegativeBacklogInterest = new Dictionary<string, string>();
//Таблицата,която се създава чрез масиви в масиви.
string[][] arrays = new string[5][];
arrays[0] = dete4;
arrays[1] = dete5;
arrays[2] = dete6;
arrays[3] = dete7;
arrays[4] = dete8;
//Масив за указанията
List<string[]> Instructions = new();
//Масив за указанията,които имат недостатъчна информация.
List<string[]> BacklogInstructions = new();
/*Указанията се декларират и добавят в масив
  Те се създават по следния начин:
  -първият елемент определя операцията,която ще се извърши
      -"i" - в това укание има само информация без допълнителни условия.Информацията се записва в на един ред в таблицата
      -"l" - тук има някакво сравнение между децата.Информацията се записва на текущия и следващия ред
  -вторият елемент е възраст
  -третият елемент е име
  -четвъртият елемент е интерес
  -минусът обозначава отрицание
 * */
string[] instruction1 = new string[4] { "i", "4", "Ivanka", "-Math" };
string[] instruction2 = new string[4] { "l", "0", "Ivan", "Programmer" };
string[] instruction3 = new string[4] { "i", "7", "0", "Guitar" };
string[] instruction4 = new string[4] { "i", "8", "-Iona", "0", };
string[] instruction5 = new string[4] { "l", "5", "Stanko", "Literature" };
Instructions.Add(instruction1);
Instructions.Add(instruction2);
Instructions.Add(instruction3);
Instructions.Add(instruction4);
Instructions.Add(instruction5);
//Речник,които съдържа операции.Използваме речник,за да избегнем допълнителни проверки
Dictionary<string, Action<string[]>> functions = new();
functions.Add("i", (p) =>
{
    string[] child = arrays[int.Parse(p[1]) - 4];
    for (int j = 0; j < child.Length; j++)
    {
        //Проверяваме за минус в елемента
        if (p[j + 2][0] == '-')
        {
            /*елемент на трета позиция-добавя се в речника за имена(отрицание)
             * елемент на трета позиция-добавя се в речника за интереси(отрицание)
             */
            if (j == 0)
                NegativeBacklogName.Add(p[1], p[j+2].Remove(0, 1));
            else 
                NegativeBacklogInterest.Add(p[1], p[j + 2].Remove(0, 1));
        }
        else
        {
            child[j] = p[j+2];
        }
    }
});
functions.Add("l", (p) => {
    if (p[1] != "0")
    {
        string[] child1 = arrays[int.Parse(p[1]) - 4];
        string[] child2 = arrays[int.Parse(p[1]) - 3];
        child1[0] = p[2];
        child2[1] = p[3];
    }
    else
    {
        //Проверявама дали името на текущото дете и интереса на слеващато поред дете са празни
        for (int i = 0; i < arrays.Length - 1; i++)
        {
            if (arrays[i][0] == "0" && arrays[i + 1][1] == "0")
            {
                arrays[i][0] = p[2];
                arrays[i + 1][1] = p[3];
            }
        }

    }
});
//Четем указнията
foreach (var item in Instructions)
{
    //Проверка дали има достатъчна информация в това указние,за да можем да попълним таблицата правилно
    if (item[1] == "0")
    {
        //Добавяме указнието в масив,които ще бъде обходен по-късно
        BacklogInstructions.Add(item);
    }
    else
    {
        //четем първият елемент и изпълняваме съответната операция
        functions[item[0]](item);
    }
    PrintArray(arrays);
}
//Четем указнията
foreach (var item in BacklogInstructions)
{
    functions[item[0]](item);
    PrintArray(arrays);
}
//декларираме кортешж за неизполваните имена.Той ни казва на каква позиция и дали е намерено на каква позиция седи неизползваното име,за да 
//мощем да намерим позицията на инетереса от речника с отрицание
(bool,int) foundIndexName = (false,0);
//декларираме кортеж за неизполваните имена.Той ни казва на каква позиция и дали е намерено на каква позиция седи неизползваният интерес,за да
//мощем да намерим позицията на инетереса от речника с отрицание
(bool,int) foundIndexInterest = (false, 0);
int i = 0;
//цъкъла ще продължи до премаване на името от речника с отрицание
while (NegativeBacklogName.Count != 0)
{
    //проверяваме за празни имена 
    if (arrays[i][0] == "0") {

        if (NegativeBacklogName.ContainsKey((i + 4).ToString()))
        {
            //попълваме кортежа
            foundIndexName.Item1 = true;
            foundIndexName.Item2 = i + 4;
            arrays[i][0] = remainingName;
        }
        else if (foundIndexName.Item1 == true)
        {
            //попълваме таблицата с името от речника с отрицание
            arrays[i][0] = NegativeBacklogName[(foundIndexName.Item2).ToString()];
            NegativeBacklogName.Remove((foundIndexName.Item2).ToString());
        }

    }
    if (i != 4)
    {
        i++;
    }
    else
    {
        i = 0;
    }

}
i = 0;
//Аналогична операция за инетересите
while (NegativeBacklogInterest.Count != 0)
{

    if (arrays[i][1] == "0")
    {

        if (NegativeBacklogInterest.ContainsKey((i + 4).ToString()))
        {
            arrays[i][1] = remainingInterest;
            foundIndexInterest.Item1 = true;
            foundIndexInterest.Item2 = i + 4;
        }
        else if (foundIndexInterest.Item1 == true )
        {
            arrays[i][1] = NegativeBacklogInterest[(foundIndexInterest.Item2).ToString()];
            NegativeBacklogInterest.Remove((foundIndexInterest.Item2).ToString());
        }
        if (i != 4)
        {
            i++;
        }
        else
        {
            i = 0;
        }

    }
}

PrintArray(arrays);

//метод за принтиране в конзолата.
void PrintArray(string[][] arr)
{
    Console.WriteLine("###############################");
    foreach (var item in arr)
    {
        Console.WriteLine($"{nameof(item)} - {item[0]} - {item[1]}");
    }
    Console.WriteLine("###############################");
    Console.WriteLine();
}


