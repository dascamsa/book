using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Pages { get; set; }
    public string Genre { get; set; }
    public string Publisher { get; set; }
    public string ISBN { get; set; }

    public static bool ValidateISBN(string isbn)
    {
   
        return Regex.IsMatch(isbn, @"^\d{10}|\d{13}$");
    }

    public static Book ReadFromFile(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        if (lines.Length < 6)
            throw new Exception("Invalid file format");

        return new Book
        {
            Title = lines[0],
            Author = lines[1],
            Pages = int.Parse(lines[2]),
            Genre = lines[3],
            Publisher = lines[4],
            ISBN = lines[5]
        };
    }

    public void WriteToFile(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(Title);
            writer.WriteLine(Author);
            writer.WriteLine(Pages);
            writer.WriteLine(Genre);
            writer.WriteLine(Publisher);
            writer.WriteLine(ISBN);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Book Management System!");

        while (true)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Pick books");
            Console.WriteLine("2. Extract email addresses");
            Console.WriteLine("3. Use dictionary");
            Console.WriteLine("4. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ManageBooks();
                    break;
                case "2":
                    ExtractEmails();
                    break;
                case "3":
                    UseDictionary();
                    break;
                case "4":
                    Console.WriteLine("Exiting program...");
                    return;
                default:
                    Console.WriteLine("Please enter a valid option.");
                    break;
            }
        }
    }

    static void ManageBooks()
    {
        Console.WriteLine("\nBooks Menu:");
        Console.WriteLine("1. Create a new book");
        Console.WriteLine("2. Search for an existing book");
        Console.WriteLine("3. List all books");
        Console.WriteLine("4. Back to main menu");
        Console.Write("Enter your choice: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                CreateNewBook();
                break;
            case "2":
                SearchForBook();
                break;
            case "3":
                ListAllBooks();
                break;
            case "4":
                Console.WriteLine("Returning to main menu...");
                break;
            default:
                Console.WriteLine("Invalid choice. Returning to main menu...");
                break;
        }
    }

    static void CreateNewBook()
    {
        Book book = new Book();

        Console.WriteLine("\nEnter book details:");

        Console.Write("Title: ");
        book.Title = Console.ReadLine();

        Console.Write("Author: ");
        book.Author = Console.ReadLine();

        Console.Write("Pages: ");
        book.Pages = int.Parse(Console.ReadLine());

        Console.Write("Genre: ");
        book.Genre = Console.ReadLine();

        Console.Write("Publisher: ");
        book.Publisher = Console.ReadLine();

        Console.Write("ISBN: ");
        book.ISBN = Console.ReadLine();


        if (!Book.ValidateISBN(book.ISBN))
        {
            Console.WriteLine("Invalid ISBN");
            return;
        }

        string filePath = "book_info.txt";
        using (StreamWriter writer = new StreamWriter(filePath, append: true))
        {
            writer.WriteLine(book.Title);
            writer.WriteLine(book.Author);
            writer.WriteLine(book.Pages);
            writer.WriteLine(book.Genre);
            writer.WriteLine(book.Publisher);
            writer.WriteLine(book.ISBN);
        }

        Console.WriteLine("Book created successfully!");
    }

    static void SearchForBook()
    {
        string filePath = "book_info.txt";
        if (!File.Exists(filePath))
        {
            Console.WriteLine("No books found.");
            return;
        }

        // Read book information from the file
        Book book = Book.ReadFromFile(filePath);

        // Display read book information
        Console.WriteLine("\nBook found:");
        Console.WriteLine("Title: " + book.Title);
        Console.WriteLine("Author: " + book.Author);
        Console.WriteLine("Pages: " + book.Pages);
        Console.WriteLine("Genre: " + book.Genre);
        Console.WriteLine("Publisher: " + book.Publisher);
        Console.WriteLine("ISBN: " + book.ISBN);
    }

    static void ListAllBooks()
    {
        string filePath = "book_info.txt";
        if (!File.Exists(filePath))
        {
            Console.WriteLine("No books found.");
            return;
        }

        // Read all lines from the file
        string[] lines = File.ReadAllLines(filePath);

        // Display book information
        Console.WriteLine("\nList of all books:");
        for (int i = 0; i < lines.Length; i += 6)
        {
            Console.WriteLine("\nBook " + (i / 6 + 1) + ":");
            Console.WriteLine("Title: " + lines[i]);
            Console.WriteLine("Author: " + lines[i + 1]);
            Console.WriteLine("Pages: " + lines[i + 2]);
            Console.WriteLine("Genre: " + lines[i + 3]);
            Console.WriteLine("Publisher: " + lines[i + 4]);
            Console.WriteLine("ISBN: " + lines[i + 5]);
        }
    }

    static void ExtractEmails()
    {
        Console.WriteLine("\nEnter text to extract email addresses from:");
        string text = Console.ReadLine();

        // Regular expression pattern for matching email addresses
        string pattern = @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}\b";

        // Find all matches using Regex.Matches method
        MatchCollection matches = Regex.Matches(text, pattern);

        // Display the found email addresses
        Console.WriteLine("\nEmail addresses found in the text:");
        foreach (Match match in matches)
        {
            Console.WriteLine(match.Value);
        }
    }

    static void UseDictionary()
    {
        Console.WriteLine("\nUsing Dictionary:");

        Dictionary<string, (string Name, int Grade)> studentData = new Dictionary<string, (string, int)>();
        AddInitialStudents(studentData);

        while (true)
        {
            Console.WriteLine("\nDictionary Menu:");
            Console.WriteLine("1. Create a new student");
            Console.WriteLine("2. List all students");
            Console.WriteLine("3. Back to main menu");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateNewStudent(studentData);
                    break;
                case "2":
                    ListAllStudents(studentData);
                    break;
                case "3":
                    Console.WriteLine("Returning to main menu...");
                    return;
                default:
                    Console.WriteLine("Please enter a valid option.");
                    break;
            }
        }
    }
    static void AddInitialStudents(Dictionary<string, (string, int)> studentData)
    {
        AddStudent(studentData, "A001", "Alice", 85);
        AddStudent(studentData, "B002", "Bob", 92);
    }
    static void CreateNewStudent(Dictionary<string, (string, int)> studentData)
    {
        Console.Write("\nEnter student ID: ");
        string id = Console.ReadLine();

        Console.Write("Enter student name: ");
        string name = Console.ReadLine();

        Console.Write("Enter student grade: ");
        int grade = int.Parse(Console.ReadLine());

        AddStudent(studentData, id, name, grade);

        Console.WriteLine("Student created successfully!");
    }

    static void AddStudent(Dictionary<string, (string, int)> dictionary, string id, string name, int grade)
    {
        dictionary[id] = (name, grade);
    }

    static void ListAllStudents(Dictionary<string, (string, int)> studentData)
    {
        if (studentData.Count == 0)
        {
            Console.WriteLine("No students found.");
            return;
        }

        Console.WriteLine("\nList of all students:");
        foreach (var entry in studentData)
        {
            Console.WriteLine($"\"{entry.Key}\" -> {entry.Value.Item1}, {entry.Value.Item2}");
        }
    }
}

