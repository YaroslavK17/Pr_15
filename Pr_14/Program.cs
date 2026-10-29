using System;
using System.Collections.Generic;

namespace Pr_14
{
    // 1. Описание структуры "Пациенты"
    public struct Patient
    {
        // Скрытие внутренней реализации: поля приватные
        private string _surname;
        private string _initials;
        private DateTime _birthDate;
        private string _gender;
        private string _diagnosis;
        private string _doctor;

        // Публичные свойства для доступа к полям (Инкапсуляция)
        public string Surname { get { return _surname; } set { _surname = value; } }
        public string Initials { get { return _initials; } set { _initials = value; } }
        public DateTime BirthDate { get { return _birthDate; } set { _birthDate = value; } }
        public string Gender { get { return _gender; } set { _gender = value; } }
        public string Diagnosis { get { return _diagnosis; } set { _diagnosis = value; } }
        public string Doctor { get { return _doctor; } set { _doctor = value; } }

        // Метод для красивого вывода одного пациента
        public void OutputInfoObjStruct()
        {
            Console.WriteLine($"{Surname} {Initials} | Дата рождения: {BirthDate:dd.MM.yyyy} | Пол: {Gender} | Диагноз: {Diagnosis} | Врач: {Doctor}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Предусматриваем многократное выполнение программы без перезапуска
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== СИСТЕМА УЧЕТА ПАЦИЕНТОВ ===");

                int n = 0;
                // Обработка ошибок при вводе количества пациентов
                while (true)
                {
                    try
                    {
                        Console.Write("Введите количество пациентов (n): ");
                        n = int.Parse(Console.ReadLine());
                        if (n > 0) break;
                        Console.WriteLine("Количество должно быть больше нуля!");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Ошибка! Введите целое число.");
                    }
                }

                // Создаем массив структур
                Patient[] patients = new Patient[n];

                // 2. Ввод данных с клавиатуры
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine($"\nВвод данных для пациента №{i + 1}:");
                    patients[i] = InfoAboutObjStruct();
                }

                // Сортируем данные (по фамилии) - метод SortedObjStruct
                SortedObjStruct(patients);

                Console.WriteLine("\n=== СПИСОК ВСЕХ ПАЦИЕНТОВ (Отсортировано по фамилии) ===");
                foreach (var p in patients)
                {
                    p.OutputInfoObjStruct();
                }

                // 3. Вывод количества пациентов для каждого лечащего врача
                Console.WriteLine("\n=== КОЛИЧЕСТВО ПАЦИЕНТОВ ПО ВРАЧАМ ===");
                CountByDoctor(patients);

                // 4. Вывод информации о пациентах с определенным диагнозом
                Console.Write("\nВведите диагноз для поиска: ");
                string searchDiagnosis = Console.ReadLine();
                FindObjStruct(patients, searchDiagnosis);

                // 5. Статистика в процентах
                Console.WriteLine("\n=== СТАТИСТИКА В ПРОЦЕНТАХ ===");
                PrintStatistics(patients);

                // Спрашиваем, хочет ли пользователь продолжить
                Console.Write("\nХотите выполнить программу еще раз? (y/n): ");
                if (Console.ReadLine().ToLower() != "y") break;
            }
        }

        // Метод ввода данных (InfoAboutObjStruct)
        static Patient InfoAboutObjStruct()
        {
            Patient p = new Patient();

            Console.Write("Фамилия: ");
            p.Surname = Console.ReadLine();

            Console.Write("Инициалы: ");
            p.Initials = Console.ReadLine();

            // Обработка ошибок при вводе даты (используем DateTime)
            while (true)
            {
                Console.Write("Дата рождения (дд.мм.гггг): ");
                string dateStr = Console.ReadLine();
                // Пытаемся преобразовать строку в дату
                if (DateTime.TryParseExact(dateStr, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    p.BirthDate = date;
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный формат даты! Попробуйте снова.");
                }
            }

            Console.Write("Пол (М/Ж): ");
            p.Gender = Console.ReadLine();

            Console.Write("Диагноз: ");
            p.Diagnosis = Console.ReadLine();

            Console.Write("Лечащий врач: ");
            p.Doctor = Console.ReadLine();

            return p;
        }

        // Метод поиска по диагнозу (FindObjStruct)
        static void FindObjStruct(Patient[] patients, string diagnosis)
        {
            bool found = false;
            Console.WriteLine($"\n=== ПАЦИЕНТЫ С ДИАГНОЗОМ: {diagnosis.ToUpper()} ===");

            foreach (var p in patients)
            {
                // Сравниваем без учета регистра
                if (p.Diagnosis.Equals(diagnosis, StringComparison.OrdinalIgnoreCase))
                {
                    p.OutputInfoObjStruct();
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Пациентов с таким диагнозом не найдено.");
            }
        }

        // Метод сортировки (SortedObjStruct)
        static void SortedObjStruct(Patient[] patients)
        {
            // Сортируем массив по фамилии с помощью лямбда-выражения
            Array.Sort(patients, (p1, p2) => string.Compare(p1.Surname, p2.Surname, StringComparison.OrdinalIgnoreCase));
        }

        // Подсчет по врачам
        static void CountByDoctor(Patient[] patients)
        {
            // Используем Dictionary для подсчета уникальных врачей
            var doctorCounts = new Dictionary<string, int>();

            foreach (var p in patients)
            {
                if (doctorCounts.ContainsKey(p.Doctor))
                    doctorCounts[p.Doctor]++;
                else
                    doctorCounts[p.Doctor] = 1;
            }

            foreach (var kvp in doctorCounts)
            {
                Console.WriteLine($"Врач {kvp.Key}: {kvp.Value} пациент(ов)");
            }
        }

        // Вывод статистики в процентах
        static void PrintStatistics(Patient[] patients)
        {
            int total = patients.Length;

            // Статистика по полу
            Console.WriteLine("--- По полу ---");
            CountAndPrintPercentage(patients, p => p.Gender, total);

            // Статистика по диагнозу
            Console.WriteLine("--- По диагнозам ---");
            CountAndPrintPercentage(patients, p => p.Diagnosis, total);

            // Статистика по врачам
            Console.WriteLine("--- По лечащим врачам ---");
            CountAndPrintPercentage(patients, p => p.Doctor, total);
        }

        // Вспомогательный метод для расчета процентов (чтобы не дублировать код)
        static void CountAndPrintPercentage(Patient[] patients, Func<Patient, string> selector, int total)
        {
            var counts = new Dictionary<string, int>();
            foreach (var p in patients)
            {
                string key = selector(p);
                if (counts.ContainsKey(key)) counts[key]++;
                else counts[key] = 1;
            }

            foreach (var kvp in counts)
            {
                double percent = ((double)kvp.Value / total) * 100;
                Console.WriteLine($"{kvp.Key}: {percent:F2}% ({kvp.Value} чел.)");
            }
        }
    }
}