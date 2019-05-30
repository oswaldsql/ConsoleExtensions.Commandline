namespace ConsoleExtensions.Commandline.Tests
{
    using System;

    public class UnitTest1
    {
        // [Fact]
        // public void Test1()
        // {
        // var model = new StringBuilder("https://github.com/oswaldsql/ConsoleExtensions.Templating");

        // var modelMap = new ModelMap(model);

        // foreach (var flag in modelMap.Flags)
        // {
        // var value = flag.Value;
        // Console.WriteLine($"-{value.Name} = {value.CurrentValue()}");
        // }

        // modelMap.Flags["Capacity"].Set("100");

        // foreach (var flag in modelMap.Flags)
        // {
        // var value = flag.Value;
        // Console.WriteLine($"-{value.Name} = {value.CurrentValue()}");
        // }
        // }

        // [Fact]
        // public void Givengiven_Whenwhen_Thenthen()
        // {
        // var model = new demo();

        // var modelMap = new ModelMap(model);

        // foreach (var flag in modelMap.Actions)
        // {
        // var value = flag.Value;

        // var keys = string.Join(" | ", value.ShortcutKeys.Select(t => t.KeyChar.ToString()));
        // Console.WriteLine($"[{keys}] {value.DisplayName}");
        // }
        // }

        // [Fact]
        // public void SetPropertiesTest()
        // {
        // // Arrange
        // CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        // var actual = new SetPropertyTestObject();
        // var map = new ModelMap(actual);

        // // Act
        // map["StringValue"] = "test";
        // map["IntValue"] = "1234";
        // map["DateTimeValue"] = "2019-03-02";
        // map["DecimalValue"] = "123.456";
        // map["EnumValue"] = "Monday";
        // map["BoolValue"] = "true";

        // // Assert
        // Assert.Equal("test", actual.StringValue);
        // Assert.Equal(1234, actual.IntValue);
        // Assert.Equal(new DateTime(2019, 03, 02), actual.DateTimeValue);
        // Assert.Equal((decimal)123.456, actual.DecimalValue);
        // Assert.Equal(DayOfWeek.Monday, actual.EnumValue);
        // Assert.True(actual.BoolValue);
        // }

        // [Fact]
        // public void SetFlagsThroughCommandLine()
        // {
        // // Arrange
        // CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        // var sut = new CallActionTestObject();
        // var map = new ModelMap(sut);

        // // Act
        // var actual = map.Invoke("ReturnSomeValue", "Monday");

        // // Assert
        // Assert.IsType<string>(actual);
        // Assert.Equal("yadnoM", actual);
        // }

        // public class CallActionTestObject
        // {
        // public string ReturnSomeValue(DayOfWeek input1)
        // {
        // return new string(input1.ToString().Reverse().ToArray());
        // }
        // }

        // public class SetPropertyTestObject
        // {
        // public string StringValue { get; set; }

        // public int IntValue { get; set; }

        // public Decimal DecimalValue { get; set; }

        // public DateTime DateTimeValue { get; set; }

        // public DayOfWeek EnumValue { get; set; }

        // public bool BoolValue { get; set; }
        // }

        // public class demo
        // {
        // [ShortcutKey('1'), ShortcutKey('2')]
        // public string TestToSeHowThisIsSplit()
        // {
        // return "";
        // }
        // }
    }
}