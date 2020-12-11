using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
// ReSharper disable MemberCanBePrivate.Global

namespace IctBaden.Units
{
    public class PhoneNumber
    {
        // http://de.wikipedia.org/wiki/Telefonnummer

        private string _countryCode;
        private string _countryName;
        private string _areaCode;
        private string _areaName;
        private string _number;
        private string _extension;
        private bool _dialInternal;

        public string CountryCode
        {
            get => _countryCode;
            set => _countryCode = value;
        }
        public string CountryName
        {
            get => _countryName;
            set => _countryName = value;
        }
        public string AreaCode
        {
            get => _areaCode;
            set => _areaCode = value;
        }
        public string AreaName
        {
            get => _areaName;
            set => _areaName = value;
        }
        public string Number
        {
            get => _number;
            set => _number = value;
        }
        public string Extension
        {
            get => _extension;
            set => _extension = value;
        }
        public bool DialInternal
        {
            get => _dialInternal;
            set => _dialInternal = value;
        }

        public PhoneNumber()
        {
            _countryCode = string.Empty;
            _countryName = string.Empty;
            _areaCode = string.Empty;
            _areaName = string.Empty;
            _number = string.Empty;
            _extension = string.Empty;
            _dialInternal = false;
        }
        public PhoneNumber(string text)
        {
            if (!IsValid(text))
                return;

            var init = Parse(text);
            _countryCode = init.CountryCode;
            _areaCode = init.AreaCode;
            _number = init.Number;
            _extension = init.Extension;
            _dialInternal = init.DialInternal;
        }
        public PhoneNumber(string countryCode, string areaCode, string number, string extension, bool dialInternal)
        {
            _countryCode = countryCode;
            _countryName = string.Empty;
            _areaCode = areaCode;
            _areaName = string.Empty;
            _number = number;
            _extension = extension;
            _dialInternal = dialInternal;
        }
        public void Clear()
        {
            _countryCode = string.Empty;
            _countryName = string.Empty;
            _areaCode = string.Empty;
            _areaName = string.Empty;
            _number = string.Empty;
            _extension = string.Empty;
            _dialInternal = false;
        }

        public DialTypes DialType
        {
            get
            {
                if (DialInternal)
                    return DialTypes.Internal;
                if (string.IsNullOrEmpty(AreaCode))
                    return DialTypes.Local;
                return string.IsNullOrEmpty(CountryCode) ? DialTypes.LongDistance : DialTypes.International;
            }
        }

        public bool IsEmpty => string.IsNullOrEmpty(_areaCode) &&
                               string.IsNullOrEmpty(_number) &&
                               string.IsNullOrEmpty(_extension);

        public static string ValidateSeparators(string text)
        {
            // Common dashes - replace with ascii minus
            text = text.Replace("\x2012", "-");  // figure dash
            text = text.Replace("\x2013", "-");  // en dash
            text = text.Replace("\x2014", "-");  // em dash
            text = text.Replace("\x2015", "-");  // horizontal bar

            // Unicode spaces - replace with ascii spaces
            text = text.Replace("\x00A0", " ");
            text = text.Replace("\x1680", " ");
            text = text.Replace("\x180E", " ");
            text = text.Replace("\x2000", " ");
            text = text.Replace("\x2001", " ");
            text = text.Replace("\x2002", " ");
            text = text.Replace("\x2003", " ");
            text = text.Replace("\x2004", " ");
            text = text.Replace("\x2005", " ");
            text = text.Replace("\x2006", " ");
            text = text.Replace("\x2007", " ");
            text = text.Replace("\x2008", " ");
            text = text.Replace("\x2009", " ");
            text = text.Replace("\x200A", " ");
            text = text.Replace("\x202F", " ");
            text = text.Replace("\x205F", " ");
            text = text.Replace("\x3000", " ");
            text = text.Trim();

            return text;
        }

        public bool Valid
        {
            get
            {
                if (string.IsNullOrEmpty(Number) && string.IsNullOrEmpty(Extension))
                    return false;
                if (string.IsNullOrEmpty(AreaCode) && !string.IsNullOrEmpty(CountryCode))
                    return false;
                return true;
            }
        }
        public static bool IsValid(string number)
        {
            return Regex.IsMatch(ValidateSeparators(number), @"^(\+[0-9]+)?[0-9 -/\(\)]+$");
        }

        public static string Trim(string text)
        {
            return text.Trim(' ', '-', '/');
        }

        public static PhoneNumber CurrentCultureLocation
        {
            get
            {
                var name = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
                var code = InternationalNumberingPlan.GetCountryCodeByName(name);
                return new PhoneNumber(code, "", "", "", false) { CountryName = name };
            }
        }

        public static PhoneNumber Parse(string text)
        {
            return Parse(text, CurrentCultureLocation);
        }

        public static PhoneNumber Parse(string text, PhoneNumber defaultLocation)
        {
            var parsedNumber = new PhoneNumber();
            text = ValidateSeparators(text);

            var localParser = NumberingPlanFactory.GetNumberingPlan(InternationalNumberingPlan.GetNameByCountryCode(defaultLocation.CountryCode));

            localParser?.ResolveInternationalDialling(ref parsedNumber, ref text);

            if (InternationalNumberingPlan.Parse(ref parsedNumber, ref text))
            {
                localParser = NumberingPlanFactory.GetNumberingPlan(parsedNumber.CountryName);
            }

            if (string.IsNullOrEmpty(parsedNumber.AreaCode))
            {
                localParser?.Parse(ref parsedNumber, ref text);
            }

            // area code by extracting numbers in braces
            if (string.IsNullOrEmpty(parsedNumber.AreaCode))
            {
                var foundAreaCode = new Regex(@"^\(([0-9 ]+)\)").Match(text);
                if (foundAreaCode.Success)
                {
                    if (foundAreaCode.Groups[1].Value == "0")
                    {
                        text = text.Replace("(0)", "").Trim();
                    }
                    else
                    {
                        var explicitAreaCode = foundAreaCode.Groups[1].Value;
                        explicitAreaCode = explicitAreaCode.Replace(" ", "");
                        text = text.Substring(foundAreaCode.Groups[0].Value.Length);
                        parsedNumber.AreaCode = explicitAreaCode;

                        localParser?.Parse(ref parsedNumber, ref explicitAreaCode);
                    }
                    text = Trim(text);
                }
            }

            var findExtDelimiter = new Regex(".*([^0-9])[0-9]+$").Match(text);
            var extDelimiter = '-';
            if (findExtDelimiter.Success)
            {
                extDelimiter = findExtDelimiter.Groups[1].Value[0];
            }
            // count all delimiters
            var delimiters = new Dictionary<char, int>();

            foreach (var ch in text)
            {
                if (char.IsDigit(ch))
                    continue;

                if (delimiters.ContainsKey(ch))
                {
                    delimiters[ch]++;
                }
                else
                {
                    delimiters.Add(ch, 1);
                }
            }

            if ((extDelimiter != ' ') && delimiters.ContainsKey(extDelimiter) && (delimiters[extDelimiter] == 1))
            {
                var extDelimiterPos = text.LastIndexOf(extDelimiter);
                parsedNumber._extension = text.Substring(extDelimiterPos + 1);
                text = text.Substring(0, extDelimiterPos);
                text = Trim(text);
            }

            if (string.IsNullOrEmpty(parsedNumber.AreaCode))
            {
                foreach (var pair in delimiters)
                {
                    if (pair.Value != 1)
                        continue;

                    var areaDelimiter = text.IndexOf(pair.Key);
                    if (areaDelimiter < 0)
                        continue;

                    parsedNumber.AreaCode = text.Substring(0, areaDelimiter);
                    if (localParser != null)
                    {
                        var code = parsedNumber.AreaCode;
                        localParser.Parse(ref parsedNumber, ref code);
                    }
                    else
                    {
                        foreach (var pair2 in delimiters)
                        {
                            parsedNumber.AreaCode = parsedNumber.AreaCode.Replace(pair2.Key.ToString(), "");
                        }
                    }
                    text = text.Substring(areaDelimiter + 1);
                    text = Trim(text);
                    break;
                }
            }

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var pair in delimiters)
            {
                text = text.Replace(pair.Key.ToString(), "");
            }
            parsedNumber._number = text;
            parsedNumber._dialInternal = string.IsNullOrEmpty(parsedNumber._countryCode) && string.IsNullOrEmpty(parsedNumber._areaCode) && string.IsNullOrEmpty(parsedNumber._number) && !string.IsNullOrEmpty(parsedNumber._extension);

            if (!parsedNumber._dialInternal)
            {
                if (string.IsNullOrEmpty(parsedNumber.AreaCode) && !string.IsNullOrEmpty(defaultLocation.AreaCode))
                {
                    parsedNumber.AreaCode = defaultLocation.AreaCode;
                    parsedNumber.AreaName = defaultLocation.AreaName;
                }
                if (string.IsNullOrEmpty(parsedNumber.CountryCode) && !string.IsNullOrEmpty(parsedNumber.AreaCode))
                {
                    parsedNumber.CountryCode = defaultLocation.CountryCode;
                    parsedNumber.CountryName = defaultLocation.CountryName;
                }
            }
            return parsedNumber;
        }

        public string GetDialString(string lineAccess, string dialingRule)
        {
            var dialString = string.Empty;
            if (!_dialInternal)
                dialString = lineAccess;
            dialString += dialingRule;
            dialString = dialString.Replace("E", _countryCode);
            dialString = dialString.Replace("F", _areaCode);
            dialString = dialString.Replace("G", _number + _extension);
            return dialString;
        }

        // ReSharper disable InconsistentNaming
        public enum PhoneNumberFormat
        {
            Default,
            DIN_5008,
            E_123,
            Microsoft,
            URI
        }
        // ReSharper restore InconsistentNaming

        public override string ToString()
        {
            return ToString(PhoneNumberFormat.Default);
        }
        public string ToString(PhoneNumberFormat format)
        {
            // TODO: implement and use IPhoneNumberFormatter
            var formatedString = string.IsNullOrEmpty(_countryCode) ? string.Empty : "+";

            switch (format)
            {
                case PhoneNumberFormat.Default:
                    if (string.IsNullOrEmpty(_number) && string.IsNullOrEmpty(_extension))
                        return string.Empty;
                    formatedString += _countryCode + _areaCode + _number + _extension;
                    break;
                case PhoneNumberFormat.DIN_5008:
                    formatedString += _countryCode;
                    if (!string.IsNullOrEmpty(_countryCode) && !string.IsNullOrEmpty(_areaCode))
                        formatedString += " ";
                    formatedString += _areaCode;
                    if (!string.IsNullOrEmpty(_areaCode) && !string.IsNullOrEmpty(_number))
                        formatedString += " ";
                    formatedString += _number + "-" + _extension;
                    break;
                case PhoneNumberFormat.E_123:
                    formatedString += _countryCode;
                    if (!string.IsNullOrEmpty(_countryCode) && !string.IsNullOrEmpty(_areaCode))
                        formatedString += " ";
                    formatedString += _areaCode;
                    if (!string.IsNullOrEmpty(_areaCode) && !string.IsNullOrEmpty(_number))
                        formatedString += " ";
                    formatedString += _number + _extension;
                    break;
                case PhoneNumberFormat.Microsoft:
                    formatedString += _countryCode;
                    if (!string.IsNullOrEmpty(_countryCode) && !string.IsNullOrEmpty(_areaCode))
                        formatedString += " ";
                    if (!string.IsNullOrEmpty(_areaCode))
                        formatedString += "(" + _areaCode + ")";
                    if (!string.IsNullOrEmpty(_areaCode) && !string.IsNullOrEmpty(_number))
                        formatedString += " ";
                    formatedString += _number + _extension;
                    break;
                case PhoneNumberFormat.URI:
                    formatedString += _countryCode;
                    if (!string.IsNullOrEmpty(_countryCode) && !string.IsNullOrEmpty(_areaCode))
                        formatedString += "-";
                    formatedString += _areaCode;
                    if (!string.IsNullOrEmpty(_areaCode) && !string.IsNullOrEmpty(_number))
                        formatedString += "-";
                    formatedString += _number + _extension;
                    break;
                default:
                    Debug.Print("Invalid PhoneNumberFormat");
                    break;
            }
            return formatedString;
        }

    }
}
