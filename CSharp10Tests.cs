namespace csharp9and10features;

public class Csharp10Tests
{
    [Test] public void GlobalUsings()
    {
        // You can add the "global" keyword to a using statement.
        // It will then apply to every source file in the
        // compilation (usually everything in the project).

        // See globalUsings.cs for an example.

        // <insert exclamation of excitement here>
    }

    [Test] public void FileScopedNamespaceDeclaration()
    {
        // You can now use a single line namespace declaration
        // for an entire file. This form omits the mostly
        // superfluous indentation of everything else in
        // virtually every .cs file.

        // 🙌🎉🤩

        // (ﾉ◕ヮ◕)ﾉ*:･ﾟ✧

        // (☞ﾟヮﾟ)☞
    }

    [Test] public void RecordStructTypes()
    {
        // You can now get all the new goodness of records.
        // But with more struct-ness.
        var obj1 = new RecordStruct("foo", 42);
        var obj2 = new RecordStruct("foo", 42);

        Assert.That(obj1, Is.EqualTo(obj2));
    }

    [Test] public void ExtendedPropertyPatterns()
    {
        // More minor improvements to pattern matching!

        Person person1 = new(42, new() { FirstName = "Fred", LastName = "Weasley" });
        Person person2 = new(78, new() { FirstName = "Wilbur", LastName = "Weasley" });

        Assert.That(IsFamousWeasleyWithoutExtendedPropertyPatterns(person1), Is.True);
        Assert.That(IsFamousWeasley(person1), Is.True);
        Assert.That(IsFamousWeasley(person2), Is.False);

        // You could do this in C# 8 and later.
        bool IsFamousWeasleyWithoutExtendedPropertyPatterns(Person person)
        {
            if (person.Name.LastName != "Weasley") return false;
            return person switch
            {
                { Name: { FirstName: "Fred" } } => true,
                { Name: { FirstName: "George" } } => true,
                { Name: { FirstName: "Ron" } } => true,
                _ => false
            };
        }

        // C# 10 adds this new, slightly easier syntax.
        bool IsFamousWeasley(Person person)
        {
            if (person.Name.LastName != "Weasley") return false;
            return person switch
            {
                { Name.FirstName: "Fred" } => true,
                { Name.FirstName: "George" } => true,
                { Name.FirstName: "Ron" } => true,
                _ => false
            };
        }
    }

    [Test] public void ConstantInterpolatedStrings()
    {
        const string constant = "compile time constant";
        const string MyConstString = $"Everything in this string has to be a {constant}";

        // Everything in a constant interpolated string has to be a constant string.

        Assert.That(MyConstString, Is.EqualTo("Everything in this string has to be a compile time constant"));

        // You can't use numeric values in constant interpolated strings
        // because the numeric value will get turned into a string
        // at runtime instead of at compile time.
        // const int intValue = 42;
        // const string AnotherConstString = $"{intValue}";
    }

    [Test] public void RecordTypesCanSealToString()
    {
        // Yay?
    }

    [Test] public void AssignmentsAndDeclarationsWithDeconstrutor()
    {
        // You automatically get a deconstructor with value tuples
        // and with records that have positional parameters.
        // You can also implement them on your own classes if you want.
        var person = new Person(42, new() { FirstName = "Tony", LastName = "Stark" });

        // Before C# 10 you could this:
        int id1;
        Name name1;
        (id1, name1) = person;
        Assert.That(id1, Is.EqualTo(42));
        Assert.That(name1.FirstName, Is.EqualTo("Tony"));

        // Or with a value tuple
        int val1;
        string val2;
        var tuple = (42, "foo");
        (val1, val2) = tuple;
        Assert.That(val1, Is.EqualTo(42));
        Assert.That(val2, Is.EqualTo("foo"));

        // Or this:
        (int id2, Name name2) = person;
        Assert.That(id2, Is.EqualTo(42));
        Assert.That(name2.FirstName, Is.EqualTo("Tony"));

        // But in C# 10 you can do this. You know, if you want to.
        int id3;
        (id3, Name name3) = person;
        Assert.That(id3, Is.EqualTo(42));
        Assert.That(name1.FirstName, Is.EqualTo("Tony"));
    }

    record Person(int Id, Name Name);

    record Name
    {
        public string FirstName { get; init; } = "";
        public string LastName { get; init; } = "";
    }

    record struct RecordStruct(string StringProperty, int IntProperty);
}
