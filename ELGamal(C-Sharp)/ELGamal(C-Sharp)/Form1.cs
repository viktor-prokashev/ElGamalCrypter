using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace ELGamal_C_Sharp_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        public void button1_Click(object sender, EventArgs e)//Генерация ключей
        {
            
            Random rand = new Random();//Объект получения случайных чисел
            Int64 p = rand.Next(65535, 2147483647);//Генерируем случайное простое число определённой длины
            while (!BigInteger.IsPrime(p))//Проверяем число на простоту
            {
                p = rand.Next(65536, 2147483647);//Генерируем случайное простое число определённой длины
            }
            BigInteger x = new BigInteger(rand.Next(4095));//Генерируем закрытый ключ
            BigInteger g = new BigInteger(2);//Первообразный корень
            BigInteger y = (g ^ x) % p;//Находим последний компонент открытого ключа
            textBox1.Text = (Int64.Parse(x.ToString())).ToString("X8");//Выводим х на экран
            textBox2.Text = (Int64.Parse(p.ToString())).ToString("X8");//Выводим р на экран
            textBox3.Text = (Int64.Parse(g.ToString())).ToString("X8");//Выводим g на экран
            textBox4.Text = (Int64.Parse(y.ToString())).ToString("X8");//Выводим y на экран
            textBox5.Text = textBox2.Text + textBox3.Text + textBox4.Text;//Формируем текст открытого ключа
        }
        private void button3_Click(object sender, EventArgs e)//Шифрование
        {
            richTextBox4.Text = "";//Очищаем поле вывода шифротекста
            Random rand = new Random();//Объект получения случайных чисел
            int end = textBox6.Text.Length;//Длина ключа
            if (end == 24)//Проверка длины ключа
            {
                string[] buffer = new string[3];//Создаём буфер для компонентов открытого ключа
                for (int i = 0; i < end; i += 8)//Цикл разбиения открытого ключа на компоненты
                {
                    string buf = "";//Буферная переменная
                    int k = 0;//Индекс текущего символа
                    while (k < 8)//Перечисление разрядов числа
                    {
                        buf = buf + textBox6.Text[i + k];//Конкатенация
                        k++;//Следующий символ
                    }
                    buffer[i / 8] = buf;//Сохранение части в массив частей
                }
                int nice = 1;//Состояние шифрования
                Int64 Out;//Буферная переменная
                CultureInfo provider = new CultureInfo("en-US");//Региональные параметры
                BigInteger p = new BigInteger();//Часть открытого ключа
                BigInteger g = new BigInteger();//Часть открытого ключа
                BigInteger y = new BigInteger();//Часть открытого ключа
                if (Int64.TryParse(buffer[0], System.Globalization.NumberStyles.HexNumber, provider, out Out))//Преобразование в число
                {
                    p = Out;//Сохнанение р
                }
                else
                {
                    nice = 0;//Ошибка в ключе
                }
                if (Int64.TryParse(buffer[1], System.Globalization.NumberStyles.HexNumber, provider, out Out))//Преобразование в число
                {
                    g = Out;//Сохнанение g
                }
                else
                {
                    nice = 0;//Ошибка в ключе
                }
                if (Int64.TryParse(buffer[2], System.Globalization.NumberStyles.HexNumber, provider, out Out))//Преобразование в число
                {
                    y = Out;//Сохнанение y
                }
                else
                {
                    nice = 0;//Ошибка в ключе
                }
                if (nice == 0)//Проверка состояния шифрования
                {
                    richTextBox4.Text = "";//Очищаем поле вывода шифротекста
                    MessageBox.Show("Возможно неправильный ключ");//Предупреждение
                }
                else
                {
                    for (int i = 0; i < richTextBox3.Text.Length; i++)//Цикл шифрации текста из поля ввода
                    {
                        Int64 k = rand.Next(128); ;//Генерация сессионного ключа
                        Int64 M = richTextBox3.Text[i];//Взятие одного символа из поля ввода
                        Int64 a = Int64.Parse(((g^k)%p).ToString());//Вычисление первой компоненты шифротекста
                        Int64 b = Int64.Parse((((y^k)*M)%p).ToString());//Вычисление второй компоненты шифротекста
                        richTextBox4.Text = richTextBox4.Text + a.ToString("X8") + b.ToString("X8");//Формирование шифротекста для одного символа
                    }
                }
            }
            else
            {
                MessageBox.Show("Неправильный ключ");//Предупреждение
            }
        }
        private void button2_Click(object sender, EventArgs e)//Расшивровка
        {
            BigInteger p = new BigInteger();//Часть открытого ключа
            BigInteger x = new BigInteger();//Закрытый ключ
            Int64 Out;//Буферная переменная
            CultureInfo provider = new CultureInfo("en-US");//Региональные параметры
            if (Int64.TryParse(textBox2.Text, System.Globalization.NumberStyles.HexNumber, provider, out Out))//Преобразование в число
            {
                p = Out;//Сохнанение р
            }
            else
            {
                MessageBox.Show("Возможно неправильный ключ");//Предупреждение
            }
            if (Int64.TryParse(textBox1.Text, System.Globalization.NumberStyles.HexNumber, provider, out Out))//Преобразование в число
            {
                x = Out;//Сохнанение х
            }
            else
            {
                MessageBox.Show("Возможно неправильный ключ");//Предупреждение
            }
            richTextBox2.Text = "";//Очищаем поле вывода сообщения
            int end = richTextBox1.Text.Length;//Длина шифротекста
            string[] buffer = new string[end / 8];//Создание буфера для компонентов шифротекста
            if (end >= 16)//Проверка длинны шифротекста 
            {
                if ((end % 16) != 0)//Проверка количества кодовых слов
                {
                    MessageBox.Show("Возможно неправильный шифротекст");//Предупреждение
                    end = end - (end % 16);//Устранение лишнего
                    buffer = new string[end / 8];//Изменение буфера
                }
                for (int i = 0; i < end; i += 8)//Цикл разбиения шифротекста на компоненты
                {
                    string buf = "";//Буферная переменная
                    int k = 0;//Индекс текущего символа
                    while (k < 8)//Перечисление разрядов числа
                    {
                        buf = buf + richTextBox1.Text[i + k];//Конкатенация
                        k++;//Следующий символ
                    }
                    buffer[i / 8] = buf;//Сохранение части в массив частей
                }
                for (int i = 0; i < buffer.Length; i += 2)//Цикл расшифровки
                {
                    BigInteger a = new BigInteger();//Часть шифротекста
                    BigInteger b = new BigInteger();//Часть шифротекста
                    if (Int64.TryParse(buffer[i], System.Globalization.NumberStyles.HexNumber, provider, out Out))//Преобразование в число
                    {
                        a = Out;//Получение первой компоненты шифротекста
                    }
                    else
                    {
                        MessageBox.Show("Неправильный шифротекст");//Предупреждение
                    }
                    if (Int64.TryParse(buffer[i + 1], System.Globalization.NumberStyles.HexNumber, provider, out Out))//Преобразование в число
                    {
                        b = Out;//Получение второй компоненты шифротекста
                    }
                    else
                    {
                        MessageBox.Show("Неправильный шифротекст");//Предупреждение
                    }
                    BigInteger M = (b * BigInteger.ModularInverse((a^x), p))%p;//Вычисление символа сообщения
                    richTextBox2.Text = richTextBox2.Text + (char)Int64.Parse(M.ToString());//Формирование сообщения
                }
            }
            else
            {
                MessageBox.Show("Неправильный шифротекст");//Предупреждение
            }
        }
    }
}
