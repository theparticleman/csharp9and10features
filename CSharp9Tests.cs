using NUnit.Framework;

namespace csharp9and10features
{
    public class CSharp9Tests
    {
        public class InitOnlySetters
        {
            [Test] public void InitOnlySettersExample()
            {
                var obj = new InitOnlySettersClass
                {
                    MyProperty = 42
                };

                Assert.That(obj.MyProperty, Is.EqualTo(42));

                // This results in a compiler error.
                // obj.MyProperty = 43;
            }

            class InitOnlySettersClass
            {
                public int MyProperty { get; init; } 
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
                object obj = null;
                if (obj is not null)
                {
                    Assert.Fail();
                }

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
                // Or more likely like this.
                // var obj2 = new MyType();

                // But now we can instantiate objects like this.
                MyType obj3 = new();

                Assert.That(obj3, Is.Not.Null);
            }

            [Test] public void 

            class MyType { }
        }
    }
}