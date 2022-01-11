using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace csharp9and10features
{
    public class CSharp9Tests
    {
        public class InitOnlySetters
        {
            [Test] public void InitOnlySettersExample()
            {
                // Readonly properties that can only be set in the
                // constructor have been available in C# for a while.
                var readonlyObj = new ReadonlyPropertyClass(42);
                Assert.That(readonlyObj.MyProperty, Is.EqualTo(42));

                // Readonly properties can only be set in the constructor,
                // not in an object initializer.
                // var thisDoesNotWork = new ReadonlyPropertyClass { MyProperty = 42 };

                // Init properties allow setting a property in the
                // constructor or in an object initializer.
                var obj = new InitOnlySettersClass { MyProperty = 42 };
                Assert.That(obj.MyProperty, Is.EqualTo(42));

                // Trying to set an init property after initialization time results in a compiler error.
                // obj.MyProperty = 43;

                // init properties can also be set in a class constructor.

                // An init property is not required to be set.
                var obj2 = new InitOnlySettersClass();
                Assert.That(obj2.MyProperty, Is.EqualTo(default(int)));
            }

            class InitOnlySettersClass
            {
                public int MyProperty { get; init; } 
            }

            class ReadonlyPropertyClass
            {
                public int MyProperty { get; }
                public ReadonlyPropertyClass(int value)
                {
                    MyProperty = value;
                }
            }
        }
    
        public class TopLevelStatements
        {
            // See Main.cs for an example
        }
    
        public class MorePatternMatchingStuff
        {
            // When pattern matching was introduced in C# 7, it felt kind of half-baked.
            // It has since gotten many more features.
            // C# 9 adds even more pattern matching features.

            [Test] public void AndOrPatterns()
            {
                Assert.That(IsLetter('a'), Is.True);
                Assert.That(IsLetter('w'), Is.True);
                Assert.That(IsLetter('M'), Is.True);
                Assert.That(IsLetter('.'), Is.False);
                Assert.That(IsLetter('?'), Is.False);

                bool IsLetter(char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';
            }

            [Test] public void NotPatterns()
            {
                object? obj = null;
                // Combining the not pattern with the is pattern
                // finally let's us check for null in a way that
                // reads nicely.
                if (obj is not null)
                {
                    Assert.Fail();
                }

                // This pattern can be used anywhere a pattern
                // is accepted, not just on if statements.
                obj = "some string";
                if (obj is not string)
                {
                    Assert.Fail();
                }
            }
        }
    
        public class MiscStuff
        {
            [Test] public void TargetTypedNew()
            {
                // We used to have to do this when instantiating an object.
                MyType obj = new MyType();
                Assert.That(obj, Is.Not.Null);

                // Or more likely like this in more recent versions of C#.
                var obj2 = new MyType();
                Assert.That(obj2, Is.Not.Null);

                // But now we can instantiate objects like this.
                MyType obj3 = new();
                Assert.That(obj3, Is.Not.Null);

                // This, of course, does not work.
                // var obj4 = new();
            }

            [Test] public void TargetTypingWithParameters()
            {
                // The compiler can figure out the type in other situations as well.
                TestForNull(new());

                void TestForNull(MyType obj) => Assert.That(obj, Is.Not.Null);

                // You can't do this because default values must be compile time
                // constants. new() isn't a compile time constant.
                // void TestForNullWithDefault(MyType obj = new()) => Assert.That(obj, Is.Not.Null);
            }

            [Test] public void TargetTypingWithObjectInitializer()
            {
                // Target typing can also be combined with object initializers.
                MyType obj = new() { IntProperty = 42, StringProperty = "life, the universe, and everything" };

                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.IntProperty, Is.EqualTo(42));
                Assert.That(obj.StringProperty, Is.Not.Empty);
            }

            [Test] public void StaticLambdaExpressionsAndAnonymousMethods()
            {
                // Lambda expressions and anonymous methods can now be marked static.
                // If they are marked static they can't capture local variables or instance variables.
                var list = new List<int> { 1, 2, 3, 4 };
                var someLocallyScopedVariable = 3;

                // A non-static lambda expression can capture local variables.
                var result = list.Where(x => x > someLocallyScopedVariable);
                Assert.That(result, Is.EqualTo(new[] { 4 }));

                // A static lambda expression cannot capture local variables.
                // This statement results in a compiler error
                // var result2 = list.Where(static x => x > someLocallyScopedVariable);

                // A static lambda that does not use local variables is valid.
                var result3 = list.Where(static x => x > 3);
                Assert.That(result3, Is.EqualTo(new[] { 4 }));
            }

            [Test] public void ForEachWithGetEnumeratorAsExtensionMethod()
            {
                TypeWithoutAnEnumerator obj = new();

                // TypeWithoutAnEnumerator does not have a GetEnumerator method, which is usually 
                // needed to work with a foreach loop. But it has a GetEnumerator extension
                // method, which is now also accepted.
                foreach (var item in obj)
                {
                    Assert.That(item, Is.Not.EqualTo(0));
                }
            }

            class MyType
            {
                public int IntProperty { get; set; }
                public string StringProperty { get; set; } = "";
            }
        }

        public class Records
        {
            // Sometimes you want an object that just holds data
            // and doesn't have any behavior. Records are a great
            // option for that (though they can have behavior).
            // They also make it easy to make immutable data structures.
            [Test] public void PositionalParameters()
            {
                PositionalParameterRecord obj = new("foo", 42);
                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.StringProperty, Is.EqualTo("foo"));
                Assert.That(obj.IntProperty, Is.EqualTo(42));

                // Positional parameters are immutable.
                // The following statement results in a compiler error.
                // obj.StringProperty = "bar";

                // Records that use positional parameters also
                // get a default Deconstruct method implemented
                // automatically for the positional parameters.
                (string stringVal, int intVal) = obj;
                Assert.That(stringVal, Is.EqualTo("foo"));
                Assert.That(intVal, Is.EqualTo(42));

                // Using positional parameters generates a constructor,
                // so you can't use an object initializer for positional parameters.
                // The following line results in a compiler error.
                // var obj2 = new PositionalParameterRecord { StringProperty = "bar", IntProperty = 43 };
            }

            [Test] public void RecordsCanHaveMethodsLikeNormalClasses()
            {
                RecordWithMethods obj = new() { IntProperty = 42 };
                Assert.That(obj.OneMore(), Is.EqualTo(43));

                // You can also have a record that uses positional parameters
                // and methods if you want.
                RecordWithMethodsAndPositionalParameters obj2 = new("foo", 42);
                Assert.That(obj2.OneMore(), Is.EqualTo(43));
            }

            [Test] public void NondestructiveMutation()
            {
                // Immutability can result in lots of extra code to
                // copy values when a new modified instance is needed.
                // Records allow using the "with" keyword to make
                // this process easy.

                SimpleRecord initialRecord = new() { StringProperty = "foo", IntProperty = 42 };
                // With a normal immutable class you'd have to do something
                // like this if you wanted to create a new instance with
                // modified values.
                SimpleRecord modifiedRecord = new() { StringProperty = "bar", IntProperty = initialRecord.IntProperty };
                // This is ugly enough with only two properties. The
                // problem only gets worse with more properties.
                Assert.That(modifiedRecord.IntProperty, Is.EqualTo(42));

                // Records allow the use of the "with" keyword to make this easy.
                modifiedRecord = initialRecord with { StringProperty = "bar" };
                Assert.That(modifiedRecord.IntProperty, Is.EqualTo(42));
            }

            [Test] public void ValueEquality()
            {
                // For normal classes, reference equality is used by default
                // when checking to see if two objects are equal. You can override
                // the Equals method and the == operator to change this behavior,
                // but you have to do it yourself.

                SimpleRecord firstRecord = new() { StringProperty = "foo", IntProperty = 42 };
                SimpleRecord equivalentRecord = new() { StringProperty = "foo", IntProperty = 42 };
                SimpleRecord differentRecord = new() { StringProperty = "foo", IntProperty = 43 };

                // Records that have different property values are not equal.
                // This is just like you would expect with a normal class.
                Assert.That(firstRecord, Is.Not.EqualTo(differentRecord));

                // Record values of the same type whose properties all have
                // the same value are considered equal.
                Assert.That(firstRecord, Is.EqualTo(equivalentRecord));

                // But maybe nUnit is doing something "tricky". Let's
                // try this for reals.
                Assert.That(firstRecord.Equals(equivalentRecord), Is.True);

                // Okay. Sure the Equals method works, but that couldn't
                // possibly work for ==, could it? That would be too easy.
                Assert.That(firstRecord == equivalentRecord, Is.True);
                // 😲
                // (ﾉ◕ヮ◕)ﾉ*:･ﾟ✧
                // It's so beautiful!
                // And about freaking time!
            }

            [Test] public void RecordsAreReferenceTypesLikeNormalClasses()
            {
                var obj1 = new SimpleRecord();
                var obj2 = new SimpleRecord();

                Assert.That(obj1.Equals(obj2), Is.True);
                Assert.That(object.ReferenceEquals(obj1, obj2), Is.False);
            }

            [Test] public void NonSuckyDefaultToString()
            {
                NormalClass normalObject = new();
                // The default ToString implementation is wildly useful.
                Assert.That(normalObject.ToString(), Is.EqualTo("csharp9and10features.CSharp9Tests+Records+NormalClass"));
                // Who wouldn't want that?
                // You can override this behavior, but why isn't it more useful by default?

                SimpleRecord recordObject = new() { StringProperty = "foo", IntProperty = 42 };
                Assert.That(recordObject.ToString(), Is.EqualTo("SimpleRecord { StringProperty = foo, IntProperty = 42 }"));
                // 😍
                // ٩(◕‿◕｡)۶
                // You mean I don't have to write my own ToString
                // implementation or some other method to print out
                // all the values of an object when I want to debug
                // something?!?
                // Thank you sweet mother of Abraham Lincoln!

                // What about more complex situations?
                ComplexRecord complexRecord = new()
                {
                    StringProperty = "life, the universe, and everything",
                    SimpleRecordProperty = new()
                    {
                        StringProperty = "foo",
                        IntProperty = 42
                    },
                    StringArrayProperty = new[] { "fizz", "buzz" }
                };

                // For simple situations, the default ToString implementation
                // on records is great. In more complex situations it works,
                // but might not be exactly what you want. For non-record
                // types, like string[], it goes back to printing the
                // type name.
                Assert.That(
                    complexRecord.ToString(),
                    Is.EqualTo(
                    "ComplexRecord { StringProperty = life, the universe, and everything, SimpleRecordProperty = SimpleRecord { StringProperty = foo, IntProperty = 42 }, StringArrayProperty = System.String[] }")
                );
            }

            [Test] public void Inheritance()
            {
                // Record types can inherit from other record types
                // but not from a class. Additionally, a class
                // cannot inherit from a record.
            }

            class NormalClass { }

            record SimpleRecord
            {
                public string StringProperty { get; init; } = "";
                public int IntProperty { get; init; }
            }

            record ComplexRecord
            {
                public string StringProperty { get; init; } = "";
                public SimpleRecord? SimpleRecordProperty { get; init; }
                public string[] StringArrayProperty { get; init; } = new string[0];
            }

            record RecordWithMethods
            {
                public int IntProperty { get; init; }
                public int OneMore()
                {
                    return IntProperty + 1;
                }
            }

            record RecordWithMethodsAndPositionalParameters(string StringProperty, int IntProperty)
            {
                public int OneMore() => IntProperty + 1;
            }

            // At the time of writing, the syntax highlighting for records isn't quite right in Visual Studio Code.
            record PositionalParameterRecord(string StringProperty, int IntProperty);

            // The above record is (mostly) equivalent to the following one.
            record RecordWithInitProperties
            {
                public string StringProperty { get; init; } = "";
                public int IntProperty { get; init; }
            }
        }
    }

    class TypeWithoutAnEnumerator
    {
        public IEnumerable<int> SomethingToIterateOver = new List<int> { 1, 2, 3, 4, 5 };
    }

    static class ExtensionMethods
    {
        public static IEnumerator<int> GetEnumerator(this TypeWithoutAnEnumerator instance)
        {
            return instance.SomethingToIterateOver.GetEnumerator();
        }
    }
}