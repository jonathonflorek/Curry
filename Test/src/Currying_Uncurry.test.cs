namespace Curry.Test 
{
    using System;
    using Xunit;

    public static class Currying_Uncurry
    {
        [Fact]
        public static void Uncurry5() 
        {
            Curry<String, String, String, String, String, String> curry = 
                param1 =>
                param2 =>
                param3 =>
                param4 =>
                param5 =>
                String.Join(", ", param1, param2, param3, param4, param5);
            
            var uncurried = curry.Uncurry();
            Assert.Equal(curry("a")("b")("c")("d")("e"), uncurried("a", "b", "c", "d", "e"));
        }
    }
}