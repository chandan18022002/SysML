namespace StudentLibrary
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Book> books = new List<Book>();
            books.Add(new Book(1, "Cycles of Time", "Roger Penrose"));
            books.Add(new Book(2, "EW Modeling and Simulation", "David Adamy"));
            books.Add(new Book(3, "The Black Hole War", "Leonard Susskind"));
            books.Add(new Book(4, "Hitchhiker's Guide to the Galaxy", "Douglas Adams"));

            Library library = new Library(books);
            List<Student> students = new List<Student>();

            students.Add(new Student("Alice", 1, library));
            students.Add(new Student("Bob", 2, library));
            students.Add(new Student("Charlie", 3, library));

            Console.WriteLine("\nBooks in library:\n");
            foreach (Book book in library.bookList)
            {
                Console.WriteLine($"{book.title} : {book.available}");
            }
            Console.WriteLine($"\nStudent list:\n");
            foreach (Student student in students)
            {
                Console.WriteLine(student.name);
            }

            students[0].getBook("Cycles of Time");
            students[0].returnBook("The Black Hole War");
            Console.WriteLine("\nBooks in library:\n");
            foreach (Book book in library.bookList)
            {
                Console.WriteLine($"{book.title} : {book.available}");
            }
        }
    }
    public class Book
    {
        public int id;
        public string title;
        public string author;
        public bool available = true;
        public bool isAvailable()
        {
            return available;
        }

        public Book(int id, string title, string author)
        {
            this.id = id;
            this.title = title;
            this.author = author;
        }
    }

    public class Library
    {
        public List<Book> bookList = new List<Book>(); // Implementation of Belongs To association
        public Dictionary<int, int> studentBookMap = new Dictionary<int, int>(); // or circulationRegister

        // bookList is the library's catalog and studentBookMap contains the map of studentIDs with BookIDs from the catalog

        public Library(List<Book> bookList)
        {
            // Constructor takes a list of books as parameter and creates a template studentBookMap from it.
            this.bookList = bookList;
            for (int i = 0; i < bookList.Count; i++)
            {
                studentBookMap.Add(bookList[i].id, -1);
            }
        }
        public void checkOut(int bookID, int collegeID)
        {
            // checkOut confirms if book is available to be lent, and then maps the bookID to the studentID of the borrower.
            List<Book> tempBookList = new List<Book>();
            foreach (Book book in bookList)
            {
                if (book.id == bookID)
                {
                    book.available = false;
                    studentBookMap[bookID] = collegeID;
                }
                tempBookList.Add(book);
            }
            bookList = tempBookList;
        }

        public void checkIn(int bookID, int collegeID)
        {
            // checkIn confirms if the book being returned was in the library's catalog, matches the IDs and updates
            // the availability state.
            foreach (Book book in bookList)
            {
                if (book.id == bookID)
                {
                    book.available = true;
                    studentBookMap[bookID] = -1;
                }
            }
        }

        public int searchBook(string title)
        {
            // Searches for book title and matches it with bookID.
            int returnID = -1;
            foreach (Book book in bookList)
            {
                if (book.title == title)
                {
                    returnID = book.id;
                    break;
                }
                else
                {
                    returnID = -1;
                }
            }
            return returnID;
        }

        public void updateMap()
        {
            // On adding or removing books from the library after initialization, studentBookMap must be updated to
            // reflect the changes in the studentBookMap.
            studentBookMap.Clear();
            for (int i = 0; i < bookList.Count; i++)
            {
                studentBookMap.Add(bookList[i].id, -1);
            }
        }
    }
    public class Student
    {
        public int rollNo;
        public string name;
        public Library library;

        public Student(string name, int rollno, Library library)
        {
            // Library is associated with student
            // Belongs To association is established between Library and Student
            this.library = library;
            this.name = name;
            this.rollNo = rollno;
        }

        public void getBook(string title)
        {
            // getBook allows student to borrow the selected book based on availability.
            int bookID = library.searchBook(title);
            if (bookID > 0 && library.bookList[bookID].available)
            {
                library.checkOut(bookID, rollNo);
                Console.WriteLine($"\n{this.name} borrowed {title}.");
            }
            else
            {
                Console.WriteLine("Book unavailable.");
            }
        }

        public void returnBook(string title)
        {
            // returnBook allows student to return any books they have borrowed,
            // as long as the library can confirm if the book was borrowed by the specific student.
            int bookID = library.searchBook(title);
            if (bookID > 0 && library.studentBookMap[bookID] == rollNo)
            {
                library.checkIn(bookID, rollNo);
                Console.WriteLine($"\n{this.name} returned {title}.");
            }
            else
            {
                Console.WriteLine($"\n{this.name} attempted to return {title} but {this.name} does not have this book.");
            }
        }
    }
}

