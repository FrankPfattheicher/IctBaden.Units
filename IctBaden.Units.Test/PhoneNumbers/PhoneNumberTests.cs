using Xunit;

namespace IctBaden.Units.Test.PhoneNumbers;

public class PhoneNumberTests
{
    [Theory]
    //                    Location; Text; Country; Area; Number; Extension; DialInternal; IsValid
    [InlineData("49", "0044 20-7615 600", "44", "20", "7615600", "", false, true)]
    [InlineData("49", "0202 / 59 00-01", "49", "202", "5900", "01", false, true)]
    [InlineData("", "0800 / 95 95 95 5", "", "0800", "9595955", "", false, true)]
    [InlineData("", "0 18 05 / 00 43 21", "", "01805", "004321", "", false, true)]
    [InlineData("", "0 18 0 / 5 00 43 21", "", "0180", "5004321", "", false, true)]
    [InlineData("49", "0 18 05 / 00 43 21", "49", "1805", "004321", "", false, true)]
    [InlineData("", "+49 (0)89/555 123-160", "49", "89", "555123", "160", false, true)]
    [InlineData("", "0160 – 83 88 251", "", "0160", "8388251", "", false, true)]
    [InlineData("49", "0160 – 83 88 251", "49", "160", "8388251", "", false, true)]
    [InlineData("", "+49 721 408069", "49", "721", "408069", "", false, true)]
    [InlineData("", "+49721408069", "49", "721", "408069", "", false, true)]
    [InlineData("", "0721408069", "", "", "0721408069", "", false, true)]
    [InlineData("49", "0721408069", "49", "721", "408069", "", false, true)]
    [InlineData("49", "408069", "", "", "408069", "", false, true)]
    [InlineData("49;721", "408069", "49", "721", "408069", "", false, true)]
    [InlineData("", "1234567", "", "", "1234567", "", false, true)]
    [InlineData("", "+49 30 12345-67", "49", "30", "12345", "67", false, true)]
    [InlineData("", "+49 30 123-45-67", "49", "30", "1234567", "", false, true)]
    [InlineData("", "+49 (30) 1234567", "49", "30", "1234567", "", false, true)]
    [InlineData("", "+49-30-1234567", "49", "30", "1234567", "", false, true)]
    [InlineData("", "tel:+49-30-1234567", "49", "30", "1234567", "", false, false)]
    [InlineData("", "+49 (0)30 12345 67", "49", "30", "1234567", "", false, true)]
    [InlineData("", "030 12345-67", "", "030", "12345", "67", false, true)]
    [InlineData("", "(0 30) 1 23 45-67", "", "030", "12345", "67", false, true)]
    [InlineData("", "(030) 123 45 67", "", "030", "1234567", "", false, true)]
    [InlineData("", "hoppla", "", "", "", "", false, false)]
    [InlineData("", "-15", "", "", "", "15", true, true)]

    public void PhoneNumberParse(string locationEx, string text, string country, string area, string number, string extension, bool dialInternal, bool valid)
    {
        var location = (locationEx + ";;").Split(';');

        var check = new PhoneNumber(text).IsValid;
        Assert.True(valid == check, "Validation test failed for number " + text);
        if (!check)
            return;

        var parsed = PhoneNumber.Parse(text, new PhoneNumber(location[0], location[1], "", "", false));
        Assert.True(country == parsed.CountryCode, "Country code not detected in " + text);
        Assert.True(area == parsed.AreaCode, "Area code not detected in " + text);
        Assert.True(number == parsed.Number, "Number not detected in " + text);
        Assert.True(extension == parsed.Extension, "Extension not detected in " + text);
        Assert.True(dialInternal == parsed.DialInternal, "DialInternal not detected in " + text);
    }

    [Fact]
    public void PhoneNumberToString()
    {
        var check = new PhoneNumber("49", "721", "408069", "00", false);
        Assert.True("+4972140806900" == check.ToString(), "Unexpected default format");
        Assert.True("+49 721 408069-00" == check.ToString(PhoneNumberFormat.DIN_5008), "Unexpected DIN 5008 format");
        Assert.True("+49 721 40806900" == check.ToString(PhoneNumberFormat.E_123), "Unexpected E123 format");
        Assert.True("+49 (721) 40806900" == check.ToString(PhoneNumberFormat.Microsoft), "Unexpected Microsoft format");
        Assert.True("+49-721-40806900" == check.ToString(PhoneNumberFormat.URI), "Unexpected URI format");

        check = new PhoneNumber("", "", "", "10", true);
        Assert.True("-10" == check.ToString(PhoneNumberFormat.DIN_5008), "Unexpected default format");
    }
}