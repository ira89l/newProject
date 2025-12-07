using SQLite;
using System;

namespace CrossHealthX.Models
{
    public class Activity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }             // Унікальний ідентифікатор запису
        public int Steps { get; set; }          // Кількість кроків за день
        public int Calories { get; set; }       // Спалені калорії
        public double Distance { get; set; }    // Пройдена дистанція у км
        public DateTime Date { get; set; }      // Дата запису
    }
}
