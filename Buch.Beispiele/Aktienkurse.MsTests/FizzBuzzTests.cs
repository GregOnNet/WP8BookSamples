using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Aktienkurse.MsTests
{
  [TestClass]
  public class FizzBuzzTests
  {
    [TestMethod]
    public void Processing_a_number_divisible_by_3_it_should_return_Fizz()
    {
      int input = 6;
      var fizzbuzz = new FizzBuzz();

      var result = FizzBuzz.Process(input);

      Assert.AreEqual("Fizz", result);
    }
  }

  public class FizzBuzz
  {
    public static string Process(int input)
    {
      return "Fizz";
    }
  }
}
