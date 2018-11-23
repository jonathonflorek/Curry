namespace Curry.Test
{
    using System;
    using Xunit;

    public static class Currying_Curry
    {
        [Fact]
        public static void Curry5()
        {
            Func<String, String, String, String, String, String> func = 
                (param1, param2, param3, param4, param5) =>
                String.Join(", ", param1, param2, param3, param4, param5);
            
            var curried = func.Curry();
            Assert.Equal(func("a", "b", "c", "d", "e"), curried("a")("b")("c")("d")("e"));
        }
    }
}