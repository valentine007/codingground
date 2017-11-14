Паттерн Fluent Builder позволяет упростить процесс создания сложных объектов с помощью методов-цепочек, которые наделяют объект каким-то определенным качеством. Применение данного паттерна делает процесс конструирования объектов более прозрачным, а код более читабельным.

Исторически fluent builder прежде всего позволял решить проблему перегруженных конструкторов.

Возьмем следующий класс User:
	
public class User
{
    public string Name { get; set; }        // имя
    public string Company { get; set; }     // компания
    public int Age { get; set; }            // возраст
    public bool IsMarried { get; set; }      // женат/замужем
 
    public User(string name, string company, int age, bool isMarried)
    {
        Name = name;
        Company = company;
        Age = age > 0 ? age : 18;
        IsMarried = isMarried;
    }
}

С какими проблемами мы можем столкнуться? Если необходимо иницилизировать множества свойств объекта, то конструктор может принимать много параметров. В данном случае для упрощения примера используются только четыре параметра и свойства, но соответственно их может быть гораздо больше. Такой код сложнее поддерживать. Мы зависим от параметров и действий, которые с этими параметрами производятся в конструкторе. Кроме того, конструктор может содержать дополнительную логику по проверке и установке значений свойств и переменных. Если надо выполнить много подобных действий, то код конструктора раздувается.

В качестве одного из решений данной проблемы можно использовать паттерн Fluent Builder. Для этого определим следующий класс UserBuilder:
	
public class UserBuilder
{
    private User user;
    public UserBuilder()
    {
        user = new User();
    }
    public UserBuilder SetName(string name)
    {
        user.Name = name;
        return this;
    }
    public UserBuilder SetCompany(string company)
    {
        user.Company = company;
        return this;
    }
    public UserBuilder SetAge(int age)
    {
        user.Age = age > 0 ? age : 0;
        return this;
    }
    public UserBuilder IsMarried
    {
        get
        { 
            user.IsMarried = true; 
            return this; 
        }
    }
    public User Build()
    {
        return user;
    }
}

С помощью ряда методов класс инициализирует различные свойства объекта User. Причем каждый подобный метод возвращает текущий объект с помощью вызова return this. При этом для этой цели можно использовать не только методы, но и свойства.

Для возвращения созданного объекта обычно определяется метод под названием Build.

Также изменим класс User:
	
public class User
{
    public string Name { get; set; }        // имя
    public string Company { get; set; }     // компания
    public int Age { get; set; }            // возраст
    public bool IsMarried { get; set; }      // женат/замужем
         
    public static UserBuilder CreateBuilder()
    {
        return new UserBuilder();
    }
}

Класс больше не имеет никаких конструкторов, кроме конструктора по умолчанию, и также определяет статический метод, который возвращает объект UserBuilder.

Используем данный класс:
1
2
	
User tom = new UserBuilder().SetName("Tom").SetCompany("Microsoft").SetAge(23).Build();
User alice = User.CreateBuilder().SetName("Alice").IsMarried.SetAge(25).Build();

В первом случае напрямую используется объект UserBuilder, а во втором - статический метод User.CreateBuilder. Такой подход является более интуитивно понятным в плане того, что и для какого свойства объекта устанавливается.

C# позволяет определять перегрузку операций преобразования, поэтому вместо возвращения объекта через метод Build определим операцию преобразования:

public class UserBuilder
{
    private User user;
    public UserBuilder()
    {
        user = new User();
    }
    public UserBuilder SetName(string name)
    {
        user.Name = name;
        return this;
    }
    public UserBuilder SetCompany(string company)
    {
        user.Company = company;
        return this;
    }
    public UserBuilder SetAge(int age)
    {
        user.Age = age > 0 ? age : 0;
        return this;
    }
    public UserBuilder IsMarried
    {
        get
        { 
            user.IsMarried = true; 
            return this; 
        }
    }
    public static implicit operator User(UserBuilder builder)
    {
        return builder.user;
    }
}

Теперь метод Build можно не использовать:
1
2
	
User tom = new UserBuilder().SetName("Tom").SetCompany("Microsoft").SetAge(23);
User alice = User.CreateBuilder().SetName("Alice").IsMarried.SetAge(25);
