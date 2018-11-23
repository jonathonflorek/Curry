namespace Curry.Test
{
    using System;
    using Xunit;

    public static class Curry_Delegate
    {
        [Fact]
        public static void Delegate2() 
        {
            Curry<Boolean, String, Int32> curry = param1 => param2 => param1 ? param2.Length : -param2.Length;

            var withParam1False = curry(false);
            Assert.IsType<Func<String, Int32>>(withParam1False);
            Assert.Equal(-5, withParam1False("Hello"));

            var withParam1True = curry(true);
            Assert.IsType<Func<String, Int32>>(withParam1True);
            Assert.Equal(5, withParam1True("Hello"));
        }

        [Fact]
        public static void Delegate3()
        {
            Curry<String, Int32, Boolean, Object> curry = param1 => param2 => param3 => param1 + param2.ToString() + param3.ToString();
            var withParam1 = curry("Param 1");
            Assert.IsType<Curry<Int32, Boolean, Object>>(withParam1);
            Assert.Equal($"Param 1{10.ToString()}{false.ToString()}", withParam1(10)(false));
        }

        [Fact]
        public static void Delegate4()
        {
            Curry<String, Int32, Boolean, Double, Object> curry = 
                param1 =>
                param2 =>
                param3 =>
                param4 =>
                new {
                    param1,
                    param2,
                    param3,
                    param4
                }.ToString();
            var withParam1 = curry("Param1");
            Assert.IsType<Curry<Int32, Boolean, Double, Object>>(withParam1);
        }
    }
}