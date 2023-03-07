using System;
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

        public string? CountryCode { get; internal set; }
        public string? CountryName { get; internal set; }
        public string? AreaCode { get; internal set; }
        public string? AreaName { get; internal set; }
        public string? Number { get; private set; }
        public string? Extension { get; private set; }
        public bool DialInternal { get; private set; }

        public PhoneNumber()
        {
            CountryCode = string.Empty;
            CountryName = string.Empty;
            AreaCode = string.Empty;
            AreaName = string.Empty;
            Number = string.Empty;
            Extension = string.Empty;
            DialInternal = false;
        }

        public PhoneNumber(string text)
        {
            if (!IsValidFormat(text))
                return;

            var init = Parse(text);
            CountryCode = init.CountryCode;
            AreaCode = init.AreaCode;
            Number = init.Number;
            Extension = init.Extension;
            DialInternal = init.DialInternal;
        }

        public PhoneNumber(string? countryCode, string? areaCode, string? number, string? extension, bool dialInternal)
        {
            CountryCode = countryCode;
            CountryName = string.Empty;
            AreaCode = areaCode;
            AreaName = string.Empty;
            Number = number;
            Extension = extension;
            DialInternal = dialInternal;
        }

        public void Clear()
        {
            CountryCode = string.Empty;
            CountryName = string.Empty;
            AreaCode = string.Empty;
            AreaName = string.Empty;
            Number = string.Empty;
            Extension = string.Empty;
            DialInternal = false;
        }

        public static PhoneNumber TryParse(string text) => TryParse(text, CurrentCultureLocation);

        public static PhoneNumber TryParse(string text, PhoneNumber location)
        {
            var countryCode = location.CountryCode;
            var localParser = NumberingPlanFactory.GetNumberingPlan(location.CountryName);
            if (localParser == null)
            {
                throw new NotSupportedException($"Numbering plan for {location.CountryName} not supported.");
            }

            text = localParser.ResolveInternationalDialling(text);

            // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
            foreach (var numberingPlanEntry in localParser.CodeList)
            {
                var areaCode = numberingPlanEntry.Code;
                var pattern = new Regex($@"^+?{countryCode}{areaCode}([0-9]+)$");
                var match = pattern.Match(text);
                if (match.Success)
                {
                    return new PhoneNumber(countryCode, areaCode, match.Groups[1].Value, "", false);
                }

                pattern = new Regex($@"^{areaCode}([0-9]+)$");
                match = pattern.Match(text);
                if (match.Success)
                {
                    return new PhoneNumber(countryCode, areaCode, match.Groups[1].Value, "", false);
                }
            }

            return Parse(text, location);
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

        public bool IsEmpty => string.IsNullOrEmpty(AreaCode) &&
                               string.IsNullOrEmpty(Number) &&
                               string.IsNullOrEmpty(Extension);

        public static string ValidateSeparators(string text)
        {
            // Common dashes - replace with ascii minus
            text = text.Replace("\x2012", "-"); // figure dash
            text = text.Replace("\x2013", "-"); // en dash
            text = text.Replace("\x2014", "-"); // em dash
            text = text.Replace("\x2015", "-"); // horizontal bar

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

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(Number) && string.IsNullOrEmpty(Extension))
                    return false;

                var localParser = NumberingPlanFactory
                    .GetNumberingPlan(InternationalNumberingPlan.GetNameByCountryCode(CountryName));

                if (localParser == null)
                {
                    // no more checks possible
                    return true;
                }
                
                return !string.IsNullOrEmpty(AreaCode) || string.IsNullOrEmpty(CountryCode);
            }
        }

        public static bool IsValidFormat(string number)
        {
            return Regex.IsMatch(ValidateSeparators(number), @"^(\+[0-9]+)?[0-9 -/\(\)]+$");
        }

        public static string Trim(string text)
        {
            return text.Trim(' ', '-', '/');
        }

        public static PhoneNumber GetCultureLocation(string twoLetterISOLanguageName)
        {
            var code = InternationalNumberingPlan.GetCountryCodeByName(twoLetterISOLanguageName);
            return new PhoneNumber(code, "", "", "", false) {CountryName = twoLetterISOLanguageName};
        }
        
        public static PhoneNumber CurrentCultureLocation
        {
            get
            {
                var name = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToUpper();
                var code = InternationalNumberingPlan.GetCountryCodeByName(name);
                return new PhoneNumber(code, "", "", "", false) {CountryName = name};
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

            var localParser = NumberingPlanFactory
                .GetNumberingPlan(InternationalNumberingPlan
                    .GetNameByCountryCode(defaultLocation.CountryCode));

            if (localParser != null)
            {
                text = localParser.ResolveInternationalDialling(text);
            }

            var intl = InternationalNumberingPlan.Parse(ref text);
            if (intl != null)
            {
                parsedNumber.CountryCode = intl.CountryCode;
                parsedNumber.CountryName = intl.CountryName;
                localParser = NumberingPlanFactory.GetNumberingPlan(intl.CountryName);
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
                parsedNumber.Extension = text.Substring(extDelimiterPos + 1);
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

            parsedNumber.Number = text;
            parsedNumber.DialInternal = string.IsNullOrEmpty(parsedNumber.CountryCode) &&
                                        string.IsNullOrEmpty(parsedNumber.AreaCode) &&
                                        string.IsNullOrEmpty(parsedNumber.Number) &&
                                        !string.IsNullOrEmpty(parsedNumber.Extension);

            if (!parsedNumber.DialInternal)
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

        public string GetDialString(string dialingRule)
        {
            return GetDialString("", dialingRule);
        }

        public string GetDialString(string lineAccess, string dialingRule)
        {
            var dialString = string.Empty;
            if (!DialInternal)
                dialString = lineAccess;
            dialString += dialingRule;
            dialString = dialString.Replace("E", CountryCode);
            dialString = dialString.Replace("F", AreaCode);
            dialString = dialString.Replace("G", Number + Extension);
            return dialString;
        }

        public override string ToString()
        {
            return ToString(PhoneNumberFormat.Default);
        }

        public string ToString(PhoneNumberFormat format)
        {
            // TODO: implement and use IPhoneNumberFormatter
            var formattedString = string.IsNullOrEmpty(CountryCode) ? string.Empty : "+";

            switch (format)
            {
                case PhoneNumberFormat.Default:
                    if (string.IsNullOrEmpty(Number) && string.IsNullOrEmpty(Extension))
                        return string.Empty;
                    formattedString += CountryCode + AreaCode + Number + Extension;
                    break;
                case PhoneNumberFormat.DIN_5008:
                    formattedString += CountryCode;
                    if (!string.IsNullOrEmpty(CountryCode) && !string.IsNullOrEmpty(AreaCode))
                        formattedString += " ";
                    formattedString += AreaCode;
                    if (!string.IsNullOrEmpty(AreaCode) && !string.IsNullOrEmpty(Number))
                        formattedString += " ";
                    formattedString += Number + "-" + Extension;
                    break;
                case PhoneNumberFormat.E_123:
                    formattedString += CountryCode;
                    if (!string.IsNullOrEmpty(CountryCode) && !string.IsNullOrEmpty(AreaCode))
                        formattedString += " ";
                    formattedString += AreaCode;
                    if (!string.IsNullOrEmpty(AreaCode) && !string.IsNullOrEmpty(Number))
                        formattedString += " ";
                    formattedString += Number + Extension;
                    break;
                case PhoneNumberFormat.Microsoft:
                    formattedString += CountryCode;
                    if (!string.IsNullOrEmpty(CountryCode) && !string.IsNullOrEmpty(AreaCode))
                        formattedString += " ";
                    if (!string.IsNullOrEmpty(AreaCode))
                        formattedString += "(" + AreaCode + ")";
                    if (!string.IsNullOrEmpty(AreaCode) && !string.IsNullOrEmpty(Number))
                        formattedString += " ";
                    formattedString += Number + Extension;
                    break;
                case PhoneNumberFormat.URI:
                    formattedString += CountryCode;
                    if (!string.IsNullOrEmpty(CountryCode) && !string.IsNullOrEmpty(AreaCode))
                        formattedString += "-";
                    formattedString += AreaCode;
                    if (!string.IsNullOrEmpty(AreaCode) && !string.IsNullOrEmpty(Number))
                        formattedString += "-";
                    formattedString += Number + Extension;
                    break;
                default:
                    Debug.Print("Invalid PhoneNumberFormat");
                    break;
            }

            return formattedString;
        }
    }
}