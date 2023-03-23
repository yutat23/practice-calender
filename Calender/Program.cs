using System;
using System.Globalization;
using System.Threading;

class CalendarApp
{
    const int MaxCalendarRow = 8;

    static void Main(string[] args)
    {
        int year, month;
        if (args.Length == 2)
        {
            year = int.Parse(args[0]);
            month = int.Parse(args[1]);
        }
        else
        {
            DateTime now = DateTime.Now;
            year = now.Year;
            month = now.Month;
        }
        var point = Console.GetCursorPosition();
        Console.CursorVisible = false;
        if (Console.BufferHeight - point.Top < MaxCalendarRow)
        {
            Console.WriteLine($"{Console.BufferHeight} {point.Top} {MaxCalendarRow}");
            Console.Clear();
            point = (0, 0);
        }
        DrawCalendar(year, month, point);
        while (true)
        {
            DrawClock(point);

            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(intercept: true).Key;
                switch (key)
                {
                    case ConsoleKey.F:
                        if (month == 12) { year++; month = 1; }
                        else month++;
                        DrawCalendar(year, month, point);
                        DrawClock(point);
                        break;
                    case ConsoleKey.B:
                        if (month == 1)
                        {
                            year--;
                            month = 12;
                        }
                        else month--;
                        DrawCalendar(year, month, point);
                        DrawClock(point);
                        break;
                    case ConsoleKey.N:
                        DateTime now = DateTime.Now;
                        year = now.Year;
                        month = now.Month;
                        DrawCalendar(year, month, point);
                        DrawClock(point);
                        break;
                    case ConsoleKey.Q:
                        //ClearCalendarArea(point);
                        Console.SetCursorPosition(point.Left, point.Top+MaxCalendarRow);
                        return;
                }
            }
            Thread.Sleep(100);
        }
    }

    static void DrawClock((int Left, int Top) point)
    {
        DateTime now = DateTime.Now;
        Console.SetCursorPosition(0, point.Top);
        Console.Write(now.ToString("yyyy/MM/dd HH:mm:ss"));
    }

    static void DrawCalendar(int year, int month, (int Left, int Top) point)
    {
        DateTime today = DateTime.Today;
        DateTime firstDayOfMonth = new DateTime(year, month, 1);
        int daysInMonth = DateTime.DaysInMonth(year, month);
        int startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;

        Console.SetCursorPosition(0, point.Top+1);
        Console.WriteLine($"{year}年 {month}月 ");
        Console.WriteLine("日 月 火 水 木 金 土");

        int day = 1;
        for (int i = 0; i < 37; i++)
        {
            if (i < startDayOfWeek || day > daysInMonth)
            {
                Console.Write("   ");
            }
            else
            {
                if (today.Year == year && today.Month == month && today.Day == day)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{day,2} ");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write($"{day,2} ");
                }
                day++;
            }

            if (i % 7 == 6)
            {
                Console.WriteLine();
            }
        }
    }

    static void ClearCalendarArea((int Left, int Top) point)
    {
        var calendarAreaRow = point.Top + MaxCalendarRow;
        for (int row = point.Top; row < calendarAreaRow; row++)
        {
            Console.SetCursorPosition(0, row);
            Console.Write(new string(' ', Console.WindowWidth));
        }
    }
}