using Xunit;

namespace IctBaden.Units.Test.PhoneNumbers
{
    public class FormatPhoneNumberTests
    {
        
        [Fact]
        public void FormatInternationalNumberShouldBeWithPlus()
        {
            const string expected = "+4972140989106";
            
            const string number1 = "+4972140989106";
            var phone = new PhoneNumber(number1);
            var text = phone.GetDialString("+EFG");
            Assert.Equal(expected, text);

            const string number2 = "4972140989106";
            phone = new PhoneNumber(number2);
            text = phone.GetDialString("+EFG");
            Assert.Equal(expected, text);

            const string number3 = "072140989106";
            phone = new PhoneNumber(number3);
            text = phone.GetDialString("+EFG");
            Assert.Equal(expected, text);
            
        }
        
        [Fact]
        public void FormatMobileShouldBeWithPlus()
        {
            const string expected = "+491727207196";
            
            const string number1 = "+491727207196";
            var phone = new PhoneNumber(number1);
            var text = phone.GetDialString("+EFG");
            Assert.Equal(expected, text);

            const string number2 = "491727207196";
            phone = new PhoneNumber(number2);
            text = phone.GetDialString("+EFG");
            Assert.Equal(expected, text);

            const string number3 = "01727207196";
            phone = new PhoneNumber(number3);
            text = phone.GetDialString("+EFG");
            Assert.Equal(expected, text);
            
        }

    }
}